namespace MediathequeBackCSharp.Texts;

/// <summary>
/// Contains the texts used into the app that NEVER WILL BE SENT TO THE FRONT CLIENT !!
/// </summary>
public class InternalErrorTexts
{
    /// <summary>
    /// Used into StartUpDI for reporting the missing connection string
    /// </summary>
    public const string ERROR_MISSING_CONNEXION_STRING = "Please insert the Connection String into the database's settings !";
}