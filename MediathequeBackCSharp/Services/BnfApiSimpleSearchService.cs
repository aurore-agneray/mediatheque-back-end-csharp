using AutoMapper;
using Infrastructure.BnfApi.Repositories;
using MediathequeBackCSharp.Services.Abstracts;
using System.Resources;

namespace MediathequeBackCSharp.Services;

/// <summary>
/// Methods for preparing the data extracted from the Bnf API with the simple search
/// </summary>
/// <remarks>
/// Constructor of the BnfApiSimpleSearchService class
/// </remarks>
/// <param name="mapper">Given AutoMapper</param>
/// <param name="textsManager">Texts manager</param>
/// <param name="repo">Repository for collecting data</param>
public class BnfApiSimpleSearchService(IMapper mapper, ResourceManager textsManager, BnfApiSimpleSearchRepository repo) 
    : BnfApiSearchService(mapper, textsManager, repo)
{
}