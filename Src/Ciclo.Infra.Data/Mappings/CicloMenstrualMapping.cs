using Ciclo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ciclo.Infra.Data.Mappings;

public class CicloMenstrualMapping : IEntityTypeConfiguration<CicloMenstrual>
{
    public void Configure(EntityTypeBuilder<CicloMenstrual> builder)
    {
        builder.Property(c => c.UsuarioId)
            .IsRequired();
        
        builder.Property(c => c.DataInicioUltimaMenstruacao)
            .IsRequired(); // Todo: olhar se data está certa
        
        builder.Property(c => c.DuracaoCiclo)
            .IsRequired();
        
        builder.Property(c => c.DuracaoMenstruacao)
            .IsRequired();
        
        builder.Property(c => c.MetodoContraceptivo)
            .HasConversion<int>()
            .IsRequired(false);
        
        builder.Property(c => c.IntensidadeFluxo)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(c => c.Ativo)
            .IsRequired();
        
        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.CiclosMenstruais)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict); // colocar um HasIndex new{id, userId}
    }
}