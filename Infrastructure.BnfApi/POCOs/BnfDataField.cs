﻿namespace Infrastructure.BnfApi.POCOs;

/// <summary>
/// Represents a mxc:datafield node retrieved from the Bnf
/// </summary>
public class BnfDataField
{
    /// <summary>
    /// Value of the attribute "tag"
    /// </summary>
    public required string Tag { get; set; }

    /// <summary>
    /// Value of the attribute "ind1"
    /// If " ", all compared values will be equal
    /// </summary>
    public string Ind1 { get; set; } = BnfGlobalConsts.IND_DEFAULT_VALUE;

    /// <summary>
    /// Value of the attribute "ind2"
    /// If " ", all compared values will be equal
    /// </summary>
    public string Ind2 { get; set; } = BnfGlobalConsts.IND_DEFAULT_VALUE;

    /// <summary>
    /// Subnodes of type mxc:subfield
    /// </summary>
    public IEnumerable<BnfSubField> Subfields { get; set; } = [];

    /// <summary>
    /// Compares the properties of the current BnfDataField 
    /// with these of another one
    /// </summary>
    /// <param name="otherDf">Compared BnfDataField</param>
    /// <returns>True if Tag, Ind1 and Ind2 of the both BnfDataField are equals</returns>
    public bool Equals(BnfDataField otherDf)
    {

        if (otherDf == null)
        {
            return false;
        }

        return this.Tag == otherDf.Tag
            && (this.Ind1 == otherDf.Ind1 || otherDf.Ind1 == BnfGlobalConsts.IND_DEFAULT_VALUE)
            && (this.Ind2 == otherDf.Ind2 || otherDf.Ind2 == BnfGlobalConsts.IND_DEFAULT_VALUE);
    }

    /// <summary>
    /// Extracts a value from the subfield list according to a given code
    /// </summary>
    /// <param name="code">Code (letter)</param>
    /// <returns>Returns the value of a subfield node</returns>
    public string? ExtractValueFromSubfield(string code)
    {
        if (Subfields == null)
        {
            return null;
        }

        return Subfields.FirstOrDefault(sf => sf.Code == code)?.Value;
    }
}