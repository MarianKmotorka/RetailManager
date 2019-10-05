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
        public HttpClient ApiClient { get; private set; }

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

            using(var response = await ApiClient.PostAsync("/token", data))
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
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (var response = await ApiClient.GetAsync("/api/user"))
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
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(api);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void LogOffUser()
        {
            ApiClient.DefaultRequestHeaders.Clear();
        }
    }
}
