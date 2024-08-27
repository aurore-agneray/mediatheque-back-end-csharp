using ApplicationCore.Interfaces;

namespace Infrastructure.MySQL;

/// <summary>
/// Object used to retrieve the settings from the appsettings.json file
/// </summary>
public class MySQLDatabaseSettings : IDatabaseSettings
{
    /// <summary>
    /// Connection string used for connecting to the database, that depends on the execution environment
    /// </summary>
    public string? DbConnectionString => "Server=localhost; User ID=root; Password=5Q9LGwGdYX; Database=library";

    /// <summary>
    /// Domain(s) of the front-end that will call the API.
    /// If several domains, separate them with ;
    /// </summary>
    public string? FrontEndDomains => "http://localhost:5173;http://localhost:8081";
}