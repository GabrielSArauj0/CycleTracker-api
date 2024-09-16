using AutoMapper;
using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.CicloMenstrual;
using Ciclo.Application.Dtos.V1.CicloMenstrual.FaseCiclo;
using Ciclo.Application.Notifications;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;

namespace Ciclo.Application.Services;

public class CicloMenstrualService : BaseService, ICicloMenstrualService
{
    private readonly ICicloMenstrualRepository _cicloMenstrualRepository;
    
    public CicloMenstrualService(INotificator notificator, IMapper mapper, ICicloMenstrualRepository cicloMenstrualRepository) : base(notificator, mapper)
    {
        _cicloMenstrualRepository = cicloMenstrualRepository;
    }

    public async Task<CicloMenstrualDto?> Adicionar(AdicionarCicloMenstrualDto dto)
    {
        var ciclo = Mapper.Map<CicloMenstrual>(dto);

        _cicloMenstrualRepository.Cadastrar(ciclo);
        if (await _cicloMenstrualRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<CicloMenstrualDto>(ciclo);
        }

        Notificator.Handle("Não foi possível cadastrar o ciclo menstrual");
        return null;
    }

    public async Task<CicloMenstrualDto?> Atualizar(int id, AtualizarCicloMenstrualDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem");
            return null;
        }

        var ciclo = await _cicloMenstrualRepository.ObterPorId(id);
        if (ciclo == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, ciclo);
        

        _cicloMenstrualRepository.Atualizar(ciclo);

        if (await _cicloMenstrualRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<CicloMenstrualDto>(ciclo);
        }

        Notificator.Handle("Não foi possível atualizar o ciclo menstrual");
        return null;
    }

    public async Task<CicloMenstrualDto?> ObterPorId(int id)
    {
        var ciclo = await _cicloMenstrualRepository.ObterPorId(id);
        if (ciclo != null)
        {
            return Mapper.Map<CicloMenstrualDto>(ciclo);
        }

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<FaseCicloDto?>> CalculoCiclo(int cicloId)
    {
        var ciclo = await _cicloMenstrualRepository.ObterPorId(cicloId);
        if (ciclo == null)
        {
            Notificator.HandleNotFoundResource();
            return new List<FaseCicloDto>();
        }

        var fases = CalcularFasesDoCiclo(ciclo);
        return Mapper.Map<List<FaseCicloDto>>(fases);
    }

    private List<FaseCicloDto> CalcularFasesDoCiclo(CicloMenstrual ciclo)
    {
        var fases = new List<FaseCicloDto>();
        DateTime inicioCiclo = ciclo.DataInicioUltimaMenstruacao;
        
        DateTime fimMenstruacao = inicioCiclo.AddDays(ciclo.DuracaoMenstruacao - 1);
        fases.Add(new FaseCicloDto { Nome = "Menstruação", Inicio = inicioCiclo, Fim = fimMenstruacao });
        
        DateTime ovulacao = inicioCiclo.AddDays(ciclo.DuracaoCiclo / 2);
        fases.Add(new FaseCicloDto { Nome = "Ovulação", Inicio = ovulacao, Fim = ovulacao });
        
        DateTime inicioFertil = ovulacao.AddDays(-4);
        DateTime fimFertil = ovulacao.AddDays(4);
        fases.Add(new FaseCicloDto { Nome = "Período Fértil", Inicio = inicioFertil, Fim = fimFertil });

        return fases;
    }
}