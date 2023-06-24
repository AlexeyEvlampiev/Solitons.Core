

CREATE OR REPLACE FUNCTION api.http_invoke(
	"method" text,
	address text, 
	headers hstore,
	content jsonb) RETURNS api.http_response
AS
$$
DECLARE
	v_client_external_id text := NULLIF(TRIM(headers->'SKYNET-IDENTITY'), '');
	v_client_version text := substring(address from '(?:(?:api-?)?version|v)=(\S+)');
	v_identity data.identity;	
	v_route api.http_route;
	v_request api.http_request;
	v_response api.http_response;
	v_sql text;
BEGIN

	IF v_client_external_id IS NULL THEN
		RETURN api.http_response_build(401, '', jsonb_build_object(
			'message', 'Authentication is required and has failed or not yet been provided.'));
	END IF;
	
	IF v_client_version IS NULL THEN
		RETURN api.http_response_build(400, '', jsonb_build_object(
			'message', 'The client version is required but was not provided in the request.'));
	END IF;
	
	--RAISE NOTICE 'Searching%', '';
	SELECT c_route.* INTO v_route
	FROM 
		data.identity AS c_identity
	INNER JOIN
		data.account AS c_account 
			ON c_account.object_id = c_identity.account_object_id
			AND c_account.deleted_utc IS NULL
	INNER JOIN
		api.http_route AS c_route
			ON "method" ~* c_route.method_regexp
			AND address ~* c_route.address_regexp
			AND v_client_version ~* c_route.version_regexp
			AND c_account.balance >= c_route.price
			AND c_route.deleted_utc IS NULL
	LEFT JOIN
		api.http_route_permission AS c_permission 
			ON c_permission.object_id IN (c_identity.object_id)
			AND c_permission.http_route_object_id = c_route.object_id
			AND c_permission.deleted_utc IS NULL
	WHERE 
		c_identity.deleted_utc IS NULL		
	AND LOWER(v_client_external_id) = LOWER(c_identity.id)
	AND (c_permission.object_id IS NOT NULL OR c_route.is_protected = false)
	ORDER BY 
		c_route.sequence_number DESC
	LIMIT 1;
	
	IF FOUND THEN	
		SELECT FORMAT('SELECT api.%s($1, $2, $3, $4)', 
			v_route.handler) 
		INTO v_sql;
		EXECUTE v_sql INTO v_response USING $1, $2, $3, $4;
		RETURN v_response;
	END IF;
		
	RETURN api.http_response_build(401, '', jsonb_build_object(
			'message', 'Not found'));

END;
$$ LANGUAGE 'plpgsql';