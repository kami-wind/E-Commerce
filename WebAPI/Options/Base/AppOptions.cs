namespace WebApi.Options.Base
{
    /// <summary>
    ///     The base interface for all options in the application.
    /// </summary>
    public abstract class AppOptions
    {
        /// <summary>
        ///     Bind the value reading from the appsettings using the input <paramref name="configuration"/>
        /// </summary>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration"/> that used to read the value from appsettings
        ///     and process binding.
        /// </param>
        public abstract void Bind(IConfiguration configuration);
    }
}
