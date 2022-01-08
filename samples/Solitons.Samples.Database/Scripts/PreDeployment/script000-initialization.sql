CREATE SCHEMA IF NOT EXISTS system;
CREATE SCHEMA IF NOT EXISTS extensions;
CREATE SCHEMA IF NOT EXISTS data;
CREATE SCHEMA IF NOT EXISTS api;

DO $$
BEGIN
	EXECUTE 'ALTER DATABASE '||current_database()||' SET search_path TO data, api, extensions;'; 	
END;
$$;

CREATE EXTENSION IF NOT EXISTS pgcrypto SCHEMA extensions;
CREATE EXTENSION IF NOT EXISTS postgis SCHEMA extensions;

CREATE OR REPLACE VIEW system.pg_type AS
SELECT FORMAT('%s.%s',nspname, typname) AS full_name, nspname, t.*
FROM pg_type AS t
INNER JOIN pg_catalog.pg_namespace AS nt ON (nt.oid = t.typnamespace)
WHERE nspname NOT IN ('pg_catalog', 'pg_toast', 'information_schema', 'extensions', 'public');

