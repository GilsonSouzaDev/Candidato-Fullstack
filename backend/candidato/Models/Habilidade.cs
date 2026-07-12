using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace candidatos.Models;

public class Habilidade
{
    public long Id { get; set; }

    public string? Nome { get; set; }
    
    // "HardSkill" or "SoftSkill"
    public string? Tipo { get; set; }
    
    // e.g. "Básico", "Intermediário", "Avançado"
    public string? Nivel { get; set; }

    public long CandidatoId { get; set; }
    
    [JsonIgnore]
    public virtual Candidato? Candidato { get; set; }
}
