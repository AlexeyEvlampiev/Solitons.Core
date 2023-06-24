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


CREATE TYPE api.http_response AS
(
	status_code int,
	headers hstore,
	"content" jsonb
);

CREATE TABLE api.http_route
(
	PRIMARY KEY(object_id),
	version_regexp text NOT NULL DEFAULT('^'),
	method_regexp text NOT NULL DEFAULT('^get$'),
	url_regexp text NOT NULL,
	"handler" text NOT NULL
) INHERITS(system.gcobject);


CREATE OR REPLACE FUNCTION api.check_http_route()
RETURNS TRIGGER AS $$
DECLARE 
  test TEXT;
BEGIN
  BEGIN
    -- Check if the regular expressions are valid by trying to match a string
    test := REGEXP_REPLACE('test', NEW.version_regexp, '');
    test := REGEXP_REPLACE('test', NEW.method_regexp, '');
    test := REGEXP_REPLACE('test', NEW.url_regexp, '');
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
