namespace MediathequeBackCSharp.Tests.AsyncMockingConfiguration;

#pragma warning disable CA1063 // Implement IDisposable Correctly

/// <summary>
/// https://github.com/stephenfuqua/safnet.libraries/blob/develop/TestHelper.AsyncDbSet/src/FakeAsyncEnumerator.cs
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class FakeAsyncEnumerator<TEntity> : IAsyncEnumerator<TEntity>
#pragma warning restore CA1063 // Implement IDisposable Correctly
{
    private readonly IEnumerator<TEntity> _inner;

    public FakeAsyncEnumerator(IEnumerator<TEntity> inner)
    {
        _inner = inner;
    }

#pragma warning disable CA1063 // Implement IDisposable Correctly
    public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        _inner.Dispose();
    }

    public Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        return Task.FromResult(_inner.MoveNext());
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        return await Task.FromResult(_inner.MoveNext());
    }

    public async ValueTask DisposeAsync()
    {
        _inner.Dispose();
    }

    public TEntity Current => _inner.Current;
}