using AutoMapper;
using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Administrador;
using Ciclo.Application.Notifications;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ciclo.Application.Services;

public class AdministradorService : BaseService, IAdministradorService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IPasswordHasher<Administrador> _passwordHasher;
    
    public AdministradorService(INotificator notificator,
        IMapper mapper, 
        IAdministradorRepository administradorRepository, 
        IPasswordHasher<Administrador> passwordHasher) 
        : base(notificator, mapper)
    {
        _administradorRepository = administradorRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AdministradorDto?> Adicionar(AdicionarAdministradorDto dto)
    {

        var administrador = Mapper.Map<Administrador>(dto);
        if (!await Validar(administrador))
        {
            return null;
        }
        
        administrador.Senha = _passwordHasher.HashPassword(administrador, administrador.Senha);
        _administradorRepository.Cadastrar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        Notificator.Handle("Não foi possível cadastrar o administrador");
        return null;
    }

    public async Task<AdministradorDto?> Atualizar(int id, AtualizarAdministradorDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem");
            return null;
        }

        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, administrador);
        
        if (!await Validar(administrador))
        {
            return null;
        }
        
        administrador.Senha = _passwordHasher.HashPassword(administrador, administrador.Senha);
        _administradorRepository.Atualizar(administrador);
        
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.Handle("Não foi possível atualizar o administrador");
        return null;
    }

    public async Task<AdministradorDto?> ObterPorId(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador != null)
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AdministradorDto?> ObterPorEmail(string email)
    {
        var administrador = await _administradorRepository.ObterPorEmail(email);
        if (administrador != null)
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<AdministradorDto>> ObterTodos()
    {
        var administradores = await _administradorRepository.ObterTodos();
        return Mapper.Map<List<AdministradorDto>>(administradores);
    }

    public async Task Ativar(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        administrador.Ativo = true;
        _administradorRepository.Atualizar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível ativar o administrador");
    }

    public async Task Desativar(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        administrador.Ativo = false;
        _administradorRepository.Atualizar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível desativar o administrador");
    }

    private async Task<bool> Validar(Administrador administrador)
    {
        if (!administrador.Validate(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }
        
        var administradorExistente = await _administradorRepository.FirstOrDefault(a => 
            a.Email == administrador.Email  && a.Id != administrador.Id);
        if (administradorExistente != null)
        {
            Notificator.Handle($"Já existe um administrador {(administradorExistente.Ativo ? "ativo" : "desativado")} cadastrado com essas identificações");
        }

        return !Notificator.HasNotification;
    }
}