using System.Collections;
using POMQC.Entities.Account;
using POMQC.Entities.Base;
using POMQC.Utilities;

namespace POMQC.Data.Account
{
    public class LoginRepository : ILoginRepository
    {
        #region ILoginRepository Members

        public LoginEntity Login(string username, string password)
        {
            var result = new LoginEntity();
            using (var sp = new StoredProcedure("sp_User_Login"))
            {
                var param = new Hashtable();
                param.Add("username", username);
                param.Add("password", password);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                result.GroupId = sp.GetParameterValue("groupid").ConvertTo<int>();
                result.UserId = sp.GetParameterValue("userid").ConvertTo<int>();
                result.Username = sp.GetParameterValue("username").ConvertTo<string>();
                result.Email = sp.GetParameterValue("email").ConvertTo<string>();
                result.Epwd = sp.GetParameterValue("epwd").ConvertTo<string>();
            }

            return result;
        }

        public Result UpdateEmail(int userId, string email, string password)
        {
            var result = new Result();
            using (var sp = new StoredProcedure("sp_User_Email"))
            {
                var param = new Hashtable();
                param.Add("userid", userId);
                param.Add("email", email);
                param.Add("password", password);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                result.Message = sp.GetParameterValue("errmsg").ConvertTo<string>();
            }

            return result;
        }

        #endregion
    }
}