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






CREATE OR REPLACE FUNCTION data.split_roles_csv_to_array(_roles_csv text) RETURNS text[] 
AS
$$
DECLARE
	_roles text[];
	_non_existing_role varchar(150);
BEGIN
	SELECT ARRAY_AGG(DISTINCT TRIM(roles.name)) INTO _roles
	FROM regexp_split_to_table(_roles_csv, E'\\s*,\\s*') AS roles(name)
	WHERE roles.name ~ '\S';
	
	FOR _non_existing_role IN 
		SELECT roles.name
		FROM UNNEST(_roles) AS roles(name)
		LEFT JOIN mvw_role AS dbrole ON dbrole.name = roles.name
		WHERE dbrole.name IS NULL
	LOOP
		RAISE EXCEPTION '''%'' role does not.', _non_existing_role;
	END LOOP;
	
	RETURN _roles;
END;
$$ LANGUAGE 'plpgsql';


CREATE TYPE api.http_content_result AS (status int, content_type text, content text);

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
	
	PERFORM system.raise_exception_if_null_or_empty_argument(_organization_object_id, '_organization_object_id');
	PERFORM system.raise_exception_if_null_or_empty_argument(_email, '_email');
	
	IF NOT EXISTS(SELECT 1 FROM data.organization WHERE object_id = _organization_object_id) THEN
		RAISE EXCEPTION 'Specified organization is not registered.';
	END IF;
	
	IF NOT EXISTS(SELECT 1 FROM system.mvw_role WHERE name = _rolname AND is_group_role) THEN
		RAISE EXCEPTION '''%'' group role does not exist.', _rolname;
	END IF;


	INSERT INTO data.user(organization_object_id, email, role_name, role_full_name)	
	SELECT _organization_object_id, _email, r.name, r.full_name
	FROM system.mvw_role AS r
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
	,name varchar(1000) UNIQUE
) INHERITS(system.gcobject);

CREATE FUNCTION api.data_contract_upsert
(
	_object_id uuid,
	_name varchar(1000)
) RETURNS SETOF api.data_contract AS
$$
BEGIN
	PERFORM system.raise_exception_if_null_or_empty_argument(_object_id, '_object_id');
	PERFORM system.raise_exception_if_null_or_empty_argument(_name, '_name');
	RETURN QUERY
	INSERT INTO api.data_contract(object_id, name) VALUES
	(_object_id, TRIM(_name))
	ON CONFLICT(object_id) DO UPDATE SET name = EXCLUDED.name;
END;
$$ LANGUAGE 'plpgsql';


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
	base_address varchar(1000) NOT NULL DEFAULT('https://localhost:80') CHECK(base_address ~ '^https://\w')
) INHERITS (system.gcobject);

CREATE OR REPLACE FUNCTION api.http_service_upsert(
	_object_id uuid,
	_id system.natural_key, 
	_description text, 
	_current_version system.version, 
	_base_address varchar(1000)) RETURNS SETOF api.http_service AS
$$
BEGIN
	PERFORM system.raise_exception_if_null_or_empty_argument(_object_id, '_object_id');
	PERFORM system.raise_exception_if_null_argument(_id, '_id');
	PERFORM system.raise_exception_if_null_argument(_description, '_description');
	PERFORM system.raise_exception_if_null_argument(_current_version, '_current_version');
	PERFORM system.raise_exception_if_null_argument(_base_address, '_base_address');		

	RETURN QUERY
	INSERT INTO api.http_service(object_id, id, description, current_version, base_address)
	VALUES(_object_id, _id, _description, _current_version, _base_address)
	ON CONFLICT(object_id) DO UPDATE SET
		id = EXCLUDED.id, 
		description = EXCLUDED.description, 
		current_version = EXCLUDED.current_version, 
		base_address = EXCLUDED.base_address
	RETURNING *;
END;
$$ LANGUAGE 'plpgsql';	


CREATE TABLE api.http_event
(
	PRIMARY KEY(object_id),
	FOREIGN KEY(object_id) REFERENCES api.data_contract(object_id),
	authorized_roles text[] CHECK (authorized_roles IS NULL OR array_length(authorized_roles, 1) > 0),
	service_verson_regexp varchar(100) NOT NULL CHECK(service_verson_regexp ~ '\S+'),
	http_methods_regexp varchar(100) NOT NULL CHECK(http_methods_regexp ~ '\S+'),
	url_regexp varchar(1000) NOT NULL,
	trigger_function varchar(1000),
	request_body_data_contract_object_id uuid REFERENCES api.data_contract(object_id),
	response_body_data_contract_object_id uuid REFERENCES api.data_contract(object_id),
	CHECK(NOT system.is_empty(object_id))
) INHERITS (system.gcobject);



CREATE OR REPLACE FUNCTION api.http_event_upsert(
	_object_id uuid,
	_authorized_roles_csv text,
	_service_verson_regexp varchar(100),
	_http_methods_regexp varchar(100),
	_url_regexp varchar(1000),
	_trigger_function varchar(1000),
	_request_body_data_contract_object_id uuid DEFAULT(NULL),
	_response_body_data_contract_object_id uuid DEFAULT(NULL)) RETURNS SETOF api.http_event
AS
$$
DECLARE
	_authorized_roles text[];
