using System;
using MoneyBox.Web.Core;

namespace MoneyBox.Web
{
    public class Global : System.Web.HttpApplication
    {


        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }
    }
}