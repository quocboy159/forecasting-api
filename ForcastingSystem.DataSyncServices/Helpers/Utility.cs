using System.Globalization;

namespace ForecastingSystem.DataSyncServices.Helpers
{
    public static class Utility
    {
        public static DateTime? BambooHRDateFromString(string dateStr)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            string bambooHRFormat = "yyyy-MM-dd";

            DateTime result;
            if (DateTime.TryParseExact(dateStr, bambooHRFormat, provider, DateTimeStyles.None, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        internal static int? ToInt(string value)
        {
            return int.Parse(value);
        }

        public static string GetUsernameFromEmail(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) return "";

            int index = userEmail.IndexOf("@");
            if (index > 0)
            {
                return userEmail.Substring(0, index);
            }

            return "";
        }
    }
}
