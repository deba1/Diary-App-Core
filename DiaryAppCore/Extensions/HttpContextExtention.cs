using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class HttpContextExtention
    {
        public static string GetUserId(this HttpContext context)
        {
            return context.User.Identity.Name;
        }

        public static bool HasUserRoles(this HttpContext context, string role)
        {
            return context.User.IsInRole(role);
        }
    }
}
