namespace BetterResult;

/// <summary>
/// Provides extension methods for sequencing collections of Result instances.
/// </summary>
public static class SequenceExtensions
{
    /// <summary>
    /// Sequences a collection of Results into a single Result containing a collection of values.
    /// Short-circuits on the first failure, returning that error without processing remaining items.
    /// </summary>
    /// <typeparam name="T">The type of values in the results.</typeparam>
    /// <param name="source">The collection of results to sequence.</param>
    /// <returns>A successful Result containing all values if all results succeeded, or the first error encountered.</returns>
    public static Result<IReadOnlyList<T>> Sequence<T>(this IEnumerable<Result<T>> source)
    {
        var list = new List<T>();
        foreach (var result in source)
        {
            if (result.IsFailure)
                return result.Error;
            list.Add(result.Value);
        }
        return list;
    }

    /// <summary>
    /// Asynchronously sequences a collection of Task-wrapped Results into a single Result containing a collection of values.
    /// Short-circuits on the first failure, returning that error without processing remaining items.
    /// </summary>
    /// <typeparam name="T">The type of values in the results.</typeparam>
    /// <param name="source">The collection of tasks containing results to sequence.</param>
    /// <returns>A task containing a successful Result with all values if all results succeeded, or the first error encountered.</returns>
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
        return list;
    }

    /// <summary>
    /// Sequences a Task-wrapped collection of Results into a single Result containing a collection of values.
    /// Awaits the task, then short-circuits on the first failure.
    /// </summary>
    /// <typeparam name="T">The type of values in the results.</typeparam>
    /// <param name="source">The task containing a collection of results to sequence.</param>
    /// <returns>A task containing a successful Result with all values if all results succeeded, or the first error encountered.</returns>
    public static async Task<Result<IReadOnlyList<T>>> Sequence<T>(
        this Task<IEnumerable<Result<T>>> source) =>
            (await source.ConfigureAwait(false)).Sequence();

    /// <summary>
    /// Sequences a Task-wrapped collection of Task-wrapped Results.
    /// Awaits the outer task, then sequences the inner tasks.
    /// </summary>
    /// <typeparam name="T">The type of values in the results.</typeparam>
    /// <param name="source">The task containing a collection of tasks containing results.</param>
    /// <returns>A task containing a successful Result with all values if all results succeeded, or the first error encountered.</returns>
    public static async Task<Result<IReadOnlyList<T>>> SequenceAsync<T>(
        this Task<IEnumerable<Task<Result<T>>>> source) =>
            await (await source.ConfigureAwait(false)).SequenceAsync().ConfigureAwait(false);
}
