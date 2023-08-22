using acesso.Domain.Entities.Endereco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace acesso.Data.EntityConfigurations.Endereco
{
    public class CidadeConfigurations : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ToTable("cidade", "endereco");

            builder.HasKey(c => c.Id);

            builder.Property(p => p.Id)
                   .HasComment("Identificador da Tabela.")
                   .ValueGeneratedNever();

            builder.Property(x => x.Nome)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

            builder.Property(x => x.CodigoIbge)
                                .IsRequired()
                                .HasColumnType("int");

            builder.Property(x => x.Cep)
                                .HasColumnType("varchar(10)");

            builder.Property(x => x.EstadoId)
                                .IsRequired()
                                .HasColumnType("int");

            builder.HasOne(c => c.Estado)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
