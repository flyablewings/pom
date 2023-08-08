using POMQC.Entities.Account;

namespace POMQC.ViewModels.Account
{
    public class LoginViewModel
    {
        public LoginEntity Login { get; set; }

        public string ErrorMessage { get; set; }
    }
}