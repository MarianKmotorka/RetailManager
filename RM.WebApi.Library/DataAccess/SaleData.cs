using RM.WebApi.Library.Internal.DataAccess;
using RM.WebApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.WebApi.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            var details = new List<SaleDetailDbModel>();
            var products = new ProductData();
            var taxRate = ConfigHelper.TaxRate;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDbModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = products.GetById(detail.ProductId);

                if (productInfo == null)
                    throw new Exception($"Product with id of {detail.ProductId} was not found in DB !");

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;
                detail.Tax = productInfo.IsTaxable ? detail.PurchasePrice * taxRate : 0;

                details.Add(detail);
            }

            var sale = new SaleDbModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            var sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "RM.Database");

            sale.Id = sql.LoadData<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }, "RM.Database").Single();

            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                sql.SaveData("dbo.spSaleDetail_Insert", item, "RM.Database");
            }
        }
    }
}
