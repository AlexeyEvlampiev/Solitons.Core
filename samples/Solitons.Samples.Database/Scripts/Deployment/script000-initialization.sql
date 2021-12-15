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

CREATE DOMAIN data.email AS varchar(150)
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


CREATE TABLE IF NOT EXISTS system.user
(
	id data.email NOT NULL PRIMARY KEY
	,UNIQUE (object_id)
) INHERITS(system.gcobject);


CREATE TYPE api.content_result AS (status int, content_type text, content text);

CREATE TABLE IF NOT EXISTS data.customer
(
	PRIMARY KEY(object_id)
	,id data.natural_key NOT NULL UNIQUE
	,email data.email NOT NULL UNIQUE
) INHERITS(system.gcobject);


CREATE TABLE IF NOT EXISTS data.user
(
	PRIMARY KEY(object_id)
	,id data.email NOT NULL
	,customer_object_id uuid NOT NULL REFERENCES data.customer(object_id)
) INHERITS(system.user);





-- FUNCTION: public.utcnow(jsonb, character varying, jsonb)

-- DROP FUNCTION public.utcnow(jsonb, character varying, jsonb);

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


