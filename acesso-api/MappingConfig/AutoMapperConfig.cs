using acesso.Application.Mappings;
using AutoMapper;

namespace acesso_api.MappingConfig
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingDtoAndModelAcesso());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
