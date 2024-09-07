using ApplicationCore.AbstractServices;
using AutoMapper;
using Infrastructure.BnfApi.Repositories;
using System.Resources;

namespace MediathequeBackCSharp.Services;

/// <summary>
/// Methods for preparing the data extracted from the Bnf API with the simple search
/// </summary>
public class BnfApiSimpleSearchService : BnfApiSearchService
{
    /// <summary>
    /// Constructor of the BnfApiSimpleSearchService class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    /// <param name="repo">Repository for collecting data</param>
    public BnfApiSimpleSearchService(IMapper mapper, ResourceManager textsManager, BnfApiSimpleSearchRepository repo) 
        : base(mapper, textsManager, repo)
    {
    }
}