using System.Linq;

namespace SystemCommandLineExtensions.AutoOptions.Internal
{
    internal static class StringExtensions
    {
        public static string ToOptionName(
            this string optionPropertyName,
            OptionPrefix prefix,
            OptionNamingConvention namingConvention)
        {
            var prefixString = prefix.AsString();
            var wordDelimiter = namingConvention.GetWordDelimiter();
            var setCasing = namingConvention.GetCasingFunc();

            var shouldAddWordDelimiter = !string.IsNullOrEmpty(wordDelimiter);

            return prefixString + (
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
    }
}