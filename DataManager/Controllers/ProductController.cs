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
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        public List<ProductModel> GetAllProducts()
        {
            var data = new ProductData();
            var products = data.GetProducts();
            return products;
        }
    }
}
