using Autofac;
using Autofac.Integration.Mvc;
using blog.Autofac;
using blog.AutoMapper;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace blog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            
            var mapper_web = MappingProfile.InitializeAutoMapper().CreateMapper();
            builder.RegisterInstance(mapper_web);
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();
            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterModule(new BlogModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
        }
    }
}
