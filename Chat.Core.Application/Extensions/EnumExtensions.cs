using System.ComponentModel;

namespace Chat.Core.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var fi = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            return (descriptionAttributes != null && descriptionAttributes.Any())
                ? descriptionAttributes.First().Description
                : value.ToString();
        }
    }
}
