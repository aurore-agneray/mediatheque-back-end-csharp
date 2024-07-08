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
    }
}