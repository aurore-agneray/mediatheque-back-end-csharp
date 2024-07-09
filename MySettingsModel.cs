namespace mediatheque_back_csharp
{
    /// <summary>
    /// Object used to retrieve the settings from the appsettings.json file
    /// </summary>
    public class MySettingsModel
    {
        /// <summary>
        /// Connection string used for connecting to the database, that depends on the execution environment
        /// </summary>
        public string? DbConnectionString { get; set; }

        /// <summary>
        /// Domain(s) of the front-end that will call the API.
        /// If several domains, separate them with ;
        /// </summary>
        public string? FrontEndDomains { get; set; }
    }
}