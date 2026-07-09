namespace candidatos.Models;

public class Candidatura
{
    public long Id { get; set; }
    public long CandidatoId { get; set; }
    public virtual Candidato? Candidato { get; set; }
    
    public long VagaId { get; set; }
    public virtual Vaga? Vaga { get; set; }

    public string StatusCandidatura { get; set; } = "Triagem";
    public DateTime DataAplicacao { get; set; } = DateTime.UtcNow;
}
