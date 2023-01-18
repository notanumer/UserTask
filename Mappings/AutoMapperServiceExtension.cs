using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Mappings
{
    public static class AutoMapperServiceExtension
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapConfig = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new UserMappingProfile());
            });

            var mapper = mapConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
