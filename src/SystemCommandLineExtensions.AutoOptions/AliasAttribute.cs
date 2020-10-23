using System;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// An attribute specifying an alias of a command option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// Adds the specified alias to this option.
        /// </summary>
        /// <remarks>
        /// The alias is added exactly as specified. The is no automatic
        /// adding of prefix or word delimiters etc.
        /// </remarks>
        /// <param name="alias">The alias of the option.</param>
        public AliasAttribute(string alias)
        {
            Alias = alias;
        }

        /// <summary>
        /// The alias of the option.
        /// </summary>
        public string Alias { get; }
    }
}