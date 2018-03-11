using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YetAnotherBlog.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetName(this System.Security.Principal.IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).Name;

            return (claim != null) ? claim : String.Empty;
        }
    }
}
