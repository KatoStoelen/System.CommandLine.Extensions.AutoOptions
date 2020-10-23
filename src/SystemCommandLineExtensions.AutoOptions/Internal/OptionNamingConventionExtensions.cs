using System;

namespace SystemCommandLineExtensions.AutoOptions.Internal
{
    internal static class OptionNamingConventionExtensions
    {
        public static string GetWordDelimiter(this OptionNamingConvention namingConvention) =>
            namingConvention switch
            {
                var conv when conv.HasFlag(OptionNamingConvention.KebabCase) => "-",
                _ => string.Empty
            };

        public static Func<string, string> GetCasingFunc(this OptionNamingConvention namingConvention) =>
            namingConvention switch
            {
                var conv when conv.HasFlag(OptionNamingConvention.UpperCase) =>
                    parameterName => parameterName.ToUpperInvariant(),

                var conv when conv.HasFlag(OptionNamingConvention.LowerCase) =>
                    parameterName => parameterName.ToLowerInvariant(),

                _ => parameterName => parameterName
            };
    }
}
