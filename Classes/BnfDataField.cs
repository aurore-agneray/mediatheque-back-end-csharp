using System.Xml.Linq;

namespace mediatheque_back_csharp.Classes;

/// <summary>
/// Represents a mxc:datafield node retrieved from the Bnf
/// </summary>
public class BnfDataField {

    /// <summary>
    /// Value of the attribute "tag"
    /// </summary>
    public required string Tag { get; set; }

    /// <summary>
    /// Value of the attribute "ind1"
    /// </summary>
    public required string Ind1 { get; set; }

    /// <summary>
    /// Value of the attribute "ind2"
    /// </summary>
    public required string Ind2 { get; set; }

    /// <summary>
    /// Subnodes of type mxc:subfield
    /// </summary>
    public IEnumerable<XElement> Subfields { get; set; }
}