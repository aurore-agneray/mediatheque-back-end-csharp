namespace mediatheque_back_csharp.Classes;

/// <summary>
/// Represents a mxc:subfield node retrieved from the Bnf
/// </summary>
public class BnfSubField {

    /// <summary>
    /// Value of the attribute "code"
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Value contained into the subfield
    /// </summary>
    public required string Value { get; set; }
}