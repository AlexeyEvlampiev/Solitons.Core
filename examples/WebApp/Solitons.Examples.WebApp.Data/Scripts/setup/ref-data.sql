INSERT INTO api.user(external_id) VALUES('aevlampiev') ON CONFLICT(external_id) DO NOTHING;
INSERT INTO api.role(id) VALUES('web-client'), ('user') ON CONFLICT(id) DO NOTHING;

INSERT INTO api.user_role(user_object_id, role_object_id)
SELECT c_user.object_id, c_role.object_id
FROM api.user AS c_user, api.role AS c_role
WHERE c_user.external_id = 'aevlampiev'
AND c_role.id = 'user'
ON CONFLICT(user_object_id, role_object_id) DO NOTHING;


INSERT INTO api.http_route_role(http_route_object_id, role_object_id)
SELECT c_route.object_id, c_role.object_id
FROM api.http_route AS c_route, api.role AS c_role
WHERE c_role.id = 'user'
ON CONFLICT(http_route_object_id, role_object_id) DO NOTHING;