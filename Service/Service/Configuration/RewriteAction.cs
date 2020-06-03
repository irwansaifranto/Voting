using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Configuration
{
    public static class RewriteAction
    {
        public static void DefaultUrl(this IApplicationBuilder app)
        {
            var option = new RewriteOptions();

            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);
        }
    }
}
