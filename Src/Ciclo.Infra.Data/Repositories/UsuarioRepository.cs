using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;
using Ciclo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Ciclo.Infra.Data.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Cadastrar(Usuario usuario)
    {
        Context.Usuarios.Add(usuario);
    }

    public void Atualizar(Usuario usario)
    {
        Context.Usuarios.Update(usario);
    }

    public async Task<Usuario?> ObterPorId(int id)
    {
        return await Context.Usuarios.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Usuario?> ObterPorEmail(string email)
    {
        return await Context.Usuarios.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<List<Usuario>> ObterTodos()
    {
        return await Context.Usuarios.AsNoTrackingWithIdentityResolution().ToListAsync();
    }
}