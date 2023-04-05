

SELECT (route).*
FROM 
	api.vw_user_http_route
WHERE
	'/customers?v=1.0' ~  (route).uri_regex_pattern
AND 'get'              ~* (route).method_regex_pattern
AND '1.0'              ~* (route).version_regex_pattern;
	
/*	
AND '/customers?v=1.0' ~  c_route.uri_regex_pattern
AND 'get'              ~* c_route.method_regex_pattern
AND '1.0'              ~* c_route.version_regex_pattern
*/