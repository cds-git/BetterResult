namespace BetterResult;

/// <summary>
/// Maps a collection through a function that returns a result. All or nothing.
/// </summary>
public static class TraverseExtensions
{
    /// <summary>
    /// Applies <paramref name="selector"/> to every element and collects the values, or stops at the
    /// first failure and returns its error. Like LINQ's <c>Select</c>, but for functions that return a
    /// result.
    /// </summary>
    public static Result<IReadOnlyList<U>> Traverse<T, U>(
        this IEnumerable<T> source,
        Func<T, Result<U>> selector)
    {
        var list = new List<U>();
        foreach (var item in source)
        {
            var result = selector(item);
            if (result.IsFailure)
                return result.Error;
            list.Add(result.Value);
        }
        return list.AsReadOnly();
    }

    /// <inheritdoc cref="Traverse{T, U}(IEnumerable{T}, Func{T, Result{U}})"/>
    public static async Task<Result<IReadOnlyList<U>>> TraverseAsync<T, U>(
        this IEnumerable<T> source,
        Func<T, Task<Result<U>>> selector)
    {
        var list = new List<U>();
        foreach (var item in source)
        {
            var result = await selector(item).ConfigureAwait(false);
            if (result.IsFailure)
                return result.Error;
            list.Add(result.Value);
        }
        return list.AsReadOnly();
    }

    /// <inheritdoc cref="Traverse{T, U}(IEnumerable{T}, Func{T, Result{U}})"/>
    public static async Task<Result<IReadOnlyList<U>>> Traverse<T, U>(
        this Task<IEnumerable<T>> source,
        Func<T, Result<U>> selector) =>
            (await source.ConfigureAwait(false)).Traverse(selector);

    /// <inheritdoc cref="Traverse{T, U}(IEnumerable{T}, Func{T, Result{U}})"/>
    public static async Task<Result<IReadOnlyList<U>>> TraverseAsync<T, U>(
        this Task<IEnumerable<T>> source,
        Func<T, Task<Result<U>>> selector) =>
            await (await source.ConfigureAwait(false)).TraverseAsync(selector).ConfigureAwait(false);
}
