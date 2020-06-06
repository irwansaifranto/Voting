using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Repository.Abstract;
using Service.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Configuration
{
    public static class DependencyInjection
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IVotingRepository, VotingRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
    }
}
