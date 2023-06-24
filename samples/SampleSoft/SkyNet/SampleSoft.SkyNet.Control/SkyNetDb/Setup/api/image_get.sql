CREATE OR REPLACE FUNCTION api.image_get(
	"method" text,
	address text, 
	headers hstore,
	content jsonb)
RETURNS api.http_response
AS
$$
DECLARE
	v_oid_text text = substring($2 from 'images/([^/?&]+)');
	v_object_id uuid := system.as_uuid(v_oid_text);
	v_content jsonb;
BEGIN
	SELECT jsonb_build_object('oid', object_id)
	INTO v_content
	FROM data.image 
	WHERE object_id = v_object_id
	AND deleted_utc IS NULL;
	
	IF FOUND THEN
		RETURN api.http_response_build(200, '', v_content);
	END IF;
	
	RETURN api.http_response_build(400, '', '{}');
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