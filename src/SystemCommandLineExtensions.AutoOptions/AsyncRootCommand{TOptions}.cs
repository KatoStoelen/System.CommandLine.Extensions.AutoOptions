using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// Represents a <see cref="RootCommand"/> with a belonging options class.
    /// </summary>
    /// <typeparam name="TOptions">The class containing the options of this command.</typeparam>
    public abstract class AsyncRootCommand<TOptions> : RootCommand where TOptions : class
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
        /// use <see cref="AsyncRootCommand{TOptions}.AsyncRootCommand(Action{Command}, string)"/>.
        /// </para>
        /// </remarks>
        /// <param name="description">An optional description of the command.</param>
        protected AsyncRootCommand(string description = "")
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
        protected AsyncRootCommand(Action<Command> configureOptions, string description = "")
            : base(description)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            configureOptions.Invoke(this);

            Handler = CommandHandler.Create((TOptions options) => InvokeAsync(options));
        }

        /// <summary>
        /// Invokes the command asynchronously.
        /// </summary>
        /// <param name="options">The options of the command.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/>, whose generic argument is <see cref="int"/>,
        /// representing the asynchronous command invocation.
        /// <para>
        /// The result of the task is an exit code.
        /// </para>
        /// </returns>
        protected abstract Task<int> InvokeAsync(TOptions options);
    }
}