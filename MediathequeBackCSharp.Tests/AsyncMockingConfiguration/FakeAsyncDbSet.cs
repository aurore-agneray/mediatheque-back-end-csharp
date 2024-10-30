using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace MediathequeBackCSharp.Tests.AsyncMockingConfiguration;

/// <summary>
/// Source : https://github.com/stephenfuqua/safnet.libraries/blob/develop/TestHelper.AsyncDbSet/src/FakeAsyncDbSet.cs
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal class FakeAsyncDbSet<TEntity> : DbSet<TEntity>, IQueryable, IAsyncEnumerable<TEntity>
        where TEntity : class
{
    public IList<TEntity> List { get; } = new List<TEntity>();
    public IList<TEntity> Added { get; } = new List<TEntity>();
    public IList<TEntity> Updated { get; } = new List<TEntity>();
    public IList<TEntity> Deleted { get; } = new List<TEntity>();

    Type IQueryable.ElementType => typeof(TEntity);

    Expression IQueryable.Expression => List.AsQueryable().Expression;

    IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<TEntity>(List.AsQueryable().Provider);

    public override IEntityType EntityType => throw new NotImplementedException();

    public override EntityEntry<TEntity> Add(TEntity entity)
    {
        List.Add(entity);
        Added.Add(entity);

        // Returning null here is only safe because we never want to
        // do anything with an EntityEntry in this application.
        return null!;
    }

    public override EntityEntry<TEntity> Update(TEntity entity)
    {
        List.Add(entity);
        Updated.Add(entity);

        // Returning null here is only safe because we never want to
        // do anything with an EntityEntry in this application.
        return null!;
    }

    public override EntityEntry<TEntity> Remove(TEntity entity)
    {
        List.Add(entity);
        Deleted.Add(entity);

        // Returning null here is only safe because we never want to
        // do anything with an EntityEntry in this application.
        return null!;
    }
}