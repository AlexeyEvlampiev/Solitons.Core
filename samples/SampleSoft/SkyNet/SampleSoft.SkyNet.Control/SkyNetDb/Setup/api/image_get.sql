CREATE OR REPLACE FUNCTION api.image_get(
	p_method text,
	p_address text, 
	p_headers hstore,
	p_content jsonb)
RETURNS api.http_response
AS
$$
DECLARE
	v_oid_text text = substring($2 from 'images/([^/?&]+)');
	v_object_id uuid := system.as_uuid(v_oid_text);
	v_result api.http_response;
BEGIN
	SELECT jsonb_build_object('oid', object_id)
	INTO v_result.content
	FROM data.image 
	WHERE object_id = v_object_id
	AND deleted_utc IS NULL;
	
	v_result.status_code := 200;
	v_result.headers := hstore('');

	IF FOUND THEN
		RETURN v_result;
	END IF;
	
	v_result.status_code := 400;
	v_result.headers := hstore('');
	v_result.content := jsonb_build_object();
	RETURN v_result;
END;
$$ LANGUAGE 'plpgsql';


INSERT INTO api.http_route(
	object_id, 
	version_regexp, 
	method_regexp, 
	address_regexp, 
	is_protected, 
	price, 
	"handler")
VALUES(
	'10ed331b-b08d-459d-8dc2-a5b365d4825d',
	'^',
	'^GET$',
	'(?i)(^|\W)images/([^?&])+',
	false,
	0.01,
	'image_get')
ON CONFLICT(object_id) DO UPDATE SET 
	version_regexp = EXCLUDED.version_regexp, 
	method_regexp = EXCLUDED.method_regexp, 
	address_regexp = EXCLUDED.address_regexp, 
	is_protected = EXCLUDED.is_protected, 
	price = EXCLUDED.price, 
	"handler" = EXCLUDED.handler;