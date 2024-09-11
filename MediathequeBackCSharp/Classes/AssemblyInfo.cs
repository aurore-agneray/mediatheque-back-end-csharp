using System.Diagnostics;
using System.Reflection;

namespace MediathequeBackCSharp.Classes;

/// <summary>
/// For getting information about the assembly
/// </summary>
public static class AssemblyInfo
{
    /// <summary>
    /// Used when the version number is not available
    /// </summary>
    public const string UNKNOWN_VERSION = "Unknown version";

    /// <summary>
    /// Retrieves the version number of the Web project assembly
    /// </summary>
    /// <returns>A string with the version number into the format x.x.x.x</returns>
    public static string GetVersionNumber()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
        return !string.IsNullOrEmpty(fileVersionInfo.FileVersion) ? fileVersionInfo.FileVersion : UNKNOWN_VERSION;
    }
}