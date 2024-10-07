using Infrastructure.BnfApi.POCOs;

namespace Infrastructure.BnfApi.Constants;

/// <summary>
/// Reference Datafields used for extracting data from the XML structure
/// </summary>
public readonly struct BnfDefaultDatafieldsConsts
{
    /// <summary>
    /// Default Datafield with Tag = "010", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_010 = new() { Tag = "010" };

    /// <summary>
    /// Default Datafield with Tag = "200", Ind1 = "1" and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_200_1_EMPTY = new() { Tag = "200", Ind1 = "1" };

    /// <summary>
    /// Default Datafield with Tag = "210", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_210 = new() { Tag = "210" };

    /// <summary>
    /// Default Datafield with Tag = "214", Ind1 = " " and Ind2 = "0"
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_214_EMPTY_0 = new() { Tag = "214", Ind2 = "0" };

    /// <summary>
    /// Default Datafield with Tag = "214", Ind1 = "1" and Ind2 = "9"
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_225_1_9 = new() { Tag = "214", Ind1 = "1", Ind2 = "9" };

    /// <summary>
    /// Default Datafield with Tag = "330", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_330 = new() { Tag = "330" };

    /// <summary>
    /// Default Datafield with Tag = "461", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_461 = new() { Tag = "461" };

    /// <summary>
    /// Default Datafield with Tag = "700", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_700 = new() { Tag = "700" };

    /// <summary>
    /// Default Datafield with Tag = "710", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_710 = new() { Tag = "710" };
}