using System;

namespace Zambon.Core.Module.Exceptions
{
    /// <summary>
    /// Exception triggered when trying load a model language that does not exists.
    /// </summary>
    public class ModelNotFoundException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="language">The searched language.</param>
        public ModelNotFoundException(string language)
            : base($"Wasn't possible find any module loaded with language \"{language}\".")
        {

        }
    }
}