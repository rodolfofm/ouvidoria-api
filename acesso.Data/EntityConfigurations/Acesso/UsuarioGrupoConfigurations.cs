using DomainLib.Base.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using acesso.Domain.Entities.Acesso;

namespace acesso.Data.EntityConfigurations.Acesso
{
    public class UsuarioGrupoConfigurations : BaseConfigurations<UsuarioGrupo>
    {
        public override void ConfigureOtherProperties(EntityTypeBuilder<UsuarioGrupo> builder)
        {
            builder.ToTable("usuario_grupo", "acesso");

            builder.Property(x => x.UsuarioId)
                    .IsRequired()
                    .HasComment("Usuario Id de UsuarioGrupo");

            builder.Property(x => x.GrupoId)
                    .IsRequired()
                    .HasComment("Grupo Id de UsuarioGrupo");

            builder.HasOne(c => c.Usuario)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Grupo)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
