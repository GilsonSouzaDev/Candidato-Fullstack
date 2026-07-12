using System.ComponentModel.DataAnnotations;

namespace candidatos.Models;

public class Vaga
{
    public long Id { get; set; }
    
    [Required]
    public string Titulo { get; set; } = string.Empty;
    
    [Required]
    public string Descricao { get; set; } = string.Empty;
    
    public string Requisitos { get; set; } = string.Empty;
    
    // Novas propriedades detalhadas
    public string NomeEmpresa { get; set; } = string.Empty;
    public string Beneficios { get; set; } = string.Empty;
    public string Atividades { get; set; } = string.Empty;
    public string RequisitosObrigatorios { get; set; } = string.Empty;
    public string RequisitosDesejaveis { get; set; } = string.Empty;

    public decimal Salario { get; set; }
    public bool StatusAberta { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public long CriadoPorId { get; set; }
    public virtual Usuario? CriadoPor { get; set; }

    public virtual ICollection<Candidatura>? Candidaturas { get; set; }
}
