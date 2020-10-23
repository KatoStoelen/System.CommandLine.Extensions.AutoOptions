using System;
using System.CommandLine;
using System.Reflection;
using SystemCommandLineExtensions.AutoOptions.Internal;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// Extensions of <see cref="Command"/>.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Adds the options defined in the class <typeparamref name="TOptions"/>
        /// to the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="prefix">
        /// The prefix to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionPrefix.TwoHyphens"/>.
        /// </para>
        /// </param>
        /// <param name="namingConvention">
        /// The naming convention to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionNamingConvention.KebabCase"/>.
        /// </para>
        /// </param>
        /// <typeparam name="TOptions">The class defining options of this command.</typeparam>
        public static void AddOptions<TOptions>(
                this Command command,
                OptionPrefix prefix = OptionPrefix.TwoHyphens,
                OptionNamingConvention namingConvention = OptionNamingConvention.KebabCase) where TOptions : class =>
            command.AddOptions(typeof(TOptions), prefix, namingConvention);

        /// <summary>
        /// Adds the options defined in the class <paramref name="optionsType"/>
        /// to the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="optionsType">The class defining options of this command.</param>
        /// <param name="prefix">
        /// The prefix to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionPrefix.TwoHyphens"/>.
        /// </para>
        /// </param>
        /// <param name="namingConvention">
        /// The naming convention to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionNamingConvention.KebabCase"/>.
        /// </para>
        /// </param>
        public static void AddOptions(
                this Command command,
                Type optionsType,
                OptionPrefix prefix = OptionPrefix.TwoHyphens,
                OptionNamingConvention namingConvention = OptionNamingConvention.KebabCase) =>
            command.AddOptions(
                optionsType,
                property => property.Name.ToOptionName(prefix, namingConvention));

        /// <summary>
        /// Adds the options defined in the class <typeparamref name="TOptions"/>
        /// to the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="defaultOptionNamer">
        /// A delegate, accepting a <see cref="PropertyInfo"/>, that returns the default
        /// name of corresponding command line option.
        /// </param>
        /// <typeparam name="TOptions">The class defining options of this command.</typeparam>
        public static void AddOptions<TOptions>(
                this Command command,
                Func<PropertyInfo, string> defaultOptionNamer) where TOptions : class =>
            command.AddOptions(typeof(TOptions), defaultOptionNamer);

        /// <summary>
        /// Adds the options defined in the class <paramref name="optionsType"/>
        /// to the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="optionsType">The class defining options of this command.</param>
        /// <param name="defaultOptionNamer">
        /// A delegate, accepting a <see cref="PropertyInfo"/>, that returns the default
        /// name of corresponding command line option.
        /// </param>
        public static void AddOptions(
                this Command command,
                Type optionsType,
                Func<PropertyInfo, string> defaultOptionNamer) =>
            command.AddOptions(
                optionsType,
                defaultOptionNamer,
                (command, option) => command.AddOption(option));

        /// <summary>
        /// Adds the options defined in the class <typeparamref name="TGlobalOptions"/>
        /// as global options of the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="prefix">
        /// The prefix to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionPrefix.TwoHyphens"/>.
        /// </para>
        /// </param>
        /// <param name="namingConvention">
        /// The naming convention to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionNamingConvention.KebabCase"/>.
        /// </para>
        /// </param>
        /// <typeparam name="TGlobalOptions">
        /// The class defining global options of this command.
        /// </typeparam>
        public static void AddGlobalOptions<TGlobalOptions>(
                this Command command,
                OptionPrefix prefix = OptionPrefix.TwoHyphens,
                OptionNamingConvention namingConvention = OptionNamingConvention.KebabCase) where TGlobalOptions : class =>
            command.AddGlobalOptions(typeof(TGlobalOptions), prefix, namingConvention);

        /// <summary>
        /// Adds the options defined in the class <paramref name="globalOptionsType"/>
        /// as global options of the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="globalOptionsType">The class defining global options of this command.</param>
        /// <param name="prefix">
        /// The prefix to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionPrefix.TwoHyphens"/>.
        /// </para>
        /// </param>
        /// <param name="namingConvention">
        /// The naming convention to use for options' default name.
        /// <para>
        /// Defaults to <see cref="OptionNamingConvention.KebabCase"/>.
        /// </para>
        /// </param>
        public static void AddGlobalOptions(
                this Command command,
                Type globalOptionsType,
                OptionPrefix prefix = OptionPrefix.TwoHyphens,
                OptionNamingConvention namingConvention = OptionNamingConvention.KebabCase) =>
            command.AddGlobalOptions(
                globalOptionsType,
                property => property.Name.ToOptionName(prefix, namingConvention));

        /// <summary>
        /// Adds the options defined in the class <typeparamref name="TGlobalOptions"/>
        /// as global options of the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="defaultOptionNamer">
        /// A delegate, accepting a <see cref="PropertyInfo"/>, that returns the default
        /// name of corresponding command line option.
        /// </param>
        /// <typeparam name="TGlobalOptions">
        /// The class defining global options of this command.
        /// </typeparam>
        public static void AddGlobalOptions<TGlobalOptions>(
                this Command command,
                Func<PropertyInfo, string> defaultOptionNamer) where TGlobalOptions : class =>
            command.AddGlobalOptions(typeof(TGlobalOptions), defaultOptionNamer);

        /// <summary>
        /// Adds the options defined in the class <paramref name="globalOptionsType"/>
        /// as global options of the current <see cref="Command"/>.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="globalOptionsType">The class defining global options of this command.</param>
        /// <param name="defaultOptionNamer">
        /// A delegate, accepting a <see cref="PropertyInfo"/>, that returns the default
        /// name of corresponding command line option.
        /// </param>
        public static void AddGlobalOptions(
                this Command command,
                Type globalOptionsType,
                Func<PropertyInfo, string> defaultOptionNamer) =>
            command.AddOptions(
                globalOptionsType,
                defaultOptionNamer,
                (command, option) => command.AddGlobalOption(option));

        private static void AddOptions(
            this Command command,
            Type optionsType,
            Func<PropertyInfo, string> defaultOptionNamer,
            Action<Command, Option> addOption)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (defaultOptionNamer == null)
                throw new ArgumentNullException(nameof(defaultOptionNamer));
            if (optionsType == null)
                throw new ArgumentNullException(nameof(optionsType));

            if (!optionsType.IsClass)
                throw new ArgumentException($"Options type '{optionsType.FullName}' is not a class");

            foreach (var option in optionsType.GetOptions(defaultOptionNamer))
            {
                addOption.Invoke(command, option);
            }
        }
    }
}