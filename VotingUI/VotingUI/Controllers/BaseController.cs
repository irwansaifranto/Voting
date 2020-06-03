using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VotingUI.Controllers
{
    public class BaseController : Controller
    {
        const string UserIdSessionKey = "_UserId";
        const string UserLevelSessionKey = "_UserLevel";
        const string UserNameSessionKey = "_UserName";
        protected string UserId
        {
            get { return HttpContext.Session?.GetString(UserIdSessionKey) ?? ""; }
            set { HttpContext.Session.SetString(UserIdSessionKey, value); }
        }
        protected string UserLevel
        {
            get { return HttpContext.Session?.GetString(UserLevelSessionKey) ?? ""; }
            set { HttpContext.Session.SetString(UserLevelSessionKey, value); }
        }
        protected string UserName
        {
            get { return HttpContext.Session?.GetString(UserNameSessionKey) ?? ""; }
            set { HttpContext.Session.SetString(UserNameSessionKey, value); }
        }
    }
}