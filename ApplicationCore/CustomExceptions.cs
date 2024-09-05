using ApplicationCore.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore;

public static class CustomExceptions
{
    /// <summary>
    /// Throws an exception with a message indicating that criteria are missing
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void ThrowExceptionForMissingCriteria()
    {
        //throw new Exception(TextsManager.GetString(TextsKeys.ERROR_MISSING_CRITERIA) + " " + _searchType);
    }
}
