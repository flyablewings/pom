using POMQC.Entities.Base;
using POMQC.ViewModels.Account;

namespace POMQC.Services.Account
{
    public interface ILoginService
    {
        LoginViewModel Login(string username, string password);

        Result UpdateEmail(int userId, string email, string password);
    }
}