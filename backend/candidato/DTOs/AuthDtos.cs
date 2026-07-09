using System.ComponentModel.DataAnnotations;

namespace candidato.DTOs;

public class RegisterDto
{
    [Required]
    public string Nome { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, MinLength(6)]
    public string Senha { get; set; } = string.Empty;
}

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Senha { get; set; } = string.Empty;
}
