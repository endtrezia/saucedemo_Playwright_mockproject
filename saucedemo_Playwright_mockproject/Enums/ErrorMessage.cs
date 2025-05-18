using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Enums
{
    public static class ErrorMessage
    {
        public static readonly string LockedOutUser = "Sorry, this user has been locked out.";
        public static readonly string InvalidUser = "Username and password do not match any user in this service";

        public static string Get(string errorType)
        {
            return errorType switch
            {
                nameof(LockedOutUser) => LockedOutUser,
                nameof(InvalidUser) => InvalidUser,
                _ => "Unknown error type."
            };
        }
    }
}
