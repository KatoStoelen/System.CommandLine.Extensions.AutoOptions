namespace SystemCommandLineExtensions.AutoOptions
{
    /// <summary>
    /// Option prefix types.
    /// </summary>
    public enum OptionPrefix
    {
        /// <summary>
        /// Use two hyphens (--) as prefix.
        /// </summary>
        TwoHyphens = 0,

        /// <summary>
        /// Use a single hyphen (-) as prefix.
        /// </summary>
        SingleHyphen = 1,

        /// <summary>
        /// Use a forward slash (/) as prefix.
        /// </summary>
        ForwardSlash = 2
    }
}