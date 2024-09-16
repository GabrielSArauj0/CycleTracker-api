using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Domain.Entities;
using Ciclo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Ciclo.Infra.Data.Repositories;

public class CicloMenstrualRepository : Repository<CicloMenstrual>, ICicloMenstrualRepository
{
    public CicloMenstrualRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Cadastrar(CicloMenstrual cicloMenstrual)
    {
        Context.CicloMenstruals.Add(cicloMenstrual);
    }

    public void Atualizar(CicloMenstrual cicloMenstrual)
    {
        Context.CicloMenstruals.Update(cicloMenstrual);
    }

    public async Task<CicloMenstrual?> ObterPorId(int id)
    {
        return await Context.CicloMenstruals
            .AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(c => c.Id == id);
    }
}