using DomainLib.Base.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using acesso.Domain.Entities.Acesso;

namespace acesso.Data.EntityConfigurations.Acesso
{
    public class PermissaoConfigurations : BaseConfigurations<Permissao>
    {
        public override void ConfigureOtherProperties(EntityTypeBuilder<Permissao> builder)
        {
            builder.ToTable("permissao", "acesso");

            builder.Property(x => x.Nome)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasComment("Nome Permissao");

            builder.Property(x => x.Role)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasComment("Nome Role");

        }

    }
}
