using Ciclo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ciclo.Infra.Data.Mappings;

public class AnotacoesMapping
{
    public void Configure(EntityTypeBuilder<Anotacoes> builder)
    {
        builder
            .Property(a => a.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(a => a.Descricao)
            .HasMaxLength(100000);
        
        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.AnotacoesList)
            .HasForeignKey(c => c.UserId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}