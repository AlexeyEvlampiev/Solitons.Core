

CREATE OR REPLACE FUNCTION api.http_invoke(
	"method" text,
	address text, 
	headers hstore,
	content jsonb DEFAULT('{}'::JSONB)) RETURNS api.http_response
AS
$body$
DECLARE
	v_client_external_id text := NULLIF(TRIM(headers->'SKYNET-IDENTITY'), '');
	v_client_version text := substring(address from '(?i)(?:(?:api-?)?version|v)=(\S+)');
	v_identity data.identity;
	v_account data.account;	
	v_route api.http_route;
	v_response api.http_response;
	v_payment NUMERIC(10, 2);
	v_balance NUMERIC(10, 2);	
	v_std_headers hstore := 'Current-Version=>1.0, Content-Type=>application/json';
	v_sql text;
BEGIN

	IF v_client_external_id IS NULL THEN
		RETURN api.http_response_build(401, v_std_headers, 
		'Unauthorized. Valid credentials are required to access this resource.');
	END IF;
	
	IF v_client_version IS NULL THEN
		RETURN api.http_response_build(400, v_std_headers, 
		'Bad Request. The client version is missing from the request.');
	END IF;
	
	SELECT c_identity.* INTO v_identity
	FROM data.identity AS c_identity
	INNER JOIN 
		data.account AS c_account 
			ON c_account.object_id = c_identity.account_object_id
			AND c_account.deleted_utc IS NULL
	WHERE 
		c_identity.deleted_utc IS NULL		
	AND LOWER(v_client_external_id) = LOWER(c_identity.id);

	IF NOT FOUND THEN
		RETURN api.http_response_build(401, v_std_headers, 
		'Unauthorized. Authentication credentials are incorrect.');
	END IF;


	SELECT c_account.* INTO v_account
	FROM data.account AS c_account
	WHERE c_account.object_id = v_identity.account_object_id
	AND c_account.balance > 0.0;

	IF NOT FOUND THEN
		RETURN api.http_response_build(402, v_std_headers, 
		'Payment Required. Your account has insufficient balance to process the request.');
	END IF;


	--RAISE NOTICE 'Searching%', '';
	SELECT c_route.* INTO v_route
	FROM api.http_route AS c_route
	WHERE 
		c_route.deleted_utc IS NULL
	AND $2 ~* c_route.address_regexp
	ORDER BY 
		c_route.sequence_number DESC,
		(CASE $1 ~* v_route.method_regexp WHEN true THEN 0 ELSE 1 END),
		(CASE v_client_version ~* v_route.version_regexp WHEN true THEN 0 ELSE 1 END)
	LIMIT 1;



	IF NOT FOUND THEN
		RETURN api.http_response_build(404, v_std_headers, 
		'Not Found. The requested resource could not be located.');
	END IF;
	
	IF NOT $1 ~* v_route.method_regexp THEN
		RETURN api.http_response_build(405, v_std_headers, 
		FORMAT('Method Not Allowed. The %s method is not allowed for this endpoint.', UPPER(quote_literal($1))));
	END IF;
	
	IF NOT v_client_version ~* v_route.version_regexp THEN
		RETURN api.http_response_build(400, v_std_headers, 
		FORMAT('Bad Request. The %s version is not supported for this endpoint.', quote_literal(v_client_version)));
	END IF;

	IF v_route.price > v_account.balance THEN
		RETURN api.http_response_build(402, v_std_headers, 
		'Payment Required. Your account balance is insufficient to cover the cost of this request.');
	END IF;

	v_std_headers := v_std_headers||
		hstore('SKYNET-ROUTE=>'||v_route.object_id::text);
	
	SELECT FORMAT($$ SELECT * FROM api.%s(api.http_request_build($1, $2, $3, $4)); $$, v_route.handler) 
	INTO v_sql;

	--RAISE NOTICE 'SQL: %', v_sql;

	EXECUTE v_sql 
	INTO v_response 
	USING $1, $2, $3, $4;

	v_payment := (CASE WHEN v_response.status_code < 400 THEN v_route.price ELSE 0.01 END);

	UPDATE data.account
	SET balance = balance - v_payment
	WHERE object_id = (
		SELECT c_account.object_id
		FROM data.account AS c_account 
		WHERE c_account.object_id = v_account.object_id
	)
	RETURNING balance INTO v_balance;


	v_response.headers = v_response.headers||
		hstore(FORMAT('Payment=>%s,Balance=>%s', v_payment, v_balance))||
		v_std_headers;

	RETURN v_response;

END;
$body$ LANGUAGE 'plpgsql';