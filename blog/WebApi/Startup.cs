using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Services.Autofac;
using Services.AutoMapper;
using System;
using System.Reflection;
using System.Web.Http;
using WebApi.AutoMapper;
using WebApi.Owin;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var builder = new ContainerBuilder();
            config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiModelBinderProvider();

            
            builder.RegisterModule(new ServiceModule());


            var container = builder.Build();       
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ConfigureOAuth(app);

            app.UseWebApi(config);
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ServicesMappingProfile>();
            });

        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/authtoken"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(90),
                Provider = new SimpleAuthorizationServerProvider(),
                ApplicationCanDisplayErrors = true,
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        string getTime()
        {
            return DateTime.Now.Millisecond.ToString();
        }
    }
}
