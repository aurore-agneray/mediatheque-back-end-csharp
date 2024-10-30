using System.Linq.Expressions;

namespace MediathequeBackCSharp.Tests.AsyncMockingConfiguration;

/// <summary>
/// Source : https://github.com/stephenfuqua/safnet.libraries/blob/develop/TestHelper.AsyncDbSet/src/FakeAsyncEnumerable.cs
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal class FakeAsyncEnumerable<TEntity> : EnumerableQuery<TEntity>, IAsyncEnumerable<TEntity>, IQueryable<TEntity>
{
    public FakeAsyncEnumerable(IEnumerable<TEntity> enumerable)
        : base(enumerable)
    {
    }

    public FakeAsyncEnumerable(Expression expression)
        : base(expression)
    {
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator()
    {
        return new FakeAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
    }
    public IAsyncEnumerator<TEntity> GetEnumerator()
    {
        return GetAsyncEnumerator();
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new FakeAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<TEntity>(this);
}