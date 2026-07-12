using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace candidatos.Models;

public class Experiencia
{
    public long Id { get; set; }

    public string? Empresa { get; set; }
    public string? Cargo { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Descricao { get; set; }

    public long CandidatoId { get; set; }
    
    [JsonIgnore]
    public virtual Candidato? Candidato { get; set; }
}
