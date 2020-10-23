using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using System.Threading.Tasks;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// Represents a <see cref="Command"/> with a belonging options class.
    /// </summary>
    /// <typeparam name="TOptions">The class containing the options of this command.</typeparam>
    public abstract class Command<TOptions> : Command where TOptions : class
    {
        /// <summary>
        /// Initializes a command with a belonging options class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">An optional description of the command.</param>
        /// <param name="optionPrefix">
        /// The prefix of options' default name.
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
        protected Command(
                string name,
                string? description = null,
                OptionPrefix optionPrefix = OptionPrefix.TwoHyphens,
                OptionNamingConvention namingConvention = OptionNamingConvention.KebabCase)
            : this(name, property => property.Name.ToOptionName(optionPrefix, namingConvention), description)
        {
        }

        /// <summary>
        /// Initializes a command with a belonging options class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="defaultOptionNamer">
        /// A delegate, accepting a <see cref="PropertyInfo"/>, that returns the default name
        /// of the option.
        /// </param>
        /// <param name="description">An optional description of the command.</param>
        protected Command(
                string name,
                Func<PropertyInfo, string> defaultOptionNamer,
                string? description = null)
            : base(name, description)
        {
            var options = typeof(TOptions).GetOptions(defaultOptionNamer);

            foreach (var option in options)
            {
                AddOption(option);
            }

            Handler = CommandHandler.Create((TOptions o) => ExecuteAsync(o));
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="options">The parsed options of the command.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/>, whose generic argument is <see cref="int"/>,
        /// representing the asynchronous command execution.
        /// <para>
        /// The result should be an exit code. Zero for success, or a non-zero value
        /// to indicate an erroneous execution.
        /// </para>
        /// </returns>
        protected abstract Task<int> ExecuteAsync(TOptions options);
    }
}