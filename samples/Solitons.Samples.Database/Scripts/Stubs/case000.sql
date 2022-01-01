INSERT INTO data.organization(object_id, id, email)
VALUES('90b74b98-6ee7-491e-a555-71078545c020','Northwind','john@northwind.com')
ON CONFLICT(object_id) DO UPDATE SET 
	id = EXCLUDED.id, email = EXCLUDED.email;



SELECT FROM data.user_upsert(
	'90b74b98-6ee7-491e-a555-71078545c020'
	,'katherine@northwind.com',
	'customer');

SELECT * FROM api.http_service_upsert(
	'e725b5e0-de00-4b1b-8d61-72f244497979', 
	'Public API', 
	'Public API', 
	'1.0',
	'prospect, customer',
	'https://localhost:1234');



