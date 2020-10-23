using System;

namespace SystemCommandLineExtensions.AutoOptions.Internal
{
    internal static class OptionPrefixExtensions
    {
        public static string AsString(this OptionPrefix prefix) =>
            prefix switch
            {
                OptionPrefix.TwoHyphens => "--",
                OptionPrefix.SingleHyphen => "-",
                OptionPrefix.ForwardSlash => "/",

                _ => throw new ArgumentOutOfRangeException(
                    nameof(prefix), prefix, $"Unknown option prefix: {prefix}")
            };
    }
}
