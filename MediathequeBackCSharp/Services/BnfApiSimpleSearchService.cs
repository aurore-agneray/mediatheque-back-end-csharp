using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using AutoMapper;
using System.Resources;

namespace MediathequeBackCSharp.Services;

/// <summary>
/// Methods for preparing the data extracted from the Bnf API with the simple search
/// </summary>
public class BnfApiSimpleSearchService : SearchService
{
    /// <summary>
    /// Constructor of the BnfApiSimpleSearchService class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    /// <param name="repo">Repository for collecting data</param>
    public BnfApiSimpleSearchService(IMapper mapper, ResourceManager textsManager, IXMLRepository repo) 
        : base(mapper, textsManager, SourceTypeEnum.XML, null, repo)
    {
    }
}