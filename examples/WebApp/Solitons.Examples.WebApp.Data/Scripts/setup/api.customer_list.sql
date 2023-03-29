CREATE OR REPLACE FUNCTION api.customer_list(p_request api.http_request) 
RETURNS api.http_response
LANGUAGE 'plpgsql'
AS
$body$
DECLARE
	v_body jsonb;
	v_headers hstore := ''::hstore;
BEGIN

	v_body := jsonb_build_object('text', 'hello world!');
	RETURN (ROW(200, 'OK', null, v_body))::api.http_response;
END;
$body$;





INSERT INTO api.http_route(
	object_id, 
	version_regex_pattern, 
	uri_regex_pattern, 
	"function")
VALUES(
	'c1dba4b4-5f5e-47d0-b30e-eff0c24e04bf', 
	'^\d{1,3}(\.\d{1,3}){1,3}$', 
	'/customers', 
	'customer_list')
ON CONFLICT(object_id) DO UPDATE SET 	
	version_regex_pattern = EXCLUDED.version_regex_pattern,
	uri_regex_pattern = EXCLUDED.uri_regex_pattern,
	"function" = EXCLUDED.function,
	deleted_utc = null;