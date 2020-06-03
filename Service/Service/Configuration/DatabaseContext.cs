using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Service.Configuration
{
    public static class DatabaseContext
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VotingContext>(options => options.UseSqlServer(configuration.GetValue<string>("Settings:ConnectionString")));
        }
    }
}
