using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ciclo.Domain.Contracts;

namespace Ciclo.Infra.Data.Extensions;

public static class ModelBuilderExtension
{
    public static void ApplyEntityConfiguration(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.GetEntities<IEntity>();
        var props = entities.SelectMany(c => c.GetProperties()).ToList();

        foreach (var property in props.Where(c => c.ClrType == typeof(int) && c.Name == "Id"))
        {
            property.IsKey();
        }
    }
    
    public static void ApplySoftDeleteConfiguration(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.GetEntities<ISoftDelete>();
        var props = entities.SelectMany(c => c.GetProperties()).ToList();

        foreach (var property in props.Where(c => c.ClrType == typeof(int) && c.Name == "Ativo"))
        {
            property.SetDefaultValue(true);
        }
    }
    
    public static void ApplyTrackingConfiguration(this ModelBuilder modelBuilder)
    {
        var propDatas = new[] { "CriadoEm", "AtualizadoEm" };
        var propInts = new[] { "CriadoPor", "AtualizadoPor" };

        var entidades = modelBuilder.GetEntities<ITracking>();

        var dataProps = entidades
            .SelectMany(c 
                => c.GetProperties().Where(p => p.ClrType == typeof(DateTime) && propDatas.Contains(p.Name)));
        
        var intProps = entidades
            .SelectMany(c 
                => c.GetProperties().Where(p => p.ClrType == typeof(int) && propInts.Contains(p.Name)));

        foreach (var prop in dataProps)
        {
            prop.SetColumnType("timestamp");
            prop.SetDefaultValueSql("CURRENT_TIMESTAMP");
        }
        
        foreach (var prop in intProps)
        {
            prop.IsNullable = false;
        }
    }
    
    private static List<IMutableEntityType> GetEntities<T>(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(c => c.ClrType.GetInterface(typeof(T).Name) != null).ToList();

        return entities;
    }
}