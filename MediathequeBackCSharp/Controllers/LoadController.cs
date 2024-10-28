using MediathequeBackCSharp.Classes;
using MediathequeBackCSharp.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MediathequeBackCSharp.Controllers;

/// <summary>
/// API requests for the loading of the search front-end application
/// </summary>
/// <remarks>
/// Constructor of the LoadController class
/// </remarks>
/// <param name="manager">Manager dedicated to LoadController</param>
[ApiController]
[Route("/load")]
public class LoadController(LoadManager manager) : ControllerBase
{
    /// <summary>
    /// Prepares the requests for the LoadController
    /// </summary>
    protected readonly LoadManager _manager = manager;

    /// <summary>
    /// Get the data used for initializing the front-end application :
    /// genres, publishers and formats lists
    /// </summary>
    /// <returns>Returns a DTO with several lists</returns>
    /// <response code="500">If an error occurred into the process, with an explicit information message</response>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        ErrorObject? propertiesError;
        if ((propertiesError = _manager.CheckProperties()) != null)
        {
            return propertiesError;
        }

        try
        {
            var results = await _manager.Get();
            return Ok(results);
        }
        catch (Exception ex)
        {
            return new ErrorObject(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}