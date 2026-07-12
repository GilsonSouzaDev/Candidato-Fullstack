using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace candidatos.Models;

public class Curso
{
    public long Id { get; set; }
    
    [Required]
    public string Nome { get; set; } = string.Empty;
    
    public string? Instituicao { get; set; }
    public int? AnoConclusao { get; set; }
    public int? TotalHoras { get; set; }

    public long CandidatoId { get; set; }
    
    [JsonIgnore]
    public virtual Candidato? Candidato { get; set; }
}
