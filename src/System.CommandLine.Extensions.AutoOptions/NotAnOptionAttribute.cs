namespace System.CommandLine.Extensions.AutoOptions
{
    /// <summary>
    /// An attribute providing a way to exclude properties that should
    /// not be generated command line options for.
    /// </summary>
    /// <remarks>
    /// The automatic option creation looks for <see langword="public"/>
    /// properties. Hence, an alternative to this attribute would be to
    /// use a different accessability modifier.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NotAnOptionAttribute : Attribute { }
}
