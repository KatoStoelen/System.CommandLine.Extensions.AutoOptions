using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.CommandLine.Extensions.AutoOptions
{
    /// <summary>
    /// Extensions of <see cref="Type"/> to generate <see cref="Option"/>s.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Generates <see cref="Option"/>s based on the properties of
        /// the current <see cref="Type"/>.
        /// <para>
        /// To add aliases of options, use the <see cref="AliasAttribute"/>.
        /// The description of the option can be set using
        /// <see cref="DescriptionAttribute"/>, and default value can
        /// be set using <see cref="DefaultValueAttribute"/>, or any
        /// attribute that extends <see cref="DefaultValueAttribute"/> (e.g.
        /// <see cref="DefaultDirectoryAttribute"/>,
        /// <see cref="DefaultFileAttribute"/> or your own custom implementation).
        /// </para>
        /// </summary>
        /// <param name="optionsType">
        /// The <see cref="Type"/> containing option properties.
        /// </param>
        /// <param name="prefix">
        /// An optional prefix of options' default name.
        /// <para>
        /// Defaults to '--'.
        /// </para>
        /// </param>
        /// <param name="namingConvention">
        /// The naming convention to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionNamingConvention.KebabCase"/>.
        /// </para>
        /// </param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Option"/>.</returns>
        public static IEnumerable<Option> GetOptions(
                this Type optionsType,
                string? prefix = "--",
                OptionNamingConvention namingConvention = OptionNamingConvention.KebabCase) =>
            optionsType.GetOptions(property => property.Name.ToOptionName(prefix, namingConvention));

        /// <summary>
        /// Generates <see cref="Option"/>s based on the properties of
        /// the current <see cref="Type"/>.
        /// <para>
        /// To add aliases of options, use the <see cref="AliasAttribute"/>.
        /// The description of the option can be set using
        /// <see cref="DescriptionAttribute"/>, and default value can
        /// be set using <see cref="DefaultValueAttribute"/>, or any
        /// attribute that extends <see cref="DefaultValueAttribute"/> (e.g.
        /// <see cref="DefaultDirectoryAttribute"/>,
        /// <see cref="DefaultFileAttribute"/> or your own custom implementation).
        /// </para>
        /// </summary>
        /// <param name="optionsType">The <see cref="Type"/> containing option properties.</param>
        /// <param name="defaultOptionNamer">
        /// A delegate, accepting a <see cref="PropertyInfo"/>, that returns the default name
        /// of the option.
        /// </param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Option"/>.</returns>
        public static IEnumerable<Option> GetOptions(
            this Type optionsType, Func<PropertyInfo, string> defaultOptionNamer)
        {
            if (defaultOptionNamer == null)
            {
                throw new ArgumentNullException(nameof(defaultOptionNamer));
            }

            var optionProperties = optionsType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(property => property.GetCustomAttribute<NotAnOptionAttribute>() == null);

            foreach (var optionProperty in optionProperties)
            {
                var defaultOptionName = defaultOptionNamer.Invoke(optionProperty);
                var aliases = new[] { defaultOptionName }
                    .Concat(optionProperty
                        .GetCustomAttributes<AliasAttribute>()
                        .Select(attribute => attribute.Alias))
                    .ToArray();
                var description = optionProperty.GetCustomAttribute<DescriptionAttribute>()?.Description;
                var defaultValue = optionProperty.GetCustomAttribute<DefaultValueAttribute>()?.Value;

                var closedGenericOptionType = typeof(Option<>).MakeGenericType(optionProperty.PropertyType);

                yield return defaultValue == null
                    ? (Option)Activator.CreateInstance(closedGenericOptionType, aliases, description)!
                    : (Option)Activator.CreateInstance(
                        closedGenericOptionType,
                        aliases,
                        Expression.Lambda(Expression.Constant(defaultValue)).Compile(),
                        description)!;
            }
        }

        internal static string ToOptionName(
            this string optionPropertyName,
            string? prefix,
            OptionNamingConvention namingConvention)
        {
            var wordDelimiter = namingConvention.GetWordDelimiter();
            var setCasing = namingConvention.GetCasingFunc();

            var shouldAddWordDelimiter = !string.IsNullOrEmpty(wordDelimiter);

            return prefix + (
                !shouldAddWordDelimiter
                    ? setCasing.Invoke(optionPropertyName)
                    : optionPropertyName.Aggregate(
                        string.Empty,
                        (optionName, currentChar) =>
                            optionName +
                            (char.IsUpper(currentChar) && optionName.Length > 0 ? wordDelimiter : string.Empty) +
                            setCasing.Invoke(new string(currentChar, 1)))
                );
        }

        private static string GetWordDelimiter(this OptionNamingConvention namingConvention) =>
            namingConvention switch
            {
                var conv when conv.HasFlag(OptionNamingConvention.KebabCase) => "-",
                _ => string.Empty
            };

        private static Func<string, string> GetCasingFunc(this OptionNamingConvention namingConvention) =>
            namingConvention switch
            {
                var conv when conv.HasFlag(OptionNamingConvention.UpperCase) => parameterName => parameterName.ToUpper(),
                var conv when conv.HasFlag(OptionNamingConvention.LowerCase) => parameterName => parameterName.ToLower(),
                _ => parameterName => parameterName
            };
    }
}