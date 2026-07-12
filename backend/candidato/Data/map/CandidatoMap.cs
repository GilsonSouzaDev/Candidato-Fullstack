using candidatos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace candidato.Data.map
{
    public class CandidatoMap : IEntityTypeConfiguration<Candidato>
    {
        public void Configure(EntityTypeBuilder<Candidato> builder)
        {
                    builder.Property(x => x.Id)
                           .HasColumnName("CandidatoId") // Exemplo de nome de coluna personalizado
                           .IsRequired();


                    builder.Property(x => x.Nome)
                           .IsRequired()
                           .HasColumnType("VARCHAR")
                           .HasMaxLength(200);


                    builder.HasOne(x => x.Filiacao)
                           .WithOne()
                           .OnDelete(DeleteBehavior.Cascade)
                           .HasForeignKey<Candidato>(c => c.FiliacaoId);

                    builder.HasOne(x => x.Endereco)
                           .WithOne()
                           .OnDelete(DeleteBehavior.Cascade)
                           .HasForeignKey<Candidato>(c => c.EnderecoId);


                    builder.HasMany(x => x.Telefones)
                           .WithOne()                          
                           .OnDelete(DeleteBehavior.Cascade);

                    builder.HasMany(x => x.Cursos)      
                           .WithOne(c => c.Candidato)
                           .HasForeignKey(c => c.CandidatoId)
                           .OnDelete(DeleteBehavior.Cascade);

                    builder.HasMany(x => x.Formacoes)
                           .WithOne(f => f.Candidato)
                           .HasForeignKey(f => f.CandidatoId)
                           .OnDelete(DeleteBehavior.Cascade);
                    builder.HasMany(x => x.Habilidades)
                           .WithOne(h => h.Candidato)
                           .HasForeignKey(h => h.CandidatoId)
                           .OnDelete(DeleteBehavior.Cascade);

                    builder.HasMany(x => x.Experiencias)
                           .WithOne(e => e.Candidato)
                           .HasForeignKey(e => e.CandidatoId)
                           .OnDelete(DeleteBehavior.Cascade);

                    builder.HasOne(x => x.Usuario)
                           .WithOne(u => u.CandidatoPerfil)
                           .HasForeignKey<Candidato>(c => c.UsuarioId)
                           .OnDelete(DeleteBehavior.Restrict);

        }
    }
}









