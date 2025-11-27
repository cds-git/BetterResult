namespace BetterResult;

public static partial class Result
{
    /// <summary>
    /// Transforms each element in the collection using the provided selector function and collects the results.
    /// Short-circuits on the first failure, returning that error without processing remaining items.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <typeparam name="U">The type of values in the resulting collection.</typeparam>
    /// <param name="source">The collection to traverse.</param>
    /// <param name="selector">The function to apply to each element.</param>
    /// <returns>A successful Result containing all transformed values if all succeeded, or the first error encountered.</returns>
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
        return list;
    }

    /// <summary>
    /// Asynchronously transforms each element in the collection using the provided selector function and collects the results.
    /// Short-circuits on the first failure, returning that error without processing remaining items.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <typeparam name="U">The type of values in the resulting collection.</typeparam>
    /// <param name="source">The collection to traverse.</param>
    /// <param name="selector">The asynchronous function to apply to each element.</param>
    /// <returns>A task containing a successful Result with all transformed values if all succeeded, or the first error encountered.</returns>
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
        return list;
    }

    /// <summary>
    /// Transforms each element in a Task-wrapped collection using the provided selector function and collects the results.
    /// Awaits the task, then short-circuits on the first failure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <typeparam name="U">The type of values in the resulting collection.</typeparam>
    /// <param name="source">The task containing the collection to traverse.</param>
    /// <param name="selector">The function to apply to each element.</param>
    /// <returns>A task containing a successful Result with all transformed values if all succeeded, or the first error encountered.</returns>
    public static async Task<Result<IReadOnlyList<U>>> Traverse<T, U>(
        Task<IEnumerable<T>> source,
        Func<T, Result<U>> selector) =>
            Traverse(await source.ConfigureAwait(false), selector);

    /// <summary>
    /// Asynchronously transforms each element in a Task-wrapped collection using the provided selector function and collects the results.
    /// Awaits the outer task, then processes each element with the async selector, short-circuiting on first failure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <typeparam name="U">The type of values in the resulting collection.</typeparam>
    /// <param name="source">The task containing the collection to traverse.</param>
    /// <param name="selector">The asynchronous function to apply to each element.</param>
    /// <returns>A task containing a successful Result with all transformed values if all succeeded, or the first error encountered.</returns>
    public static async Task<Result<IReadOnlyList<U>>> TraverseAsync<T, U>(
        Task<IEnumerable<T>> source,
        Func<T, Task<Result<U>>> selector) =>
            await TraverseAsync(await source.ConfigureAwait(false), selector).ConfigureAwait(false);
}
