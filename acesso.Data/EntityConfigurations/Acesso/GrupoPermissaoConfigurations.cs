using acesso.Domain.Entities.Acesso;
using DomainLib.Base.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace acesso.Data.EntityConfigurations.Acesso
{
    public class GrupoPermissaoConfigurations : BaseConfigurations<GrupoPermissao>
    {
        public override void ConfigureOtherProperties(EntityTypeBuilder<GrupoPermissao> builder)
        {
            builder.ToTable("grupo_permissao", "acesso");

            builder.Property(x => x.GrupoId)
                    .IsRequired()
                    .HasComment("Grupo Id");

            builder.Property(x => x.PermissaoId)
                    .IsRequired()
                    .HasComment("Permissao Id");

            builder.HasOne(c => c.Permissao)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Grupo)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
