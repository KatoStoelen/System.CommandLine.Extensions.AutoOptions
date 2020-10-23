using System;
using System.ComponentModel;
using System.IO;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// An attribute specifying the default value of an option of type
    /// <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <remarks>
    /// Attribute arguments needs to be compile-time constant. To provide
    /// workaround for setting a default value of an option of type
    /// <see cref="DirectoryInfo"/>, this attribute, which extends
    /// <see cref="DefaultValueAttribute"/>, initializes a
    /// <see cref="DirectoryInfo"/> from the specified <see cref="string"/>
    /// (an absolute or relative path to the default directory).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DefaultDirectoryAttribute : DefaultValueAttribute
    {
        /// <summary>
        /// Sets a <see cref="DirectoryInfo"/> as default value of the
        /// option.
        /// </summary>
        /// <param name="path">
        /// The path (absolute or relative) to the default directory.
        /// </param>
        public DefaultDirectoryAttribute(string path)
            : base(new DirectoryInfo(path))
        {
        }
    }
}
