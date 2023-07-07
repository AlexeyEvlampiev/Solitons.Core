INSERT INTO data.account(object_id, balance, "id", "description")
VALUES('5840573e-9786-4cae-bd2e-201976dc1555', 99999999.99, 'SkyNet', 'SkyNet is a cutting-edge technology firm specializing in advanced artificial intelligence and drone technologies, aiming to revolutionize global logistics by creating a network of autonomous delivery systems for instant transportation of goods across the globe. Through harnessing the vast potential of the skies, SkyNet is pushing boundaries to shape a future where the sky''s no longer the limit but the new superhighway.')
ON CONFLICT(object_id) DO UPDATE SET
	"id" = EXCLUDED.id,
	balance = 99999999.99,
	"description" = EXCLUDED.description;


