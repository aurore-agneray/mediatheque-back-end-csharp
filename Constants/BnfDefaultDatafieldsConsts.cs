using mediatheque_back_csharp.Classes;

namespace mediatheque_back_csharp.Constants;

/// <summary>
/// Reference Datafields used for extracting data from the XML structure
/// </summary>
public struct BnfDefaultDatafieldsConsts {
    
    /// <summary>
    /// Default Datafield with Tag = "010", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_010 = new BnfDataField() { Tag = "010" };

    /// <summary>
    /// Default Datafield with Tag = "200", Ind1 = "1" and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_200_1_EMPTY = new BnfDataField() { Tag = "200", Ind1 = "1" };

    /// <summary>
    /// Default Datafield with Tag = "210", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_210 = new BnfDataField() { Tag = "210" };

    /// <summary>
    /// Default Datafield with Tag = "214", Ind1 = " " and Ind2 = "0"
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_214_EMPTY_0 = new BnfDataField() { Tag = "214", Ind2 = "0" };

    /// <summary>
    /// Default Datafield with Tag = "214", Ind1 = "1" and Ind2 = "9"
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_225_1_9 = new BnfDataField() { Tag = "214", Ind1 = "1", Ind2 = "9" };

    /// <summary>
    /// Default Datafield with Tag = "330", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_330 = new BnfDataField() { Tag = "330" };

    /// <summary>
    /// Default Datafield with Tag = "461", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_461 = new BnfDataField() { Tag = "461" };

    /// <summary>
    /// Default Datafield with Tag = "700", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_700 = new BnfDataField() { Tag = "700" };

    /// <summary>
    /// Default Datafield with Tag = "710", Ind1 = " " and Ind2 = " "
    /// </summary>
    public static readonly BnfDataField _DATA_FIELD_710 = new BnfDataField() { Tag = "710" };
}