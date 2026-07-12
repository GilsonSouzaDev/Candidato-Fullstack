using AutoMapper;
using candidatos.Models;
using candidato.DTOs;

namespace candidato.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Candidato & related mapping
        CreateMap<Candidato, CandidatoDto>().ReverseMap();
        CreateMap<Filiacao, FiliacaoDto>().ReverseMap();
        CreateMap<Endereco, EnderecoDto>().ReverseMap();
        CreateMap<Cidade, CidadeDto>().ReverseMap();
        CreateMap<Estado, EstadoDto>().ReverseMap();
        CreateMap<Telefone, TelefoneDto>().ReverseMap();
        CreateMap<Curso, CursoDto>().ReverseMap();
        CreateMap<Formacao, FormacaoDto>().ReverseMap();
        CreateMap<Habilidade, HabilidadeDto>().ReverseMap();
        CreateMap<Experiencia, ExperienciaDto>().ReverseMap();
        
        // Vaga & related mapping
        CreateMap<Vaga, VagaDto>()
            .ForMember(dest => dest.Recrutador, opt => opt.MapFrom(src => src.CriadoPor != null ? src.CriadoPor.Nome : "Desconhecido"))
            .ReverseMap();
        CreateMap<Candidatura, CandidaturaDto>().ReverseMap();
    }
}
