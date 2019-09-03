using System;

namespace Zambon.Core.Database.Domain.Attributes
{
    /// <summary>
    /// Attribute to inform the database that a specific property should be queried in server.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbQueryAttribute : Attribute
    {
        /// <summary>
        /// The database query.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Arguments to pass to the query.
        /// </summary>
        public string[] Args { get; set; }

        /// <summary>
        /// Attribute to inform the database that a specific property should be queried in server.
        /// </summary>
        /// <param name="Query">The database query.</param>
        public DbQueryAttribute(string Query)
        {
            this.Query = Query;
        }

        /// <summary>
        /// Attribute to inform the database that a specific property should be queried in server.
        /// </summary>
        /// <param name="Query">The database query.</param>
        /// <param name="Args">Arguments to pass to the query.</param>
        public DbQueryAttribute(string Query, params string[] Args)
        {
            this.Query = Query;
            this.Args = Args;
        }
    }
}