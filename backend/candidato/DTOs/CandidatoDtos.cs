using System;
using System.Collections.Generic;

namespace candidato.DTOs;

public class CandidatoDto
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? ResumoProfissional { get; set; }
    public long UsuarioId { get; set; }
    
    public FiliacaoDto? Filiacao { get; set; }
    public EnderecoDto? Endereco { get; set; }
    public List<TelefoneDto>? Telefones { get; set; }
    public List<CursoDto>? Cursos { get; set; }
    public List<FormacaoDto>? Formacoes { get; set; }
    public List<HabilidadeDto>? Habilidades { get; set; }
    public List<ExperienciaDto>? Experiencias { get; set; }
}

public class FiliacaoDto
{
    public long Id { get; set; }
    public string? NomePai { get; set; }
    public string? NomeMae { get; set; }
}

public class EnderecoDto
{
    public long Id { get; set; }
    public string? Logradouro { get; set; }
    public string? Cep { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public CidadeDto? Cidade { get; set; }
}

public class CidadeDto
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public EstadoDto? Estado { get; set; }
}

public class EstadoDto
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Sigla { get; set; }
}

public class TelefoneDto
{
    public long Id { get; set; }
    public string? Numero { get; set; }
    public string? Tipo { get; set; }
}

public class CursoDto
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Instituicao { get; set; }
    public int? AnoConclusao { get; set; }
    public int? TotalHoras { get; set; }
}

public class FormacaoDto
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Instituicao { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Status { get; set; }
}

public class HabilidadeDto
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Tipo { get; set; }
    public string? Nivel { get; set; }
}

public class ExperienciaDto
{
    public long Id { get; set; }
    public string? Empresa { get; set; }
    public string? Cargo { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Descricao { get; set; }
}
