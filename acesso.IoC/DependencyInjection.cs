using UtilsLib.EmailService;
using UtilsLib.EmailService.Configuration;
using UtilsLib.EmailService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLib.DependencyInjection;
using acesso.Service.Autenticacao;
using acesso.Service.Acesso;
using acesso.Data.Context;
using acesso.Domain.Interfaces.Acesso;
using acesso.Domain.Interfaces.Endereco;
using acesso.Service.Endereco;
using acesso.Domain.ViewModels;
using AutenticacaoLib.Core;

namespace acesso.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AcessoContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DBACESSO"))
#if DEBUG
                //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
#endif  
            );

            services.AddUnitOfWork<AcessoContext>();

            services.AddTransient<ISegurancaService, SegurancaService>();
            services.AddTransient<ICidadeService, CidadeService>();
            services.AddTransient<IUsuarioAcessoService, UsuarioAcessoService>();
            services.AddTransient<IGrupoService, GrupoService>();
            services.AddTransient<IGrupoPermissaoService, GrupoPermissaoService>();
            services.AddTransient<IPermissaoService, PermissaoService>();
            services.AddTransient<IUsuarioGrupoService, UsuarioGrupoService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            
            services.AddSingleton(configuration.GetSection("ParametrosAplicacao").Get<ParametrosAplicacao>());
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            if (emailConfig != null)
            {
                services.AddSingleton(emailConfig);
                services.AddScoped<IEmailSender, EmailSender>();
            }

            return services;
        }
    }

}
