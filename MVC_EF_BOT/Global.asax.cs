using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC_EF_BOT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Classes.TeleBot telebot;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            new Task(startTeleBot).Start();
        }
        public void startTeleBot()
        {
            telebot = new Classes.TeleBot();
        }
    }
}
