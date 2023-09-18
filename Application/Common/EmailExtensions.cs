using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Common
{
    public static class EmailExtensions
    {
        public static string GetUsernameFromEmail(this string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            int index = email.IndexOf("@");
            if (index > 0)
            {
                return email.Substring(0, index);
            }

            return "";
        }
    }
}
