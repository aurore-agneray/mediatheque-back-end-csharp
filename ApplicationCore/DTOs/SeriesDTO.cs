﻿using ApplicationCore.Interfaces.Entities;

namespace ApplicationCore.Dtos;

/// <summary>
/// DTO for the Series
/// </summary>
public class SeriesDTO : INamed
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Custom code
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Number of planned volumes
    /// </summary>
    public int? PlannedVolumes { get; set; }

    /// <summary>
    /// Publisher ID
    /// </summary>
    public int PublisherId { get; set; }
}