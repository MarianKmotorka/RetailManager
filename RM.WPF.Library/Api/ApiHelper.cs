using RM.WPF.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RM.WPF.Library.Api
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;
        private ILoggedInUserModel _loggedInUserModel;

        public ApiHelper(ILoggedInUserModel loggedInUserModel)
        {
            InitializeClient();
            _loggedInUserModel = loggedInUserModel;
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

        public async Task GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (var response = await _apiClient.GetAsync("/api/user"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUserModel.Id = user.Id;
                    _loggedInUserModel.FirstName = user.FirstName;
                    _loggedInUserModel.LastName = user.LastName;
                    _loggedInUserModel.CreatedDate = user.CreatedDate;
                    _loggedInUserModel.EmailAddress = user.EmailAddress;
                    _loggedInUserModel.Token = token;
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
