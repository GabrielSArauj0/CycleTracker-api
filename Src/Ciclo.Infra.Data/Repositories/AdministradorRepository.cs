using Microsoft.EntityFrameworkCore;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;
using Ciclo.Infra.Data.Context;

namespace Ciclo.Infra.Data.Repositories;

public class AdministradorRepository : Repository<Administrador>, IAdministradorRepository
{
    public AdministradorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Cadastrar(Administrador administrador)
    {
        Context.Administradores.Add(administrador);
    }

    public void Atualizar(Administrador administrador)
    {
        Context.Administradores.Update(administrador);
    }

    public async Task<Administrador?> ObterPorId(int id)
    {
        return await Context.Administradores.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Administrador?> ObterPorEmail(string email)
    {
        return await Context.Administradores.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<List<Administrador>> ObterTodos()
    {
        return await Context.Administradores.AsNoTrackingWithIdentityResolution().ToListAsync();
    }
}