using RM.WPF.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RM.WPF.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private IApiHelper _apiHelper;

        public SaleEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task Post(SaleModel model)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/sale", model))
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
