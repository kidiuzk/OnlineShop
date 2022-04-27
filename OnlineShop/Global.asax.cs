using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e) //Request sever trả về cho client
        {
            var AccountCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];  //truy vấn lấy cookie
            if (AccountCookie != null)  //ktra cookie dang nhap r
            {
                var authTicket = FormsAuthentication.Decrypt(AccountCookie.Value);     //decrypt để giải mã tài khoản cookie để lấy quyền
                var roles = authTicket.UserData.Split(new Char[] { ',' });      //lấy từng quyền
                var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);  //gán role vào
                Context.User = userPrincipal;
            }
        }
    }
}
