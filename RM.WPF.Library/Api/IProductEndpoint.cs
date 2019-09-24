using System.Collections.Generic;
using System.Threading.Tasks;
using RM.WPF.Library.Models;

namespace RM.WPF.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}