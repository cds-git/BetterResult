namespace BetterResult;

public static partial class Result
{
    /// <summary>
    /// Applies <paramref name="selector"/> to every element and collects the values, or stops at the
    /// first failure and returns its error. Like LINQ's <c>Select</c>, but for functions that return a
    /// result.
    /// </summary>
    public static Result<IReadOnlyList<U>> Traverse<T, U>(
        IEnumerable<T> source,
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
        IEnumerable<T> source,
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
        Task<IEnumerable<T>> source,
        Func<T, Result<U>> selector) =>
            Traverse(await source.ConfigureAwait(false), selector);

    /// <inheritdoc cref="Traverse{T, U}(IEnumerable{T}, Func{T, Result{U}})"/>
    public static async Task<Result<IReadOnlyList<U>>> TraverseAsync<T, U>(
        Task<IEnumerable<T>> source,
        Func<T, Task<Result<U>>> selector) =>
            await TraverseAsync(await source.ConfigureAwait(false), selector).ConfigureAwait(false);
}
