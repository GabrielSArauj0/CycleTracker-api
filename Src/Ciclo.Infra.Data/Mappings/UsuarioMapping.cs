using Ciclo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ciclo.Infra.Data.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder
            .Property(a => a.Nome)
            .HasMaxLength(120)
            .IsRequired();
        
        builder
            .Property(a => a.Email)
            .HasMaxLength(120)
            .IsRequired();
        
        builder
            .Property(a => a.Senha)
            .HasMaxLength(250)
            .IsRequired();
    }
}