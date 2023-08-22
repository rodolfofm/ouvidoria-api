using acesso.Domain.Entities.Acesso;
using DomainLib.Base.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace acesso.Data.EntityConfigurations.Acesso
{
    public class GrupoConfigurations : BaseConfigurations<Grupo>
    {
        public override void ConfigureOtherProperties(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("grupo", "acesso");

            builder.Property(x => x.Nome)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasComment("Nome do Grupo");
        }
    }
}
