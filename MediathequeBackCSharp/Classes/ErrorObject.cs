using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Classes;

/// <summary>
/// My own object returned by HTTP methods in case of errors
/// </summary>
public class ErrorObject : ObjectResult
{
    /// <summary>
    /// Main constructor which calls the ObjectResult constructor
    /// </summary>
    /// <param name="statusCode">HttpStatusCode enum</param>
    /// <param name="message">Error message for the front-end</param>
    public ErrorObject(HttpStatusCode statusCode, string message) 
        : base(new { Message = message })
    {
        StatusCode = (int)statusCode;
    }
}