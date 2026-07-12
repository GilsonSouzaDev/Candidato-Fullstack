using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using candidatos.Models;
using Microsoft.EntityFrameworkCore;
using candidatos.Data;

namespace candidato.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegracaoController : ControllerBase
{
    private readonly CandidatoContext _context;

    public IntegracaoController(CandidatoContext context)
    {
        _context = context;
    }


    [HttpGet("cursos-graduacao")]
    public IActionResult GetCursosGraduacao()
    {
        // Simulating an external API containing Brazilian higher education courses
        var cursos = new List<string>
        {
            "Administração", "Agronomia", "Análise e Desenvolvimento de Sistemas", "Arquitetura e Urbanismo", 
            "Artes Cênicas", "Artes Visuais", "Automação Industrial", "Banco de Dados",
            "Biomedicina", "Ciência da Computação", "Ciências Biológicas",
            "Ciências Contábeis", "Ciências Econômicas", "Design de Interação",
            "Design Gráfico", "Design de Moda", "Direito", "Educação Física", "Enfermagem",
            "Engenharia Agronômica", "Engenharia Ambiental", "Engenharia Civil",
            "Engenharia da Computação", "Engenharia de Controle e Automação",
            "Engenharia de Produção", "Engenharia de Software",
            "Engenharia Elétrica", "Engenharia Mecânica", "Engenharia Química",
            "Estatística", "Farmácia", "Filosofia", "Física", "Fisioterapia", 
            "Fonoaudiologia", "Gastronomia", "Geografia", "Gestão de Recursos Humanos",
            "Gestão Financeira", "Gestão Pública", "Gestão da Tecnologia da Informação",
            "História", "Jornalismo", "Letras", "Logística", "Matemática",
            "Medicina", "Medicina Veterinária", "Nutrição", "Odontologia",
            "Pedagogia", "Psicologia", "Publicidade e Propaganda",
            "Química", "Radiologia", "Relações Internacionais", "Redes de Computadores", 
            "Sistemas de Informação", "Sistemas para Internet", "Serviço Social", 
            "Turismo", "Zootecnia"
        };
        
        return Ok(cursos.OrderBy(c => c).ToList());
    }
}
