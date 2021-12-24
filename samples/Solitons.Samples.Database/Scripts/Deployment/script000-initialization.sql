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

CREATE DOMAIN data.natural_key AS varchar(150) CHECK(VALUE ~ '^\S.*\S$');

CREATE DOMAIN system.email AS varchar(150)
  CHECK ( value ~ '^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$' );

CREATE TYPE data.loglevel AS ENUM ('critical', 'error', 'warning', 'info', 'verbose'); 
COMMENT ON DOMAIN data.natural_key IS 'Solitons Sample log levels';

CREATE TABLE system.gcobject(
	object_id uuid  NOT NULL  DEFAULT extensions.gen_random_uuid()
	,created_utc timestamp NOT NULL DEFAULT now()	
	,deleted_utc timestamp
	,CHECK(false) NO INHERIT
);
COMMENT ON TABLE system.gcobject IS 'GC- managed entry';

CREATE OR REPLACE FUNCTION data.role_full_name(_name data.natural_key) RETURNS data.natural_key
AS
$$
	SELECT FORMAT('%s_%s', current_database(), _name)::data.natural_key;
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
	,id data.natural_key NOT NULL UNIQUE
	,email system.email NOT NULL UNIQUE
) INHERITS(system.gcobject);


CREATE TABLE IF NOT EXISTS data.user
(	
	email system.email NOT NULL UNIQUE
	,organization_object_id uuid NOT NULL REFERENCES data.organization(object_id)
	,role_name data.natural_key NOT NULL
	,role_full_name data.natural_key NOT NULL CHECK(role_full_name = data.role_full_name(role_name))
	,PRIMARY KEY(object_id)
) INHERITS(system.user);



CREATE OR REPLACE FUNCTION data.user_upsert(
	_organization_object_id uuid, 
	_email system.email, 
	_rolname data.natural_key DEFAULT(NULL)) RETURNS data.user 
AS
$$
DECLARE 
	_host_rolname data.natural_key;
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
	,content_type data.natural_key NOT NULL CHECK(content_type IN ('application/xml','application/json'))
	,"schema" text CHECK(
		CASE content_type
			WHEN 'application/xml' THEN ("schema" IS NULL OR "schema"::xml IS NOT NULL)
			WHEN 'application/json' THEN ("schema" IS NULL OR "schema"::json IS NOT NULL)
			ELSE false
		END)
	,PRIMARY KEY(data_contract_object_id, content_type)
);




CREATE TABLE api.http_event
(
	PRIMARY KEY(object_id)
	,dotnet_type varchar(1000)
	,supported_content_types text[] NOT NULL
	,version_regexp varchar(100) NOT NULL 
	,method_regexp varchar(100) NOT NULL 
	,url_template varchar(500) NOT NULL 
	,payload_object_id uuid
	,payload_dotnet_type varchar(1000)
	,response_object_id uuid
	,response_dotnet_type varchar(1000)
) INHERITS(system.gcobject);

CREATE TABLE api.http_trigger
(
	PRIMARY KEY(object_id)
	,procedure varchar(1000) NOT NULL
) INHERITS(api.http_event);


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


