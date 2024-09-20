using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;
using Ciclo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Ciclo.Infra.Data.Repositories;

public class AnotacoesRepository : Repository<Anotacoes>, IAnotacoesRepository 
{
    public AnotacoesRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Anotacoes?> ObterPorId(int id)
    {
        return await Context.Anotacoes.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(a => a.Id == id);
    }

    public void Save(Anotacoes anotacoes)
    {
        Context.Anotacoes.Add(anotacoes);
    }

    public void Update(Anotacoes anotacoes)
    {
        Context.Anotacoes.Update(anotacoes);
    }

    public void Delete(Anotacoes anotacoes)
    {
        Context.Anotacoes.Remove(anotacoes);
    }
}