CREATE OR REPLACE VIEW api.vw_user_http_route AS
SELECT 
	c_user AS "user", c_http_route AS "route", c_role AS "role"
FROM 
	api.user AS c_user
INNER JOIN
	api.user_role AS c_ur
		ON c_ur.user_object_id = c_user.object_id
INNER JOIN
	api.http_route_role AS c_rr
		ON c_rr.role_object_id = c_ur.role_object_id
INNER JOIN
	api.http_route AS c_http_route
		ON  c_http_route.object_id = c_rr.http_route_object_id
		AND c_http_route.deleted_utc IS NULL
INNER JOIN
	api.role AS c_role
		ON c_role.object_id = c_ur.role_object_id;