using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace candidatos.Models;


public class Endereco
{
    public long Id { get; set; }
    public string? Logradouro { get; set; }
    public string? Cep { get; set; }
    public string? Numero { get; set; }
    public virtual Cidade? Cidade { get; set; }
    public long CidadeId { get; internal set; }
}
    