using AutoMapper;
using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Usuario;
using Ciclo.Application.Notifications;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ciclo.Application.Services;

public class UsuarioService : BaseService, IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    
    public UsuarioService(INotificator notificator, IMapper mapper, IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher) : base(notificator, mapper)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UsuarioDto?> Adicionar(AdicionarUsuarioDto dto)
    {
        var usuario = Mapper.Map<Usuario>(dto);
        if (!await Validar(usuario))
        {
            return null;
        }
        
        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);
        usuario.Ativo = true;
        
        _usuarioRepository.Cadastrar(usuario);
        if (await _usuarioRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<UsuarioDto>(usuario);
        }
        Notificator.Handle("Não foi possível cadastrar o usuario");
        return null;
    }

    public Task<UsuarioDto?> Atualizar(int id, UsuarioDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<UsuarioDto?> ObterPorId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UsuarioDto?> ObterPorEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<List<UsuarioDto>> ObterTodos()
    {
        throw new NotImplementedException();
    }

    public Task Ativar(int id)
    {
        throw new NotImplementedException();
    }

    public Task Desativar(int id)
    {
        throw new NotImplementedException();
    }
    
    private async Task<bool> Validar(Usuario usuario)
    {
        if (!usuario.Validate(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }
        
        var administradorExistente = await _usuarioRepository.FirstOrDefault(a => 
            a.Email == usuario.Email  && a.Id != usuario.Id);
        if (administradorExistente != null)
        {
            Notificator.Handle($"Já existe um usuario {(usuario.Ativo ? "ativo" : "desativado")} cadastrado com esse email");
        }

        return !Notificator.HasNotification;
    }
}