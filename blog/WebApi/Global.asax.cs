using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using Services.Autofac;
using System.Reflection;
using System.Web.Http;
using WebApi.AutoMapper;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);

        }
    }
}
