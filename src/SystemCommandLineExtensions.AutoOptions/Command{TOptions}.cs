using System;
using System.CommandLine;
using System.CommandLine.Invocation;

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
        /// <remarks>
        /// This constructor uses the default configuration for options
        /// (<see cref="OptionPrefix.TwoHyphens"/> and
        /// <see cref="OptionNamingConvention.KebabCase"/>).
        /// <para>
        /// To customize the options configuration (prefix, naming convention etc.)
        /// use <see cref="Command{TOptions}.Command(string, Action{Command}, string?)"/>.
        /// </para>
        /// </remarks>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">An optional description of the command.</param>
        protected Command(string name, string? description = null)
            : this(name, command => command.AddOptions<TOptions>(), description)
        {
        }

        /// <summary>
        /// Initializes a command with a belonging options class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="configureOptions">
        /// A delegate, accepting this <see cref="Command"/>, configuring its options.
        /// <para>
        /// E.g:
        /// </para>
        /// <para>
        /// command =&gt; command.AddOptions&lt; TOptions &gt;(OptionPrefix.SingleHyphen, OptionNamingConvention.MatchPropertyName)
        /// </para>
        /// </param>
        /// <param name="description">An optional description of the command.</param>
        protected Command(string name, Action<Command> configureOptions, string? description = null)
            : base(name, description)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            configureOptions.Invoke(this);

            Handler = CommandHandler.Create((TOptions options) => Invoke(options));
        }

        /// <summary>
        /// Invokes the command.
        /// </summary>
        /// <param name="options">The options of the command.</param>
        /// <returns>An exit code.</returns>
        protected abstract int Invoke(TOptions options);
    }
}