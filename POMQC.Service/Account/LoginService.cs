using POMQC.Data.Account;
using POMQC.Entities.Base;
using POMQC.ViewModels.Account;

namespace POMQC.Services.Account
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _login = new LoginRepository();

        #region ILoginService Members

        public LoginViewModel Login(string username, string password)
        {
            var data = _login.Login(username, password);

            return new LoginViewModel { Login = data, ErrorMessage = data.UserId > 0 ? string.Empty : "Wrong username or password" };
        }

        public Result UpdateEmail(int userId, string email, string password)
        {
            return _login.UpdateEmail(userId, email, password);
        }

        #endregion
    }
}