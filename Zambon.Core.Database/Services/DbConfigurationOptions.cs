namespace Zambon.Core.Database.Services
{
    public class DbConfigurationOptions
    {
        public string[] ReferencedAssemblies { get; private set; }

        public DbConfigurationOptions(string[] referencedAssemblies)
        {
            ReferencedAssemblies = referencedAssemblies;
        }
    }
}