BEGIN
	PERFORM system.raise_exception_if_null_or_empty_argument(_object_id, '_object_id');
	PERFORM system.raise_exception_if_null_or_empty_argument(_service_verson_regexp, '_service_verson_regexp');
	PERFORM system.raise_exception_if_null_or_empty_argument(_http_methods_regexp, '_http_methods_regexp');
	PERFORM system.raise_exception_if_null_or_empty_argument(_url_regexp, '_url_regexp');
	
	_trigger_function := NULLIF(TRIM(_trigger_function),'');

	IF (_trigger_function IS NOT NULL) AND NOT EXISTS(
		SELECT 1 
		FROM system.mvw_function 
		WHERE "name" = _trigger_function 
		AND "schema" = 'api') THEN
		RAISE EXCEPTION '''api.%'' function does not exist.', _trigger_function;
	END IF;

	SELECT data.split_roles_csv_to_array(_authorized_roles_csv) INTO _authorized_roles;
	IF array_length(_authorized_roles, 1) = 0 THEN
    	_authorized_roles := NULL;
	END IF;
		
	RETURN QUERY
	INSERT INTO api.http_event(
		object_id,
		authorized_roles,
		service_verson_regexp,
		http_methods_regexp,
		url_regexp,
		request_body_data_contract_object_id,
		response_body_data_contract_object_id) VALUES
	(
		_object_id,
		_authorized_roles,
		_service_verson_regexp,
		_http_methods_regexp,
		_url_regexp,
		_request_body_data_contract_object_id,
		_response_body_data_contract_object_id
	)
	ON CONFLICT(object_id) DO UPDATE SET
		authorized_roles = EXCLUDED.authorized_roles,
		service_verson_regexp = EXCLUDED.service_verson_regexp,
		http_methods_regexp = EXCLUDED.http_methods_regexp,
		url_regexp = EXCLUDED.url_regexp,
		request_body_data_contract_object_id = EXCLUDED.request_body_data_contract_object_id,
		response_body_data_contract_object_id = EXCLUDED.response_body_data_contract_object_id
	RETURNING *;
END;
$$ LANGUAGE 'plpgsql';





/*

CREATE TABLE api.http_trigger_queue
(
	id bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
	PRIMARY KEY(http_event_object_id),
	http_event_object_id uuid NOT NULL REFERENCES api.http_trigger(http_event_object_id),
	event_args json NOT NULL,
	request_body text,
	result api.http_content_result	
) INHERITS (system.gcobject);


CREATE OR REPLACE FUNCTION api.customer_get(
	_args jsonb,
	_content_type character varying,
	_content jsonb)
    RETURNS SETOF api.http_content_result 
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


*/



CREATE OR REPLACE VIEW api.vw_data_contract AS
SELECT 
	dc.name,
	(http_event_args.object_id IS NOT NULL) AS is_http_event_args, 
	(EXISTS(SELECT 1 FROM api.http_event WHERE request_body_data_contract_object_id = dc.object_id)) AS is_http_event_request, 
	(EXISTS(SELECT 1 FROM api.http_event WHERE response_body_data_contract_object_id = dc.object_id)) AS is_http_event_response, 
	dc.object_id,
	dc.created_utc	
FROM api.data_contract AS dc
LEFT JOIN api.http_event AS http_event_args ON http_event_args.object_id = dc.object_id
LEFT JOIN api.http_event AS http_event_request ON http_event_request.request_body_data_contract_object_id = dc.object_id
LEFT JOIN api.http_event AS http_event_response ON http_event_request.response_body_data_contract_object_id = dc.object_id
WHERE dc.deleted_utc IS NULL;




CREATE OR REPLACE FUNCTION api.customer_get(p_request jsonb) RETURNS jsonb 
AS
$$
DECLARE 
	v_customer_object_id uuid := p_request->>'oid';
BEGIN
	PERFORM system.raise_exception_if_null_or_empty_argument(v_customer_object_id, 'oid');
	RETURN jsonb_build_object(
			'oid', v_customer_object_id,
			'id', 'Customer ID goes here',
			'email', 'Customer email goes here');
	
END;
$$ LANGUAGE 'plpgsql' STABLE;


CREATE OR REPLACE FUNCTION api.weather_forecast_get(p_request jsonb) RETURNS jsonb 
AS
$$
DECLARE 
	v_summaries text[] := ARRAY['Freezing', 'Bracing', 'Chilly', 'Cool', 'Mild', 'Warm', 'Balmy', 'Hot', 'Sweltering', 'Scorching'];
	v_items jsonb[];
BEGIN
	SELECT  array_agg(jsonb_build_object(
		'date', now() + (index||' days')::interval,
		'temp', (-20 + floor(random() * 75)::int),
		'summary', v_summaries[floor(random() * 10+1)::int]))
	INTO v_items
	FROM generate_series(0,5) AS sample("index");
	
	RETURN jsonb_build_object('items', v_items);
END;
$$ LANGUAGE 'plpgsql';


CREATE OR REPLACE FUNCTION api.image_get(p_request jsonb) RETURNS jsonb 
AS
$$
DECLARE 
	v_image_object_id uuid := p_request->>'oid';
BEGIN
	PERFORM system.raise_exception_if_null_or_empty_argument(v_image_object_id, 'oid');	
	SET LOCAL intervalstyle = 'iso_8601';
	RETURN jsonb_build_object(
		'relativePath', 'images/'||v_image_object_id,
		'accessTimeWindow','5 minutes'::interval,
		'allowAllIpAddresses', false);
END;
$$ LANGUAGE 'plpgsql';