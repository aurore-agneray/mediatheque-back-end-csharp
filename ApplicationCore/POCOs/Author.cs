﻿using ApplicationCore.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Pocos;

/// <summary>
/// POCO for the Authors
/// </summary>
[Table("Author")]
public class Author : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// Custom code
    /// </summary>
    [Column("auth_code")]
    public String Code { get; set; } = null!;

    /// <summary>
    /// First name
    /// </summary>
    [Column("first_name")]
    public String? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    [Column("last_name")]
    public String? LastName { get; set; }

    /// <summary>
    /// First name + last name (generated into the database)
    /// </summary>
    [Column("complete_name")]
    public String? CompleteName { get; set; }
}