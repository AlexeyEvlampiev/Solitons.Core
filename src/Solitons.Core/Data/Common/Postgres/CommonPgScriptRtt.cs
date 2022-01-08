﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Solitons.Data.Common.Postgres
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class CommonPgScriptRtt : Solitons.Text.Sql.PgRuntimeTextTemplate
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\r\nCREATE DOMAIN ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(".natural_key AS varchar(150) CHECK(VALUE ~ \'^\\S.*\\S$\');\r\nCREATE DOMAIN ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(".version AS varchar(25) CHECK (value ~ \'^\\d+(\\.\\d+){0,3}$\');\r\nCREATE DOMAIN ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(".email AS varchar(150) CHECK ( value ~ \'");
            this.Write(this.ToStringHelper.ToStringWithCulture(EmailPattern));
            this.Write("\');\r\n\r\nCREATE OR REPLACE FUNCTION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(@".raise_exception_if_null_argument(arg anyelement, arg_name varchar(50)) RETURNS void AS
$$
BEGIN
	IF arg IS NULL THEN
		RAISE EXCEPTION '''%'' argument is required', COALESCE(arg_name, '?');
	END IF;
END;
$$ LANGUAGE 'plpgsql' IMMUTABLE;



CREATE OR REPLACE FUNCTION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(@".raise_exception_if_null_or_empty_argument(arg uuid, arg_name varchar(50)) RETURNS void AS
$$
BEGIN
	IF NULLIF(arg, '00000000-0000-0000-0000-000000000000'::uuid) IS NULL THEN
		RAISE EXCEPTION '''%'' argument is required', COALESCE(arg_name, '?');
	END IF;
END;
$$ LANGUAGE 'plpgsql' IMMUTABLE;

CREATE OR REPLACE FUNCTION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(@".raise_exception_if_empty_argument(arg uuid, arg_name varchar(50)) RETURNS void AS
$$
BEGIN
	IF arg = '00000000-0000-0000-0000-000000000000'::uuid THEN
		RAISE EXCEPTION '''%'' argument is empty.', COALESCE(arg_name, '?');
	END IF;
END;
$$ LANGUAGE 'plpgsql' IMMUTABLE;



CREATE OR REPLACE FUNCTION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(@".raise_exception_if_null_or_empty_argument(arg text, arg_name varchar(50)) RETURNS void AS
$$
BEGIN
	IF NULLIF(TRIM(arg), '') IS NULL THEN
		RAISE EXCEPTION '''%'' argument is required', COALESCE(arg_name, '?');
	END IF;
END;
$$ LANGUAGE 'plpgsql' IMMUTABLE;



CREATE OR REPLACE FUNCTION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(@".try_cast(_in text, INOUT _out ANYELEMENT) AS
$$
BEGIN
   EXECUTE FORMAT('SELECT %L::%s', $1, pg_typeof(_out)) INTO  _out;
EXCEPTION WHEN others THEN
   -- do nothing: _out already carries default
END;
$$ LANGUAGE 'plpgsql' IMMUTABLE;



CREATE OR REPLACE FUNCTION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(".nullif_empty(_arg uuid) RETURNS uuid AS\r\n$$\r\n\tSELECT NULLIF(_arg, \'00000000-0000" +
                    "-0000-0000-000000000000\'::uuid);\r\n$$ LANGUAGE \'sql\' IMMUTABLE;\r\n\r\n\r\nCREATE OR RE" +
                    "PLACE FUNCTION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(".is_empty(_arg uuid) RETURNS bool AS\r\n$$\r\n\tSELECT (_arg = \'00000000-0000-0000-000" +
                    "0-000000000000\'::uuid);\r\n$$ LANGUAGE \'sql\' IMMUTABLE;\r\n\r\n\r\nCREATE MATERIALIZED V" +
                    "IEW ");
            this.Write(this.ToStringHelper.ToStringWithCulture(SchemaName));
            this.Write(@".mvw_role AS
SELECT 
	rolname AS full_name
	,REGEXP_REPLACE(rolname, '^[^_]+[_]', '') AS name
	,rolcanlogin AS is_login_role
	,(NOT rolcanlogin) AS is_group_role
FROM pg_catalog.pg_roles AS pgr
WHERE pgr.rolname LIKE current_database()||'\_%'
ORDER BY rolname;

CREATE UNIQUE INDEX ux_mvw_role ON system.mvw_role (name);
CREATE UNIQUE INDEX ux_mvw_full_name ON system.mvw_role (full_name);


CREATE MATERIALIZED VIEW system.mvw_function AS
SELECT 
	p.proname AS name, 
	n.nspname AS ""schema"", 
	FORMAT('%s.%s',n.nspname, p.proname) AS fullname, 
	t.typname AS return_type,
    d.description,
	proargnames AS arg_names,
	pg_get_function_arguments(p.oid) AS args_csv,
	COALESCE(array_length(proargnames, 1), 0) AS args_count
FROM 
	pg_catalog.pg_proc p
INNER JOIN pg_catalog.pg_namespace n 
	ON n.oid = p.pronamespace
INNER JOIN pg_type t on p.prorettype = t.oid
LEFT JOIN pg_description d on p.oid = d.objoid	
WHERE 
	pg_catalog.pg_function_is_visible(p.oid)
AND n.nspname <> 'pg_catalog'
AND n.nspname <> 'information_schema';

CREATE UNIQUE INDEX ux_mvw_function_pkey ON system.mvw_function(""name"", ""schema"", args_csv);
CREATE INDEX ix_mvw_function_schema ON system.mvw_function(""schema"", ""name"");");
            return this.GenerationEnvironment.ToString();
        }
    }
}
