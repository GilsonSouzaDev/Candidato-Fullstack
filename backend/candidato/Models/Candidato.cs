using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace candidatos.Models;

public class Candidato
{

    public long Id { get;  set; }

    public string? Nome { get;  set; }
    public long FiliacaoId { get; internal set;}
    public virtual Filiacao? Filiacao { get; set; }
    public long EnderecoId { get; internal set; }
    public virtual Endereco? Endereco { get; set; }
    public virtual ICollection<Telefone>? Telefones  { get; set; }
    public virtual ICollection<Curso>? Cursos { get;  set; }
    
    public virtual ICollection<Candidatura>? Candidaturas { get; set; }
}
