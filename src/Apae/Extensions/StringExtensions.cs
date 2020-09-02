namespace Apae.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            char[] result = value.ToCharArray();
            result[0] = char.ToUpper(result[0]);
            return new string(result);
        }

        public static string Fallback(this string value, string fallback)
        {
            return !string.IsNullOrWhiteSpace(value) ? value : fallback;
        }
    }
}
