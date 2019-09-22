using System.Threading.Tasks;
using RM.WPF.Library.Models;

namespace RM.WPF.Library.Api
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string userName, string password);
        Task GetLoggedInUserInfo(string token);
    }
}