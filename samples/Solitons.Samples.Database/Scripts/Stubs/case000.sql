INSERT INTO data.organization(object_id, id, email)
VALUES('90b74b98-6ee7-491e-a555-71078545c020','Northwind','john@northwind.com')
ON CONFLICT(object_id) DO UPDATE SET 
	id = EXCLUDED.id, email = EXCLUDED.email;



SELECT FROM data.user_upsert(
	'90b74b98-6ee7-491e-a555-71078545c020'
	,'katherine@northwind.com',
	'customer')