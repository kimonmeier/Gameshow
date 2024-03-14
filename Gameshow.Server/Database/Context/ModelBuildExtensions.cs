using Microsoft.EntityFrameworkCore;

namespace Gameshow.Server.Database.Context
{
    internal static class ModelBuildExtensions
    {
        /// <summary>
        /// Diese Methode registriert alle Configuration der angegebeben Assembly. Dabei werden diese rekursiv registriert.
        /// Das heisst, dass auch Abstrakte Klassen berücksichtigt werden.
        /// </summary>
        /// <typeparam name="TContext">Der aktuelle <see cref="DbContext"/>. Aus diesem wird die <see cref="Assembly"/> geladen</typeparam>
        public static void ApplyConfigurationsRecursive<TContext>(this ModelBuilder modelBuilder) where TContext : DbContext
        {
            Assembly persistenceAssembly = typeof(TContext).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(persistenceAssembly);
            IEnumerable<Type> types = persistenceAssembly.GetTypes().Where(x => x.IsPublic);

            foreach (Type type in types)
            {
                CheckForConfigurationsRecursive(modelBuilder, type, type);
            }
        }


        private static void CheckForConfigurationsRecursive(ModelBuilder modelBuilder, Type originalType, Type type)
        {
            Type? entityTypeConfiguration = type.GetInterfaces().Where(x => x.IsGenericType).FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

            if (entityTypeConfiguration is null)
            {
                if ((type.BaseType != null) && (type.BaseType != typeof(object)))
                {
                    CheckForConfigurationsRecursive(modelBuilder, originalType, type.BaseType);
                }

                return;
            }

            MethodInfo methodInfo = typeof(ModelBuilder).GetMethod(nameof(modelBuilder.ApplyConfiguration))!;

            methodInfo.MakeGenericMethod(entityTypeConfiguration.GetGenericArguments().First()).Invoke(modelBuilder, new[]
                {
                    Activator.CreateInstance(originalType)
                }
            );
        }
    }
}
