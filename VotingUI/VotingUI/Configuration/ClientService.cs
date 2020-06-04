using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VotingUI.Services;

namespace VotingUI.Configuration
{
    public static class ClientService
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<VotingService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("VotingEndPoint:BaseUrl"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }
    }
}
