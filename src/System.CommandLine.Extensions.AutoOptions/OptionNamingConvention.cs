namespace System.CommandLine.Extensions.AutoOptions
{
    /// <summary>
    /// Naming convantions of options.
    /// </summary>
    [Flags]
    public enum OptionNamingConvention
    {
        /// <summary>
        /// Use the name of the option class property.
        /// <para>
        /// E.g. using prefix '-':
        /// </para>
        /// <para>
        /// <c>public string OptionName { get; } // yields: -OptionName</c>
        /// </para>
        /// </summary>
        MatchPropertyName = 0,

        /// <summary>
        /// Lower case everything.
        /// <para>
        /// E.g. using prefix '/':
        /// </para>
        /// <para>
        /// <c>public string OptionName { get; } // yields: /optionname</c>
        /// </para>
        /// </summary>
        LowerCase = 1 << 0,

        /// <summary>
        /// Upper case everything.
        /// <para>
        /// E.g. using prefix '/':
        /// </para>
        /// <para>
        /// <c>public string OptionName { get; } // yields: /OPTIONNAME</c>
        /// </para>
        /// </summary>
        UpperCase = 1 << 1,

        /// <summary>
        /// Use kebab case.
        /// <para>
        /// E.g. using prefix '--':
        /// </para>
        /// <para>
        /// <c>public string OptionName { get; } // yields: --option-name</c>
        /// </para>
        /// </summary>
        KebabCase = (1 << 2) | LowerCase,

        /// <summary>
        /// Use SCREAMING KEBAB CASE.
        /// <para>
        /// E.g. using prefix '--':
        /// </para>
        /// <para>
        /// <c>public string OptionName { get; } // yields: --OPTION-NAME</c>
        /// </para>
        /// </summary>
        ScreamingKebabCase = KebabCase | UpperCase
    }
}