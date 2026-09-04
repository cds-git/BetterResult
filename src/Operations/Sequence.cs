namespace BetterResult;

/// <summary>
/// Turns a collection of results into a single result. All or nothing.
/// </summary>
public static class SequenceExtensions
{
    /// <summary>
    /// Collects every value, or stops at the first failure and returns its error.
    /// Use <c>Partition</c> instead when you want both sides rather than short-circuiting.
    /// </summary>
    public static Result<IReadOnlyList<T>> Sequence<T>(this IEnumerable<Result<T>> source)
    {
        var list = new List<T>();
        foreach (var result in source)
        {
            if (result.IsFailure)
                return result.Error;
            list.Add(result.Value);
        }
        return list.AsReadOnly();
    }

    /// <inheritdoc cref="Sequence{T}(IEnumerable{Result{T}})"/>
    public static async Task<Result<IReadOnlyList<T>>> SequenceAsync<T>(
        this IEnumerable<Task<Result<T>>> source)
    {
        var list = new List<T>();
        foreach (var resultTask in source)
        {
            var result = await resultTask.ConfigureAwait(false);
            if (result.IsFailure)
                return result.Error;
            list.Add(result.Value);
        }
        return list.AsReadOnly();
    }

    /// <inheritdoc cref="Sequence{T}(IEnumerable{Result{T}})"/>
    public static async Task<Result<IReadOnlyList<T>>> Sequence<T>(
        this Task<IEnumerable<Result<T>>> source) =>
            (await source.ConfigureAwait(false)).Sequence();

    /// <inheritdoc cref="Sequence{T}(IEnumerable{Result{T}})"/>
    public static async Task<Result<IReadOnlyList<T>>> SequenceAsync<T>(
        this Task<IEnumerable<Task<Result<T>>>> source) =>
            await (await source.ConfigureAwait(false)).SequenceAsync().ConfigureAwait(false);
}
