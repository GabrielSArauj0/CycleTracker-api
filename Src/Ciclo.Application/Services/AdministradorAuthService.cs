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

public class AdministradorAuthService : BaseService, IAdministradorAuthService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IPasswordHasher<Administrador> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    
    public AdministradorAuthService(INotificator notificator, IMapper mapper, IPasswordHasher<Administrador> passwordHasher, IAdministradorRepository administradorRepository, IOptions<JwtSettings> jwtSettings, IJwtService jwtService) : base(notificator, mapper)
    {
        _passwordHasher = passwordHasher;
        _administradorRepository = administradorRepository;
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

        var adiministrador = await _administradorRepository.ObterPorEmail(login.Email);
        if (adiministrador == null || !adiministrador.Ativo)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
        
        if (_passwordHasher.VerifyHashedPassword(adiministrador, adiministrador.Senha, login.Senha) !=
            PasswordVerificationResult.Failed)
            return new TokenDto
            {
                Token = await GerarToken(adiministrador)
            };
        
        Notificator.Handle("Não foi possível fazer o login");
        return null;

    }

    public async Task<string> GerarToken(Administrador administrador)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, administrador.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, administrador.Nome));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, administrador.Email));
        claimsIdentity.AddClaim(new Claim("Administrador", administrador.Adm.ToString().ToLower()));
        claimsIdentity.AddClaim(
            new Claim("TipoUsuario",
                administrador.Adm
                    ? ETipoUsuario.Administrador.ToDescriptionString()
                    : ETipoUsuario.Comum.ToDescriptionString())
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