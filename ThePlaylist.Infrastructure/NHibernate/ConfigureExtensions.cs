using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using ThePlaylist.Core.Projections;

namespace ThePlaylist.Infrastructure.NHibernate;

public static class ConfigureExtensions
{
    private static readonly MethodInfo ImportMethod
        = typeof(ModelMapper)
            .GetTypeInfo().GetDeclaredMethods(nameof(ModelMapper.Import))
            .Single(x => x.GetParameters().Length == 1);
    
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
    
    public static void AddProjectionsFromAssemblyNamespace(this Configuration configuration, Assembly assembly, string @namespace)
    {
        var mapper = new ModelMapper();
        
        var projections = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && 
                        t.Namespace?.StartsWith(@namespace) == true);

        foreach (var type in projections)
            ImportMethod.MakeGenericMethod(type).Invoke(mapper, new object[] { type.Name  });
        
        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        configuration.AddMapping(mapping);
    }
}