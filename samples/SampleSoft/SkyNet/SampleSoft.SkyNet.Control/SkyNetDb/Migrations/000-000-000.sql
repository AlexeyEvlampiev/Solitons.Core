CREATE SCHEMA IF NOT EXISTS system;
CREATE SCHEMA IF NOT EXISTS data;
CREATE SCHEMA IF NOT EXISTS api;

COMMENT ON SCHEMA system IS 'Reference types and data. Includes cloud provider metadata such as Azure regions, Azure batch pool VM types etc.';
COMMENT ON SCHEMA data IS 'SkyNet data. Includes Physical and Logical database layer objects';
COMMENT ON SCHEMA api IS 'SkyNet api layer objects';

REVOKE EXECUTE ON ALL FUNCTIONS IN SCHEMA api FROM PUBLIC;

CREATE TABLE IF NOT EXISTS system.gcobject(
	object_id uuid  NOT NULL  DEFAULT extensions.gen_random_uuid()
	,created_utc timestamp NOT NULL DEFAULT now()	
	,deleted_utc timestamp
	,CHECK(false) NO INHERIT
);
COMMENT ON TABLE system.gcobject IS 'GC- managed entry';


CREATE TABLE data.account
(
	"id" text NOT NULL,
	"description" text NOT NULL DEFAULT(''),
	balance NUMERIC(10, 2) NOT NULL DEFAULT(0.0),
	PRIMARY KEY(object_id)
) INHERITS(system.gcobject);



CREATE TABLE data.identity
(
	PRIMARY KEY(object_id),
	"id" text NOT NULL UNIQUE,	
	account_object_id uuid NOT NULL REFERENCES data.account(object_id),
	CHECK(false) NO INHERIT
) INHERITS(system.gcobject);


CREATE TABLE data.email
(
	PRIMARY KEY(object_id),
	account_object_id uuid NOT NULL REFERENCES data.account(object_id),
	CONSTRAINT id_is_valid_email CHECK (LOWER("id") ~ '^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$'),
	CONSTRAINT id_should_be_of_lower_case CHECK(LOWER("id") = "id")
) INHERITS(data.identity);


CREATE TABLE api.http_route
(
	PRIMARY KEY(object_id),
	sequence_number BIGINT GENERATED ALWAYS AS IDENTITY,	
	version_regexp text NOT NULL DEFAULT('^'),
	method_regexp text NOT NULL DEFAULT('^get$'),
	address_regexp text NOT NULL,
	is_protected bool NOT NULL DEFAULT(true),
	price NUMERIC(10, 2) NOT NULL,
	"handler" text NOT NULL
) INHERITS(system.gcobject);
CREATE INDEX ix_httproute_sequencenumber ON api.http_route(sequence_number)
WHERE deleted_utc IS NULL;


CREATE TABLE api.http_route_permission
(	
	http_route_object_id uuid NOT NULL REFERENCES api.http_route(object_id)	
) INHERITS(system.gcobject);

CREATE UNIQUE INDEX ux_entity_httproute ON api.http_route_permission(object_id, http_route_object_id)
WHERE deleted_utc IS NULL;

CREATE UNIQUE INDEX ux_httproute_entity ON api.http_route_permission(http_route_object_id, object_id)
WHERE deleted_utc IS NULL;


CREATE OR REPLACE FUNCTION api.check_http_route()
RETURNS TRIGGER AS $$
DECLARE 
  test TEXT;
BEGIN
  BEGIN
    -- Check if the regular expressions are valid by trying to match a string
    test := REGEXP_REPLACE('test', NEW.version_regexp, '');
    test := REGEXP_REPLACE('test', NEW.method_regexp, '');
    test := REGEXP_REPLACE('test', NEW.address_regexp, '');
  EXCEPTION WHEN others THEN
    RAISE EXCEPTION 'Invalid regular expression in version_regexp, method_regexp, or url_regexp.';
  END;

  -- Check if the function exists in the 'api' schema
  IF NOT EXISTS (
    SELECT 1 
    FROM pg_proc p
    JOIN pg_namespace n ON p.pronamespace = n.oid 
    WHERE n.nspname = 'api' AND p.proname = NEW.handler
  ) THEN
    RAISE EXCEPTION 'Function named "%" does not exist in the "api" schema.', NEW.handler;
  END IF;
  
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER check_http_route
BEFORE INSERT OR UPDATE ON api.http_route
FOR EACH ROW EXECUTE FUNCTION api.check_http_route();


CREATE TYPE api.http_request AS
(
	"method" text,
	address text,
	headers hstore,
	"content" jsonb
);


CREATE TYPE api.http_response AS
(
	status_code int,
	headers hstore,
	"content" jsonb
);


CREATE OR REPLACE FUNCTION api.http_request_build(
	"method" text,
	address text,
	headers hstore,
	"content" jsonb
) RETURNS api.http_request AS $$
SELECT ROW($1, $2, $3, $4)::api.http_request;
$$ LANGUAGE sql;


CREATE OR REPLACE FUNCTION api.http_response_build(
    status_code int,
    headers hstore,
    "content" jsonb
) RETURNS api.http_response AS $$
SELECT ROW($1, $2, $3)::api.http_response;
$$ LANGUAGE sql;