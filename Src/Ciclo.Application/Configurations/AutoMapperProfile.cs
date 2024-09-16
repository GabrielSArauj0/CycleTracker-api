using AutoMapper;
using Ciclo.Application.Dtos.V1.CicloMenstrual;
using Ciclo.Application.Dtos.V1.Usuario;
using Ciclo.Domain.Entities;

namespace Ciclo.Application.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Administrador

        CreateMap<Dtos.V1.Administrador.AdministradorDto, Administrador>().ReverseMap();
        CreateMap<Dtos.V1.Administrador.AdicionarAdministradorDto, Administrador>().ReverseMap();
        CreateMap<Dtos.V1.Administrador.AtualizarAdministradorDto, Administrador>().ReverseMap();

        #endregion

        #region Usuario

        CreateMap<UsuarioDto, Usuario>().ReverseMap();
        CreateMap<AdicionarUsuarioDto, Usuario>().ReverseMap();
        CreateMap<AtualizarUsuarioDto, Usuario>().ReverseMap();

        #endregion

        #region Ciclo Menstrual

        CreateMap<CicloMenstrualDto, CicloMenstrual>().ReverseMap();
        CreateMap<AdicionarCicloMenstrualDto, CicloMenstrual>().ReverseMap();
        CreateMap<AtualizarCicloMenstrualDto, CicloMenstrual>().ReverseMap();

        #endregion
    }
}