using DomainLib.Base.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using acesso.Domain.Entities.Acesso;

namespace acesso.Data.EntityConfigurations.Acesso
{
    public class UsuarioConfigurations : BaseConfigurations<Usuario>
    {
        public override void ConfigureOtherProperties(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario", "acesso");

            builder.Property(x => x.Nome)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasComment("Nome do Usuario");

            builder.Property(x => x.Cpf)
                    .IsRequired()
                    .HasColumnType("varchar(11)")
                    .HasComment("Cpf do Usuario");

            builder.Property(x => x.Email)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasComment("Email do Usuario");

            builder.Property(x => x.EmailValidado)
                    .IsRequired()
                    .HasColumnType("bool")
                    .HasDefaultValueSql("false");

            builder.Property(x => x.Administrador)
                    .IsRequired()
                    .HasColumnType("bool")
                    .HasDefaultValueSql("false");

            builder.Property(x => x.Telefone)
                    .HasColumnType("varchar(20)")
                    .HasComment("Telefone do Contato");

            builder.Property(x => x.Login)
                    .HasColumnType("varchar(50)")
                    .IsRequired()
                    .HasComment("Login do Usuario");

            builder.Property(x => x.Senha)
                    .HasColumnType("varchar(255)")
                    .IsRequired()
                    .HasComment("Senha do Usuario");

            builder.Property(x => x.Cep)
                   .IsRequired()
                   .HasColumnType("varchar(20)")
                   .HasComment("Cep da Usuario");

            builder.Property(x => x.Logradouro)
                    .HasColumnType("varchar(200)")
                    .HasComment("Logradouro da Usuario");

            builder.Property(x => x.Complemento)
                    .HasColumnType("varchar(200)")
                    .HasComment("Complemento do Endereço da Usuario");

            builder.Property(x => x.Numero)
                    .HasColumnType("varchar(20)")
                    .HasComment("Número do Endereço da Usuario");

            builder.Property(x => x.CidadeId)
                    .HasColumnType("int4")
                    .HasComment("Cidade Id da Usuario");

        }
    }
}
