using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using SystemCommandLineExtensions.AutoOptions.Internal;

namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// Represents a <see cref="RootCommand"/> with a belonging options class.
    /// </summary>
    /// <typeparam name="TOptions">The class containing the options of this command.</typeparam>
    public class RootCommand<TOptions> : RootCommand where TOptions : class
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
        /// use <see cref="RootCommand{TOptions}.RootCommand(Action{Command}, string, string?)"/>.
        /// </para>
        /// </remarks>
        /// <param name="description">An optional description of the command.</param>
        /// <param name="invokeMethodName">
        /// The name of the method to call when invoking the command.
        /// <para>
        /// If not specified, you would have to set the <see cref="Command.Handler"/>
        /// property yourself (if the command should have a handler, that is).
        /// </para>
        /// </param>
        protected RootCommand(string description = "", string? invokeMethodName = null)
            : this(command => command.AddOptions<TOptions>(), description, invokeMethodName)
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
        /// command =&gt; command.AddOptions&lt; MyOptions &gt;(OptionPrefix.SingleHyphen, OptionNamingConvention.MatchPropertyName)
        /// </para>
        /// </param>
        /// <param name="description">An optional description of the command.</param>
        /// <param name="invokeMethodName">
        /// The name of the method to call when invoking the command.
        /// <para>
        /// If not specified, you would have to set the <see cref="Command.Handler"/>
        /// property yourself (if the command should have a handler, that is).
        /// </para>
        /// </param>
        protected RootCommand(
                Action<Command> configureOptions,
                string description = "",
                string? invokeMethodName = null)
            : base(description)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            configureOptions.Invoke(this);

            if (invokeMethodName != null)
            {
                Handler = CommandHandler.Create(GetType().GetDeclaredMethod(invokeMethodName), this);
            }
        }
    }
}