CREATE OR REPLACE FUNCTION api.http_invoke(
	p_method text, 
	p_uri text, 
	p_headers hstore DEFAULT (NULL), 
	p_body jsonb DEFAULT (NULL)) 
RETURNS api.http_response
LANGUAGE 'plpgsql'
AS
$body$
DECLARE
	v_version text := substring(p_uri from '(?<=[?&])v(?:ersion)?=([^&]+)');
	v_route_object_id uuid := public.as_uuid(v_version);
	v_headers hstore := 'Content-Type=>application/json'::hstore;
	v_request api.http_request;
	v_response api.http_response;
	v_route api.http_route;
BEGIN	
	IF v_route_object_id IS NOT NULL THEN	
		SELECT * INTO v_route
		FROM api.http_route
		WHERE object_id = v_route_object_id
		AND deleted_utc IS NULL;
		IF NOT FOUND THEN
			RETURN ROW(
				404, 
				'Route not found.', 
				v_headers, 
				jsonb_build_object('error', 'Invalid route.'))::api.http_response;
		END IF;
	ELSE
		SELECT * INTO v_route
		FROM api.http_route AS c_route
		WHERE v_version ~ c_route.version_regex_pattern
		AND p_uri ~ c_route.uri_regex_pattern
		AND deleted_utc IS NULL
		ORDER BY "id" DESC
		LIMIT 1;
		IF NOT FOUND THEN
			RETURN ROW(
				400, 
				'Bad request', 
				v_headers, 
				jsonb_build_object('error', 'Invalid route.'))::api.http_response;
		END IF;
	END IF;
	v_request := ROW(p_method, p_uri, p_headers, p_body);
	EXECUTE v_route.command USING v_request INTO v_response;
	v_response.headers := COALESCE(v_response.headers, ''::hstore);
	v_response.headers := v_response.headers||hstore('Content-Type','application/json');
	RETURN v_response;
END;
$body$;


select * from api.http_invoke(
	'get', 
	'/customers?v=c1dba4b4-5f5e-47d0-b30e-eff0c24e04bf');