using RMDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Helpers
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;

        public ApiHelper()
        {
            InitializeClient();
        }

        public async Task<AuthenticatedUser> Authenticate(string userName, string password)
        {

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("username", userName),
                new KeyValuePair<string,string>("password",password)
            });

            using(var response = await _apiClient.PostAsync("/token", data))
            {
                if(response.IsSuccessStatusCode)
                {
                    var authenticatedUser = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return authenticatedUser;
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
