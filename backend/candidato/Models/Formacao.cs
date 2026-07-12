using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace candidatos.Models;

public class Formacao
{
    public long Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    public string? Instituicao { get; set; }
    
    // Concluído, Em Andamento, Trancado, etc.
    public string? Status { get; set; }

    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public long CandidatoId { get; set; }
    
    [JsonIgnore]
    public virtual Candidato? Candidato { get; set; }
}
