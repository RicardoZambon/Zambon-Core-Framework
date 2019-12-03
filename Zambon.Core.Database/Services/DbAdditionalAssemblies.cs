namespace Zambon.Core.Database.Services
{
    /// <summary>
    /// Defines additional assemblies to use when configuring the database.
    /// </summary>
    public interface IDbAdditionalAssemblies
    {
        /// <summary>
        /// List of assemblies names.
        /// </summary>
        string[] ReferencedAssemblies { get; }
    }
}