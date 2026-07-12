using System;
using System.Collections.Generic;

namespace candidato.DTOs;

public class VagaDto
{
    public long Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Requisitos { get; set; }
    public string? NomeEmpresa { get; set; }
    public string? Beneficios { get; set; }
    public string? Atividades { get; set; }
    public string? RequisitosObrigatorios { get; set; }
    public string? RequisitosDesejaveis { get; set; }
    public decimal? Salario { get; set; }
    public bool StatusAberta { get; set; }
    public DateTime? DataCriacao { get; set; }
    public long CriadoPorId { get; set; }
    public string? Recrutador { get; set; }

    public List<CandidaturaDto>? Candidaturas { get; set; }
}

