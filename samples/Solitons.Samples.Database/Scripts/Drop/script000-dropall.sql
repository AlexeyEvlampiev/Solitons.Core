DO $$
BEGIN
	RAISE NOTICE 'Dropping database objects...';	
END;
$$;

DROP SCHEMA IF EXISTS api CASCADE;
DROP SCHEMA IF EXISTS data CASCADE;
DROP SCHEMA IF EXISTS system CASCADE;