using System;

namespace Solitons
{
    public static partial class Extensions
    {
        public static bool IsNumeric(this Type self)
        {
            if (self is null) return false;
            self = Nullable.GetUnderlyingType(self) ?? self;
            var code = Type.GetTypeCode(self);

            if (code == TypeCode.Decimal) 
                return true;

            if(self.IsPrimitive && code != TypeCode.Object && code != TypeCode.Boolean && code != TypeCode.Char)
                return true;

            return false;
        }
    }
}
