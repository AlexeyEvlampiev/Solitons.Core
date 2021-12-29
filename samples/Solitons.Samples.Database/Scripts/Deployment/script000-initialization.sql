CREATE SCHEMA IF NOT EXISTS system;
CREATE SCHEMA IF NOT EXISTS data;
CREATE SCHEMA IF NOT EXISTS api;

COMMENT ON SCHEMA system IS 'Reference types and data. Includes cloud provider metadata such as Azure regions, Azure batch pool VM types etc.';
COMMENT ON SCHEMA data IS 'Solitons Sample data. Includes Physical and Logical database layer objects';
COMMENT ON SCHEMA api IS 'Solitons Sample api layer objects';

DO $$
BEGIN
	EXECUTE 'ALTER DATABASE '||current_database()||' SET search_path TO data, api, extensions;'; 		
END;
$$;


CREATE EXTENSION IF NOT EXISTS pgcrypto SCHEMA extensions; 
CREATE EXTENSION IF NOT EXISTS postgis SCHEMA extensions; 
CREATE EXTENSION IF NOT EXISTS hstore SCHEMA extensions; 


REVOKE EXECUTE ON ALL FUNCTIONS IN SCHEMA api FROM PUBLIC;


CREATE TABLE system.gcobject(
	object_id uuid  NOT NULL  DEFAULT extensions.gen_random_uuid()
	,created_utc timestamp NOT NULL DEFAULT now()	
	,deleted_utc timestamp
	,CHECK(false) NO INHERIT
);
COMMENT ON TABLE system.gcobject IS 'GC- managed entry';

CREATE OR REPLACE FUNCTION data.role_full_name(_name system.natural_key) RETURNS system.natural_key
AS
$$
	SELECT FORMAT('%s_%s', current_database(), _name)::system.natural_key;
$$ LANGUAGE 'sql' IMMUTABLE;



CREATE TABLE IF NOT EXISTS system.user
(
	email system.email NOT NULL UNIQUE	
	,organization_object_id uuid NOT NULL
	,PRIMARY KEY(object_id)
) INHERITS(system.gcobject);



CREATE MATERIALIZED VIEW data.mvw_role AS
SELECT 
	rolname AS full_name
	,REGEXP_REPLACE(rolname, '^[^_]+[_]', '') AS name
	,rolcanlogin AS is_login_role
	,(NOT rolcanlogin) AS is_group_role
FROM pg_catalog.pg_roles AS pgr
WHERE pgr.rolname LIKE current_database()||'\_%'
ORDER BY rolname;

CREATE UNIQUE INDEX ux_mvw_role ON data.mvw_role (name);
CREATE UNIQUE INDEX ux_mvw_full_name ON data.mvw_role (full_name);


CREATE TYPE api.content_result AS (status int, content_type text, content text);

CREATE TABLE IF NOT EXISTS data.organization
(
	PRIMARY KEY(object_id)
	,id system.natural_key NOT NULL UNIQUE
	,email system.email NOT NULL UNIQUE
) INHERITS(system.gcobject);


CREATE TABLE IF NOT EXISTS data.user
(	
	email system.email NOT NULL UNIQUE
	,organization_object_id uuid NOT NULL REFERENCES data.organization(object_id)
	,role_name system.natural_key NOT NULL
	,role_full_name system.natural_key NOT NULL CHECK(role_full_name = data.role_full_name(role_name))
	,PRIMARY KEY(object_id)
) INHERITS(system.user);



CREATE OR REPLACE FUNCTION data.user_upsert(
	_organization_object_id uuid, 
	_email system.email, 
	_rolname system.natural_key DEFAULT(NULL)) RETURNS data.user 
AS
$$
DECLARE 
	_host_rolname system.natural_key;
	_user data.user;
