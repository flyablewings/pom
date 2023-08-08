using POMQC.Entities.Account;
using POMQC.Entities.Base;

namespace POMQC.Data.Account
{
    public interface ILoginRepository
    {
        LoginEntity Login(string username, string password);

        Result UpdateEmail(int userId, string email, string password);
    }
}