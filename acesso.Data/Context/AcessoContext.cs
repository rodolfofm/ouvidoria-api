using Microsoft.EntityFrameworkCore;
using RepositoryLib;
using acesso.Data.EntityConfigurations.Endereco;
using acesso.Data.EntityConfigurations.Acesso;
using acesso.Domain.Entities.Acesso;
using acesso.Domain.Entities.Endereco;

namespace acesso.Data.Context
{
    public partial class AcessoContext : DbContext
    {
        public AcessoContext()
        {
        }

        public AcessoContext(DbContextOptions<AcessoContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
        public virtual DbSet<Grupo> Grupo { get; set; } = null!;
        public virtual DbSet<GrupoPermissao> GrupoPermissao { get; set; } = null!;
        public virtual DbSet<Permissao> Permissao { get; set; } = null!;
        public virtual DbSet<UsuarioGrupo> UsuarioGrupo { get; set; } = null!;
        public virtual DbSet<Usuario> Usuario { get; set; } = null!;
        public virtual DbSet<UsuarioAcesso> UsuarioAcesso { get; set; } = null!;
        public virtual DbSet<Cidade> Cidade { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("pt_BR.utf8")
                //.HasPostgresExtension("metaphoneptbr")
                .HasPostgresExtension("unaccent");
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.ToSnakeNames();
            modelBuilder.UseDefaultColumnCollation("pt_BR.utf8");
            modelBuilder.UseIdentityAlwaysColumns();

            modelBuilder
                .ApplyConfiguration(new EstadoConfigurations())
                .ApplyConfiguration(new CidadeConfigurations())
                .ApplyConfiguration(new GrupoConfigurations())
                .ApplyConfiguration(new GrupoPermissaoConfigurations())
                .ApplyConfiguration(new PermissaoConfigurations())
                .ApplyConfiguration(new UsuarioAcessoConfigurations())
                .ApplyConfiguration(new UsuarioConfigurations())
                .ApplyConfiguration(new UsuarioGrupoConfigurations());


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
