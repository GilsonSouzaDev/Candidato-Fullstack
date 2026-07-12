namespace candidatos.Models;

public class Usuario
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Candidato"; // Admin, Recrutador, Candidato
    
    public virtual ICollection<Vaga>? VagasCriadas { get; set; }
    public virtual Candidato? CandidatoPerfil { get; set; }
}
