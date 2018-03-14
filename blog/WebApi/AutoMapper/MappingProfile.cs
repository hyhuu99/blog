using AutoMapper;
using Services.AutoMapper;

namespace WebApi.AutoMapper
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServicesMappingProfile>();
            });
            return config;
        }
    }        
}
