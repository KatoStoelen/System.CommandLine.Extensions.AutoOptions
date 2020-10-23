using System;
using System.ComponentModel;
using System.IO;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// An attribute specifying the default value of an option of type
    /// <see cref="FileInfo"/>.
    /// </summary>
    /// <remarks>
    /// Attribute arguments needs to be compile-time constant. To provide
    /// workaround for setting a default value of an option of type
    /// <see cref="FileInfo"/>, this attribute, which extends
    /// <see cref="DefaultValueAttribute"/>, initializes a
    /// <see cref="FileInfo"/> from the specified <see cref="string"/>
    /// (an absolute or relative path to the default file).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DefaultFileAttribute : DefaultValueAttribute
    {
        /// <summary>
        /// Sets a <see cref="FileInfo"/> as default value of the
        /// option.
        /// </summary>
        /// <param name="path">
        /// The path (absolute or relative) to the default file.
        /// </param>
        public DefaultFileAttribute(string path)
            : base(new FileInfo(path))
        {
        }
    }
}
