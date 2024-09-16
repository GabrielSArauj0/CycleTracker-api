using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Auth;
using Ciclo.Application.Notifications;
using Ciclo.Core.Enums;
using Ciclo.Core.Extensions;
using Ciclo.Core.Settings;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace Ciclo.Application.Services;

public class UsuarioAuthService : BaseService, IUsuarioAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    
    public UsuarioAuthService(INotificator notificator, IMapper mapper, IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher, IOptions<JwtSettings> jwtSettings, IJwtService jwtService) : base(notificator, mapper)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto?> Login(LoginDto login)
    {
        if (string.IsNullOrEmpty(login.Senha))
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var usuario = await _usuarioRepository.ObterPorEmail(login.Email);
        if (usuario == null || !usuario.Ativo)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
        
        if (_passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, login.Senha) !=
            PasswordVerificationResult.Failed)
            return new TokenDto
            {
                Token = await GerarToken(usuario)
            };
        
        Notificator.Handle("Não foi possível fazer o login");
        return null;
    }
    
    public async Task<string> GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
        claimsIdentity.AddClaim(
            new Claim("TipoUsuario", ETipoUsuario.Comum.ToDescriptionString())
        );
        
        var key = await _jwtService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Emissor,
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
            SigningCredentials = key,
            Claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Aud, _jwtSettings.Audiences() }
            }
        });

        return tokenHandler.WriteToken(token);
    }
}