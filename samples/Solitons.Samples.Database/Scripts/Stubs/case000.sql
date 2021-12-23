INSERT INTO data.organization(object_id, id, email)
VALUES('90b74b98-6ee7-491e-a555-71078545c020','Northwind','john@northwind.com');



SELECT FROM data.user_upsert(
	'90b74b98-6ee7-491e-a555-71078545c020'
	,'katherine@northwind.com',
	'customer')