using BLL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ILogic
{
    public interface IUserInfoLogic
    {
        UserInfoViewModel AddUserInfo(UserInfoModel userInfoModel);
        UserInfoViewModel CheckUserLogin(UserLoginModel userLogin);
        bool ExitLogic();
        bool UpdateUserInfo(UserInfoModel userInfoModel);
        string GetUserAvatarUrl(string telPhone);
        string GetToken();
    }
}
