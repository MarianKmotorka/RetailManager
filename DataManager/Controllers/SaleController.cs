using Microsoft.AspNet.Identity;
using RM.WebApi.Library.DataAccess;
using RM.WebApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/sale")]
    public class SaleController : ApiController
    {
        [HttpPost]
        public void Post(SaleModel model)
        {
            var saleData = new SaleData();
            var loggedInUserId = RequestContext.Principal.Identity.GetUserId();
            saleData.SaveSale(model, loggedInUserId);
        }
    }
}
