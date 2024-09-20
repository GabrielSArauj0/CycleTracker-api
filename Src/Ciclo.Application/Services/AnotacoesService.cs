using AutoMapper;
using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Anotacoes;
using Ciclo.Application.Notifications;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;

namespace Ciclo.Application.Services;

public class AnotacoesService : BaseService, IAnotacoesService
{
    private readonly IAnotacoesRepository _anotacoesRepository;
    
    public AnotacoesService(INotificator notificator, IMapper mapper, IAnotacoesRepository anotacoesRepository) : base(notificator, mapper)
    {
        _anotacoesRepository = anotacoesRepository;
    }

    public async Task<AnotacaoDto?> Adicionar(AtualizarAnotacaoDto dto)
    {
        var anotacao = Mapper.Map<Anotacoes>(dto);
      
        anotacao.Ativo = true;
       _anotacoesRepository.Save(anotacao);
        if (await _anotacoesRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AnotacaoDto>(anotacao);
        }
        Notificator.Handle("Não foi possível cadastrar uma anotação");
        return null;
    }
    public async Task<AnotacaoDto?> Atualizar(int id, AtualizarAnotacaoDto dto)
    {
        
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem");
            return null;
        }

        var anotacao = await _anotacoesRepository.ObterPorId(id);
        if (anotacao == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, anotacao);


       _anotacoesRepository.Update(anotacao);

        if (await _anotacoesRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AnotacaoDto>(anotacao);
        }

        Notificator.Handle("Não foi possível atualizar a anotação");
        return null;
        
    }

    public async Task<AnotacaoDto?> ObterPorId(int id)
    {
        var anotacao = await _anotacoesRepository.ObterPorId(id);
        if (anotacao == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        return Mapper.Map<AnotacaoDto>(anotacao);
    }
    
    
    public async Task<bool> Delete(int id)
    {
        var anotacao = await _anotacoesRepository.ObterPorId(id);
        if (anotacao == null)
        {
            Notificator.HandleNotFoundResource();
            return false;
        }

        anotacao.Ativo = false; 
        _anotacoesRepository.Update(anotacao);

        if (await _anotacoesRepository.UnitOfWork.Commit())
        {
            return true;
        }

        Notificator.Handle("Não foi possível remover a anotação");
        return false;
    }

    public async Task<List<AnotacaoDto>> AnotacoesUser(int userId)
    {
        throw new NotImplementedException();
    }
}