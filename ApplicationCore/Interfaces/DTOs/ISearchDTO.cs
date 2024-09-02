namespace ApplicationCore.Interfaces.DTOs;

/// <summary>
/// General properties transmitted by all types of search
/// </summary>
public interface ISearchDTO
{
    /// <summary>
    /// Indicates if the app need to connect to the BnF API or not
    /// </summary>
    public bool UseBnfApi { get; set; }

    /// <summary>
    /// Number of notices returned by the BnF API
    /// </summary>
    public int BnfNoticesQuantity { get; set; }
}