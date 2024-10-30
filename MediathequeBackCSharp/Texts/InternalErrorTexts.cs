namespace MediathequeBackCSharp.Texts;

/// <summary>
/// Contains the texts used into the app that NEVER WILL BE SENT TO THE FRONT CLIENT !!
/// </summary>
public class InternalErrorTexts
{
    /// <summary>
    /// Used into BnfApiSearchService while retrieving the BnfApiRepository from DI
    /// </summary>
    public const string ERROR_BNFAPI_REPO_RETRIEVAL = "Les données fournies par la BnF ne pourront pas être récupérées";

    /// <summary>
    /// Used into MySQLSearchService while retrieving the MySQLRepository from DI
    /// </summary>
    public const string ERROR_MYSQL_REPO_RETRIEVAL = "Les données fournies par la base MySQL ne pourront pas être récupérées";

    /// <summary>
    /// Used into StartUpDI for reporting the missing connection string
    /// </summary>
    public const string ERROR_MISSING_CONNEXION_STRING = "Les paramètres de connexion à la base de données {0} sont manquants";

    /// <summary>
    /// Used into StartUpOptions for reporting the missing front-end domains
    /// </summary>
    public const string ERROR_MISSING_CORS_POLICY_DOMAINS = "Some settings are missing for configuring CORS policy";

    /// <summary>
    /// Used into the Search Manager while retrieving the TextManager from DI
    /// </summary>
    public const string ERROR_TEXT_MANAGER_RETRIEVAL = "Les textes de l'application ne s'afficheront peut-être pas correctement";
}