using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace candidatos.Models;

public enum TipoTelefone
{
    [Description("Residencal")]
    Residencial = 1,
    [Description("Celular")]
    Celular = 2,
    [Description("Contato")]
    Contato = 3
}
