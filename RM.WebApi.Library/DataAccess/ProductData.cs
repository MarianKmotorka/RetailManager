using RM.WebApi.Library.Internal.DataAccess;
using RM.WebApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.WebApi.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            var data = new SqlDataAccess();
            var products = data.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "RM.Database");
            return products;
        }

        public ProductModel GetById(int productId)
        {
            var data = new SqlDataAccess();
            var product = data.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "RM.Database").SingleOrDefault();
            return product;
        }
    }
}
