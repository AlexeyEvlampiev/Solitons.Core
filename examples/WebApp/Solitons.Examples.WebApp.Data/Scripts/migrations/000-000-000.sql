CREATE SCHEMA IF NOT EXISTS system;
CREATE SCHEMA IF NOT EXISTS data;
CREATE SCHEMA IF NOT EXISTS api;

CREATE TABLE system.gc_object
(
	object_id uuid NOT NULL DEFAULT(gen_random_uuid()),
	created_utc timestamp NOT NULL DEFAULT now(),
	deleted_utc timestamp,
	CONSTRAINT gc_object_abstract_base  CHECK(false) NO INHERIT
);
COMMENT ON TABLE system.gc_object IS 'This table serves as an abstract base table for garbage collection objects, which can be deleted based on the deleted_utc timestamp.';
COMMENT ON COLUMN system.gc_object.object_id IS 'Primary key for the garbage collection object, stored as a UUID.';
COMMENT ON COLUMN system.gc_object.created_utc IS 'Timestamp indicating when the garbage collection object was created.';
COMMENT ON COLUMN system.gc_object.deleted_utc IS 'Timestamp indicating when the garbage collection object was deleted.';
COMMENT ON CONSTRAINT gc_object_abstract_base ON system.gc_object IS 'Check constraint that prevents data from being inserted directly into this table.';



CREATE OR REPLACE FUNCTION public.as_uuid(input_text text)
RETURNS uuid 
LANGUAGE 'sql'
IMMUTABLE
AS
$$
  SELECT input_text::uuid
  FROM (SELECT input_text) AS t(input_text)
  WHERE  TRIM(input_text) ~ '^[0-9a-f]{8}-?[0-9a-f]{4}-?[0-9a-f]{4}-?[0-9a-f]{4}-?[0-9a-f]{12}$'
$$;



CREATE TYPE api.http_request AS (
	method text, 
	uri text, 
	headers hstore, 
	body jsonb);
COMMENT ON TYPE api.http_request IS 'Represents an HTTP request message.';
COMMENT ON COLUMN api.http_request.method IS 'The HTTP method used in the request (e.g. GET, POST, etc.).';
COMMENT ON COLUMN api.http_request.uri IS 'The URI of the requested resource.';
COMMENT ON COLUMN api.http_request.headers IS 'A hstore object containing the headers of the request.';
COMMENT ON COLUMN api.http_request.body IS 'The body of the request.';


CREATE TYPE api.http_response AS (
	status_code integer, 
	status_text text, 
	headers hstore, 
	body jsonb);
COMMENT ON TYPE api.http_response IS 'Represents an HTTP response message.';
COMMENT ON COLUMN api.http_response.status_code IS 'The HTTP status code returned by the server.';
COMMENT ON COLUMN api.http_response.status_text IS 'The reason phrase associated with the status code.';
COMMENT ON COLUMN api.http_response.headers IS 'A JSON object containing the headers of the response.';
COMMENT ON COLUMN api.http_response.body IS 'The body of the response.';



CREATE TABLE api.http_route
(
	"id" BIGINT GENERATED ALWAYS AS IDENTITY,
	method_regex_pattern text NOT NULL DEFAULT('^get$'),
	uri_regex_pattern text NOT NULL,
	version_regex_pattern text NOT NULL DEFAULT('^\d{1,3}(\.\d{1,3}){1,3}$'),
	"function" text NOT NULL,
	command text GENERATED ALWAYS AS ('SELECT * FROM api.'||"function"||'($1);') STORED,
	isolation_level text NOT NULL DEFAULT('ReadCommitted'),
	PRIMARY KEY(object_id),
	CONSTRAINT method_regex_pattern CHECK (NOT '' ~ method_regex_pattern),
	CONSTRAINT uri_regex_pattern_is_valid CHECK (NOT '' ~ uri_regex_pattern),
	CONSTRAINT version_regex_pattern_is_valid CHECK (NOT '' ~ version_regex_pattern)
) INHERITS(system.gc_object);

CREATE UNIQUE INDEX ix_http_route_id ON api.http_route("id" DESC) 
INCLUDE(method_regex_pattern, uri_regex_pattern, version_regex_pattern)
WHERE deleted_utc IS NULL;

CREATE UNIQUE INDEX ix_http_route_object_id ON api.http_route(object_id) 
INCLUDE(method_regex_pattern, uri_regex_pattern, version_regex_pattern)
WHERE deleted_utc IS NULL;




CREATE TABLE api.user
(
	external_id varchar(150) NOT NULL UNIQUE,
	PRIMARY KEY(object_id),
	CONSTRAINT external_id_is_valid CHECK (external_id ~ '^\S{8,100}$')
) INHERITS(system.gc_object);

CREATE UNIQUE INDEX ux_api_user_object_id ON api.user(external_id)
WHERE deleted_utc IS NULL;


CREATE TABLE api.role
(
	id varchar(150) NOT NULL UNIQUE,
	PRIMARY KEY(object_id)	
) INHERITS(system.gc_object);

CREATE UNIQUE INDEX ux_api_role_object_id ON api.role(id)
WHERE deleted_utc IS NULL;


CREATE TABLE api.user_role
(
	user_object_id uuid NOT NULL REFERENCES api.user(object_id),
	role_object_id uuid NOT NULL REFERENCES api.role(object_id),
	PRIMARY KEY(user_object_id, role_object_id)	
);


CREATE TABLE api.http_route_role
(
	http_route_object_id uuid NOT NULL REFERENCES api.http_route(object_id),
	role_object_id uuid NOT NULL REFERENCES api.role(object_id),
	PRIMARY KEY(http_route_object_id, role_object_id)	
);