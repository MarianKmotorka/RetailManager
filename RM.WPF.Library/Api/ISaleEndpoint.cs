using System.Threading.Tasks;
using RM.WPF.Library.Models;

namespace RM.WPF.Library.Api
{
    public interface ISaleEndpoint
    {
        Task Post(SaleModel model);
    }
}