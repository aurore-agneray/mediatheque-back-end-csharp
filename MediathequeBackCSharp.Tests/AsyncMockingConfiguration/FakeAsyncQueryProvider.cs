using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MediathequeBackCSharp.Tests.AsyncMockingConfiguration;

/// <summary>
/// Source : https://github.com/stephenfuqua/safnet.libraries/blob/develop/TestHelper.AsyncDbSet/src/FakeAsyncQueryProvider.cs
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal class FakeAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal FakeAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new FakeAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new FakeAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(expression));
    }

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
    {
        return new FakeAsyncEnumerable<TResult>(Execute<TResult>(expression) as IEnumerable<TResult>);
    }

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute<TResult>(expression));
    }

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Execute<TResult>(expression);
    }
}