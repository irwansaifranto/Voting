using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VotingUI.Helper;
using VotingUI.Models.Response;

namespace VotingUI.Services
{
    public class VotingService
    {
        HttpClient Client { get; }
        public VotingService(HttpClient client)
        {
            Client = client;
        }

        public async Task<BaseResponse> GetAsync(string uri)
        {
            var result = new BaseResponse();

            try
            {
                var response = await Client.GetAsync(uri);

                response.EnsureSuccessStatusCode();

                result = JsonConvert.DeserializeObject<BaseResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }

            return result;
        }

        public async Task<BaseResponse> PostAsync<T>(string uri, T model) where T : class
        {
            var result = new BaseResponse();

            try
            {
                var response = await Client.PostAsync(uri, new JsonContent(model));

                response.EnsureSuccessStatusCode();

                result = JsonConvert.DeserializeObject<BaseResponse>(await response.Content.ReadAsStringAsync());
            }

            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }

            return result;
        }
    }
}
