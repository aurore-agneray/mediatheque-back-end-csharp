﻿using Infrastructure.BnfApi.POCOs;

namespace Infrastructure.BnfApi.Constants;

/// <summary>
/// Contains tags and codes attributes for which the markups have to be read into the XML content
/// </summary>
public class BnfTagsAndCodesConsts
{
    /// <summary>
    /// Joins tags and codes used to extract books data from
    /// the XML BnF API result
    /// </summary>
    public static readonly Dictionary<string, Dictionary<BnfDataField, string[]>> TAGS_AND_CODES = new() {
    {
        BnfPropertiesConsts.AUTHOR,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_200_1_EMPTY, ["f"] },
            { BnfDefaultDatafieldsConsts._DATA_FIELD_700, ["a", "b"] },
            { BnfDefaultDatafieldsConsts._DATA_FIELD_710, ["a"] }
        }
    },
    {
        BnfPropertiesConsts.ISBN,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_010, ["a"] }
        }
    },
    {
        BnfPropertiesConsts.PUBLICATION_DATE_BNF,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_210, ["d"] },
            { BnfDefaultDatafieldsConsts._DATA_FIELD_214_EMPTY_0, ["d"] }
        }
    },
    {
        BnfPropertiesConsts.PUBLISHER,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_210, ["c"] },
            { BnfDefaultDatafieldsConsts._DATA_FIELD_214_EMPTY_0, ["c"] }
        }
    },
    {
        BnfPropertiesConsts.SERIES_NAME,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_225_1_9, ["a"] },
            { BnfDefaultDatafieldsConsts._DATA_FIELD_461, ["t"] }
        }
    },
    {
        BnfPropertiesConsts.SUBTITLE,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_200_1_EMPTY, ["h", "i"] }
        }
    },
    {
        BnfPropertiesConsts.SUMMARY,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_330, ["a"] }
        }
    },
    {
        BnfPropertiesConsts.TITLE,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_200_1_EMPTY, ["a", "e"] }
        }
    },
    {
        BnfPropertiesConsts.VOLUME,
        new() {
            { BnfDefaultDatafieldsConsts._DATA_FIELD_225_1_9, ["v"] },
            { BnfDefaultDatafieldsConsts._DATA_FIELD_461, ["v"] }
        }
    }
};

    /// <summary>
    /// Tags that are searched into the XML content
    /// </summary>
    public static readonly List<string> NEEDED_TAGS = TAGS_AND_CODES.SelectMany(
        item => item.Value.Select(tagAndCodes => tagAndCodes.Key.Tag)
    ).Distinct().ToList();
}
