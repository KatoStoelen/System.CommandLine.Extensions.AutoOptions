# SystemCommandLineExtensions.AutoOptions

Extension of [System.CommandLine](https://github.com/dotnet/command-line-api) to automatically create command options.

[System.CommandLine](https://github.com/dotnet/command-line-api) is currently in beta. Hence, this library is as well.

## Intsall

```console
dotnet add package SystemCommandLineExtensions.AutoOptions --version 1.0.0-beta.*
```

## Usage

Instead of defining options using `new Option<T>(...)`, you can use a Plain Old CLR Object (POCO). The aliases, default values and descriptions of options are defined using attributes.

| Attribute | Purpose |
| --- | --- |
| `SystemCommandLineExtensions.AutoOptions.AliasAttribute` | Add one or more aliases of an option |
| `System.ComponentModel.DefaultValue` | Set the default value of an option |
| `System.ComponentModel.DescriptionAttribute` | Set the description of an option |
| `SystemCommandLineExtensions.AutoOptions.NotAnOptionAttribute` | Indicates that a property should not have a corresponding command line option. Useful for properties that returns the result of an expression. |
| `SystemCommandLineExtensions.AutoOptions.DefaultDirectoryAttribute` | Set the default value of an option of type `System.IO.DirectoryInfo`. Since attribute arguments need to be compile-time constant, it is not possible to use `[DefaultValue(new DirectoryInfo(...))]`. This attribute extends `System.ComponentModel.DefaultValue` and initializes a `DirectoryInfo` using a path string. |
| `SystemCommandLineExtensions.AutoOptions.DefaultFileAttribute` | Same as `DefaultDirectoryAttribute` but for `System.IO.FileInfo` |

### Example

Options class:

```csharp
using System.ComponentModel;
using System.IO;
using SystemCommandLineExtensions.AutoOptions;

public class MyOptions
{
    [Alias("-i")]
    [DefaultFile("input.txt")]
    [Description("The file to process")]
    public FileInfo InputFile { get; set; }

    [Alias("-o")]
    [Alias("/output")]
    [DefaultDirectory("result")]
    [Description("The output directory of the processed file")]
    public DirectoryInfo OutputDirectory { get; set; }

    [DefaultValue("utf-8")]
    [Description("The file encoding to use")]
    public string FileEncoding { get; set; }

    [Alias("-v")]
    [Description("Enable verbose output")]
    public bool Verbose { get; set; }

    [NotAnOption]
    public Encoding Encoding => Encoding.GetEncoding(FileEncoding);
}
```

Entry point (using the `AddOptions<TOptions>()` extension method of `Command`, to populate the `Option`s):

```csharp
private static int Main(string[] args)
{
    var rootCommand = new RootCommand("Processing of files");

    // Generate and add options defined in type MyOptions
    rootCommand.AddOptions<MyOptions>();

    rootCommand.Handler = CommandHandler.Create((MyOptions options) => { /* ... */ });

    return rootCommand.Invoke(args);
}
```

The setup above would yield the following command line interface:

```console
_APPNAME_:
  Processing of files

Usage:
  _APPNAME_ [options]

Options:
  -i, --input-file <input-file>                         The file to process [default: input.txt]
  -o, --output-directory, /output <output-directory>    The output directory of the processed file [default: result]
  --file-encoding <file-encoding>                       The file encoding to use [default: utf-8]
  -v, --verbose                                         Enable verbose output
  --version                                             Show version information
  -?, -h, --help                                        Show help and usage information
```

### Commands as separate classes

If you like to separate the commands into different classes, to keep the entry point nice and tidy, this library provides a couple of base classes to simplify the option setup.

| Base class | Purpose |
| --- | --- |
| `RootCommand<TOptions> : RootCommand` | Represents a root command with belonging options (`TOptions`) |
| `Command<TOption> : Command` | Represents a sub-command with belonging options (`TOptions`) |

Using this approach, the previous example would look like this:

```csharp
public class MyRootCommand : RootCommand<MyOptions>
{
    public MyRootCommand()
        : base("Processing of files", invokeMethodName: nameof(Invoke))
    {
    }

    private int Invoke(MyOptions options)
    {
        // ...
        return 0;
    }
}

private static int Main(string[] args)
{
    var rootCommand = new MyRootCommand();

    return rootCommand.Invoke(args);
}
```

Or, if you have any sub-commands as well, the entry point could look like this:

```csharp
private static int Main(string[] args)
{
    var rootCommand = new MyRootCommand
    {
        new MySubCommand1(),
        new MySubCommand2(),
        // ...
    };

    return rootCommand.Invoke(args);
}
```

Not exactly groundbreaking stuff, but allows you to focus more on the logic rather than the command line interface.

### Customization

By default, the default (long) name of options is generated using two hyphens (`--`) as prefix and kebab case as naming convention (lowercase and single hypen `-` as word delimiter). This can be customized using any of the overloads of `.AddOptions<TOptions>()`.

E.g., to use a different prefix and/or naming convention:

```csharp
command.AddOptions<MyOptions>(
    OptionPrefix.SingleHyphen,
    OptionNamingConvention.MatchPropertyName);
```

Or, to take full control of the option name generation:

```csharp
command.AddOptions<MyOptions>(
    property =>
    {
        var optionName = string.Empty;

        for (var i = 0; i < property.Name.Length; i++)
        {
            if (i % 2 == 0)
            {
                optionName += char.ToUpper(property.Name[i]);
            }
            else
            {
                optionName += char.ToLower(property.Name[i]);
            }
        }

        return optionName;
    })
```

If `null` or an empty string is returned from the supplied delegate, no default option name will be generated and only the aliases specified via attributes will be used.

**NB!** When taking full control of the option name generation, keep in mind that the option names needs to use a convention supported by [System.CommandLine](https://github.com/dotnet/command-line-api) for its model binding to work.

## Remarks

This extension adds a bit more reflection into the mix. If performance is of high priority, you might be better off without this extension.
