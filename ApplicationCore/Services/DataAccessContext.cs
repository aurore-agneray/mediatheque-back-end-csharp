using ApplicationCore.AbstractClasses;
using ApplicationCore.Interfaces.Databases;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services;

public class DataAccessContext
{
    public new bool WithDbContext { get; set; }

    public IMediathequeDbContextFields? DbContext { get; set; }

    public DataAccessContext(IMediathequeDbContextFields dbContext)
    {
        DbContext = dbContext;
    }
}