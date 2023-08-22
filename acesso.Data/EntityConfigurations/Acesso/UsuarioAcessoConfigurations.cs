using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using acesso.Domain.Entities.Acesso;

namespace acesso.Data.EntityConfigurations.Acesso
{
    public class UsuarioAcessoConfigurations : IEntityTypeConfiguration<UsuarioAcesso>
    {
        public void Configure(EntityTypeBuilder<UsuarioAcesso> builder)
        {
            builder.ToTable("usuario_acesso", "acesso");

            builder.HasKey(c => c.Id);

            builder.Property(p => p.Id)
                   .HasComment("Identificador da Tabela.");

            builder.Property(p => p.UsuarioId)
                   .IsRequired()
                   .HasComment("Identificador do Usuário.");

            builder.Property(x => x.Ip)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasComment("Ip do Acesso");

            builder.Property(x => x.DataUlitmoAcesso)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .HasComment("Data do Úlitmo Acesso");

            builder.HasOne(c => c.Usuario)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
