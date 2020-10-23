using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// Represents a <see cref="RootCommand"/> with a belonging options class.
    /// </summary>
    /// <typeparam name="TOptions">The class containing the options of this command.</typeparam>
    public abstract class RootCommand<TOptions> : RootCommand where TOptions : class
    {
        /// <summary>
        /// Initializes a root command with a belonging options class.
        /// </summary>
        /// <remarks>
        /// This constructor uses the default configuration for options
        /// (<see cref="OptionPrefix.TwoHyphens"/> and
        /// <see cref="OptionNamingConvention.KebabCase"/>).
        /// <para>
        /// To customize the options configuration (prefix, naming convention etc.)
        /// use <see cref="RootCommand{TOptions}.RootCommand(Action{Command}, string)"/>.
        /// </para>
        /// </remarks>
        /// <param name="description">An optional description of the command.</param>
        protected RootCommand(string description = "")
            : this(command => command.AddOptions<TOptions>(), description)
        {
        }

        /// <summary>
        /// Initializes a root command with a belonging options class.
        /// </summary>
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
        protected RootCommand(Action<Command> configureOptions, string description = "")
            : base(description)
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