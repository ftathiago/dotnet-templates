using System;

namespace WebApi.Shared.Extensions
{
    public static class EnumExtension
    {
        public static int AsInteger(this Enum value) =>
            Convert.ToInt32(value);

        public static string AsString(this Enum value) =>
            Enum.GetName(value.GetType(), value);
    }
}
