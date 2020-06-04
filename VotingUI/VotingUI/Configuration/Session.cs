using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingUI.Configuration
{
    public static class Session
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Voting.Session";
                options.IdleTimeout = TimeSpan.FromDays(90);
                //options.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential = true;
            });
        }
    }
}
