using candidatos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace candidato.Data.map
{
    public class EnderecoMap : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {

            builder.Property(x => x.Id)
                   .HasColumnName("EnderecoId") // Exemplo de nome de coluna personalizado
                   .IsRequired();


            builder.Property(x => x.Logradouro)
                   .IsRequired()
                   .HasColumnType("VARCHAR")
                   .HasMaxLength(200);

            builder.Property(x => x.Cep)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(x => x.Numero)
                   .HasColumnType("VARCHAR")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasOne(x => x.Cidade)
                   .WithOne()
                   .HasForeignKey<Endereco>(c => c.CidadeId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}


