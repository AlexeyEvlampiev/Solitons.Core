GRANT USAGE ON SCHEMA data TO skynetdb_api;
GRANT USAGE ON SCHEMA extensions TO skynetdb_api;
GRANT USAGE ON SCHEMA system TO skynetdb_api;

GRANT USAGE ON SCHEMA api TO skynetdb_api;
GRANT USAGE ON SCHEMA data TO skynetdb_api;
GRANT USAGE ON SCHEMA system TO skynetdb_api;
GRANT SELECT, REFERENCES, TRIGGER ON ALL TABLES IN SCHEMA data TO skynetdb_api;
GRANT SELECT, REFERENCES, TRIGGER ON ALL TABLES IN SCHEMA api TO skynetdb_api;
GRANT SELECT, REFERENCES, TRIGGER ON ALL TABLES IN SCHEMA system TO skynetdb_api;
GRANT UPDATE (balance) ON TABLE data.account TO skynetdb_api;