BEGIN
	_rolname := COALESCE(_rolname, 'prospect');
	
	IF _organization_object_id IS NULL THEN
		RAISE EXCEPTION 'Organization object ID is required.';
	END IF;	
	
	IF NOT EXISTS(SELECT 1 FROM data.organization WHERE object_id = _organization_object_id) THEN
		RAISE EXCEPTION 'Specified organization is not registered.';
	END IF;

	
	IF _email IS NULL THEN
		RAISE EXCEPTION 'User email is required.';
	END IF;

	
	IF NOT EXISTS(SELECT 1 FROM data.mvw_role WHERE name = _rolname AND is_group_role) THEN
		RAISE EXCEPTION '''%'' group role does not exist.', _rolname;
	END IF;


	INSERT INTO data.user(organization_object_id, email, role_name, role_full_name)	
	SELECT _organization_object_id, _email, r.name, r.full_name
	FROM data.mvw_role AS r
	WHERE name = _rolname
	ON CONFLICT(email) DO UPDATE SET 
		role_name = EXCLUDED.role_name
		,role_full_name = EXCLUDED.role_full_name
	RETURNING * INTO _user;

	IF _user IS NULL THEN
		RAISE EXCEPTION 'Insert/update failed for ''%'' (role: ''%'')', _user, _rolname;
	END IF;

	RETURN _user;
END;
$$ LANGUAGE 'plpgsql';








CREATE OR REPLACE FUNCTION api.set_user_context(_user_object_id uuid)
    RETURNS void
    LANGUAGE 'plpgsql'

AS $$

BEGIN
	
	SET ROLE sampledb_admin;
END;
$$;


CREATE TABLE api.data_contract
(
	PRIMARY KEY(object_id)
	,dotnet_type varchar(1000) UNIQUE
) INHERITS(system.gcobject);


CREATE TABLE api.data_contract_content_type
(
	data_contract_object_id uuid NOT NULL REFERENCES api.data_contract(object_id)
	,content_type system.natural_key NOT NULL CHECK(content_type IN ('application/xml','application/json'))
	,"schema" text CHECK(
		CASE content_type
			WHEN 'application/xml' THEN ("schema" IS NULL OR "schema"::xml IS NOT NULL)
			WHEN 'application/json' THEN ("schema" IS NULL OR "schema"::json IS NOT NULL)
			ELSE false
		END)
	,PRIMARY KEY(data_contract_object_id, content_type)
);


CREATE TABLE api.http_service 
(
	PRIMARY KEY(object_id),
	id system.natural_key NOT NULL UNIQUE,	
	description text NOT NULL,
	current_version system.version NOT NULL DEFAULT('1.0'),
	authorized_roles text[],
	base_address varchar(1000) NOT NULL DEFAULT('https://localhost:80') CHECK(base_address ~ '^https://\w')
) INHERITS (system.gcobject);

CREATE OR REPLACE FUNCTION api.http_service_upsert(
	_object_id uuid,
	_id system.natural_key, 
	_description text, 
	_current_version system.version, 
	_authorized_roles_csv text,
	_base_address varchar(1000)) RETURNS SETOF api.http_service AS
$$
DECLARE
	_authorized_roles text[];
BEGIN
	PERFORM system.raise_exception_if_null_or_empty_argument(_object_id, '_object_id');
	PERFORM system.raise_exception_if_null_argument(_id, '_id');
	PERFORM system.raise_exception_if_null_argument(_description, '_description');
	PERFORM system.raise_exception_if_null_argument(_current_version, '_current_version');
	PERFORM system.raise_exception_if_null_argument(_base_address, '_base_address');

	SELECT ARRAY_AGG(TRIM(authorized.role)) INTO _authorized_roles
	FROM regexp_split_to_table(_authorized_roles_csv, E'\\s*,\\s*') AS authorized(role)
	WHERE authorized.role ~ '\S';

	RETURN QUERY
	INSERT INTO api.http_service(object_id, id, description, current_version, authorized_roles, base_address)
	VALUES(_object_id, _id, _description, _current_version, _authorized_roles, _base_address)
	ON CONFLICT(object_id) DO UPDATE SET
		id = EXCLUDED.id, 
		description = EXCLUDED.description, 
		current_version = EXCLUDED.current_version, 
		host = EXCLUDED.host
	RETURNING *;
END;
$$ LANGUAGE 'plpgsql';	

/*
DROP DOMAIN public.version;
CREATE DOMAIN public.version AS varchar(25) CHECK (value ~ '^\d+(\.\d+){0,3}$');
select '3.2.1'::public.version;
*/

--SELECT * FROM api.http_service_upsert('10000000-0000-0000-0000-000000000000', 'id', 'description', '3.2.1', 'https://localhost');


CREATE TABLE api.http_event_type
(
	PRIMARY KEY(object_id),
	FOREIGN KEY(object_id) REFERENCES api.data_contract(object_id),
	authorized_roles_csv varchar(1000) NOT NULL DEFAULT('admin'),
	dontnet_event_args_type varchar(1000) NOT NULL,
	service_verson_regexp varchar(100) NOT NULL,
	http_methods_regexp varchar(100) NOT NULL,
	url_regexp varchar(1000) NOT NULL,
	request_body_data_contract_object_id uuid REFERENCES api.data_contract(object_id),
	response_body_data_contract_object_id uuid REFERENCES api.data_contract(object_id)
) INHERITS (system.gcobject);



CREATE TABLE api.http_trigger 
(
	http_event_type_object_id uuid PRIMARY KEY REFERENCES api.http_event_type(object_id),
	trigger_function varchar(1000) NOT NULL
) ;

CREATE TABLE api.http_service_event_type
(
	http_service_object_id uuid NOT NULL REFERENCES api.http_service(object_id),
	http_event_type_object_id uuid NOT NULL REFERENCES api.http_event_type(object_id),
	PRIMARY KEY(http_service_object_id, http_event_type_object_id)
);


CREATE TABLE api.http_trigger_queue
(
	id bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
	PRIMARY KEY(http_event_type_object_id),
	http_event_type_object_id uuid NOT NULL REFERENCES api.http_trigger(http_event_type_object_id),
	event_args json NOT NULL,
	request_body text,
	result api.content_result	
) INHERITS (system.gcobject);


CREATE OR REPLACE FUNCTION api.customer_get(
	_args jsonb,
	_content_type character varying,
	_content jsonb)
    RETURNS SETOF api.content_result 
    LANGUAGE 'plpgsql'

AS $$
DECLARE 
	_object_id uuid := _args->>'id';
	_response jsonb;
BEGIN
	
	SELECT jsonb_build_object(
		'oid', object_id
		,'id', id
		,'email', email) INTO _response
	FROM data.customer AS c
	WHERE c.object_id = _object_id;

	IF NOT FOUND THEN
		RETURN QUERY SELECT 400, 'text/plain', FORMAT('%s customer not found', _object_id)::text;
	END IF;
	
	RETURN QUERY SELECT 200, 'application/json', _response::text;
END;
$$;


