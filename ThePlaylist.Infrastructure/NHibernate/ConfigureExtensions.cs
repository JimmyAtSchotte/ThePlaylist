using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ThePlaylist.Infrastructure.NHibernate;

public static class ConfigureExtensions
{
    public static void AddMappingsFromAssembly(this Configuration configuration, Assembly assembly)
    {
        var mapper = new ModelMapper();
        
        var mappingTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && 
                        t.BaseType.IsGenericType && 
                        t.BaseType.GetGenericTypeDefinition() == typeof(ClassMapping<>));

        foreach (var type in mappingTypes)
            mapper.AddMapping(type);
        
        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        configuration.AddMapping(mapping);
    }
    
}