using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Databases;

/// <summary>
/// Object used to configure a database access
/// </summary>
public class IDatabaseSettings
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