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
    public string Ind1 { get; set; } = " ";

    /// <summary>
    /// Value of the attribute "ind2"
    /// </summary>
    public string Ind2 { get; set; } = " ";

    /// <summary>
    /// Subnodes of type mxc:subfield
    /// </summary>
    public IEnumerable<BnfSubField> Subfields { get; set; }

    /// <summary>
    /// Compares the properties of the current BnfDataField 
    /// with these of another one
    /// </summary>
    /// <param name="otherDf">Compared BnfDataField</param>
    /// <returns>True if Tag, Ind1 and Ind2 of the both BnfDataField are equals</returns>
    public bool Equals (BnfDataField otherDf) {

        if (otherDf == null) {
            return false;
        }

        return this.Tag == otherDf.Tag
            && this.Ind1 == otherDf.Ind1
            && this.Ind2 == otherDf.Ind2;
    }

    /// <summary>
    /// Extracts a value from the subfield list according to a given code
    /// </summary>
    /// <param name="code">Code (letter)</param>
    /// <returns>Returns the value of a subfield node</returns>
    public string? ExtractValueFromSubfield(string code) {
        if (Subfields == null) {
            return null;
        }

        return Subfields.FirstOrDefault(sf => sf.Code == code)?.Value;
    }
}