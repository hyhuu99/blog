using Autofac;
using blog.ApiResponse.Interface;

namespace blog.Autofac
{
    public class BlogModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<blog.ApiResponse.ApiResponse>().As<IApiResponse>();          
            base.Load(builder);
        }
    }
}
