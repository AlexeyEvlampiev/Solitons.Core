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
	v_jwt_subject text := COALESCE(TRIM(p_headers->'JWT-Sub'));
	v_request  api.http_request;
	v_response api.http_response;
	v_route    api.http_route;
BEGIN	
	
	IF v_jwt_subject IS NULL THEN
		RETURN ROW(
			401, 
			'Unauthorized', 
			v_headers, 
			jsonb_build_object('error', 'Authentication failed. Please provide a valid JWT-Sub header.'))::api.http_response;	
	END IF;
	

	PERFORM set_config('api.jwt_subject', v_jwt_subject, true);
	--RAISE NOTICE 'Caller: %', current_setting('api.jwt_subject');
	
	-- TODO: ADD "missing version" handler
	
	IF v_route_object_id IS NOT NULL THEN	
		SELECT (route).* INTO v_route
		FROM api.vw_user_http_route
		WHERE 
			(user).external_id = v_jwt_subject
		AND (route).object_id = v_route_object_id
		AND p_uri     ~ (route).uri_regex_pattern
		AND p_method  ~* (route).method_regex_pattern
		LIMIT 1;
	ELSE
		
		SELECT (route).* INTO v_route
		FROM api.vw_user_http_route
		WHERE 
			p_uri     ~ (route).uri_regex_pattern
		AND p_method  ~* (route).method_regex_pattern 
		AND v_version ~* (route).version_regex_pattern		
		ORDER BY (route).id DESC
		LIMIT 1;
	END IF;
	
	IF v_route IS NULL THEN
		IF NOT EXISTS (SELECT 1 FROM api.http_route WHERE p_uri ~ uri_regex_pattern) THEN
		RETURN ROW(
			404, 
			'Not found', 
			v_headers, 
			jsonb_build_object('error', 'The specified resource was not found.'))::api.http_response;			
		END IF;
		IF NOT EXISTS (
			SELECT 1 FROM api.http_route 
			WHERE p_uri      ~ uri_regex_pattern
			AND   p_method  ~* method_regex_pattern) THEN
		RETURN ROW(
			405, 
			'Method Not Allowed', 
			v_headers, 
			jsonb_build_object('error', 'The specified HTTP method is not supported by this endpoint.'))::api.http_response;			
		END IF;		
		RETURN ROW(
			404, 
			'Bad Request', 
			v_headers, 
			jsonb_build_object('error', 'The requested version of the API is not available.'))::api.http_response;			
	END IF;
	
	v_request := ROW(p_method, p_uri, p_headers, p_body);
	EXECUTE v_route.command USING v_request INTO v_response;
	v_response.headers := COALESCE(v_response.headers, ''::hstore);
	v_response.headers := v_response.headers||hstore('Content-Type','application/json');
	RETURN v_response;
END;
$body$;


