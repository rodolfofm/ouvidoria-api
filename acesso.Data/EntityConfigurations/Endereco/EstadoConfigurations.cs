using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using acesso.Domain.Entities.Endereco;

namespace acesso.Data.EntityConfigurations.Endereco
{
    public class EstadoConfigurations : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.ToTable("estado", "endereco");

            builder.HasKey(c => c.Id);

            builder.Property(p => p.Id)
                   .HasComment("Identificador da Tabela.")
                   .ValueGeneratedNever();

            builder.Property(x => x.Nome)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

            builder.Property(x => x.Sigla)
                                .IsRequired()
                                .HasColumnType("varchar(2)");

            builder.Property(x => x.CodigoIbge)
                                .IsRequired()
                                .HasColumnType("int");

        }
    }
}
