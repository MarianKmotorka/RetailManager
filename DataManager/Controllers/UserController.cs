using Microsoft.AspNet.Identity;
using RM.WebApi.Library.DataAccess;
using RM.WebApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public UserModel GetUserById()
        {
            var data = new UserData();
            var userId = RequestContext.Principal.Identity.GetUserId();
            var output = data.GetUserById(userId);

            return output;
        }
    }
}