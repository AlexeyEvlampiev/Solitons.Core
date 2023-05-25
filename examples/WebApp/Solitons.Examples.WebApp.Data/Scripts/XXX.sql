/*
CREATE TYPE api.http_response AS (
    status INTEGER,
    headers HSTORE,
    body JSONB
);
*/

DROP TABLE IF EXISTS api.http_route;
CREATE TABLE api.http_route
(
	id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	version_regexp text NOT NULL DEFAULT('.*')
);


CREATE OR REPLACE FUNCTION api.http_invoke(p_method text, p_url text, p_headers hstore, p_body jsonb) 
RETURNS api.http_response
AS
$$
DECLARE
	v_client_version text;
	v_headers hstore := ''::hstore;
	v_response api.http_response;
BEGIN
	p_method := COALESCE(UPPER(TRIM(p_method)), '...');
	SELECT captures[1] INTO v_client_version
	FROM regexp_matches(p_url, '(?<=[?&](v|version)=)([^?&]{1,12})', 'gi') AS captures;
	
	IF NOT FOUND THEN
		RETURN (400, ''::hstore, jsonb_build_object(
			'message', 'Client version is required.'));
	END IF;
	
	v_headers := v_headers||hstore('version', v_client_version);
	
	
	if p_method = 'GET' THEN
		return (200, v_headers, p_body);
	end if;
	return (202, v_headers, p_body);
END;
$$ LANGUAGE plpgsql;


select * from api.http_invoke('XXX', '/hello?v=1720', 'a=>b'::hstore, '{}'::jsonb);