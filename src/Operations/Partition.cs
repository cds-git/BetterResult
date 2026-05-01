namespace BetterResult;

/// <summary>
/// Provides extension methods for partitioning collections of Result instances into successes and failures.
/// </summary>
public static class PartitionExtensions
{
    /// <summary>
    /// Partitions a collection of Results into two separate collections: successes and failures.
    /// Unlike Sequence/Traverse, this processes all items without short-circuiting.
    /// </summary>
    /// <typeparam name="T">The type of values in the results.</typeparam>
    /// <param name="source">The collection of results to partition.</param>
    /// <returns>A tuple containing all successful values and all errors encountered.</returns>
    public static (IReadOnlyList<T> Successes, IReadOnlyList<Error> Failures) Partition<T>(
        this IEnumerable<Result<T>> source)
    {
        var successes = new List<T>();
        var failures = new List<Error>();

        foreach (var result in source)
        {
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return (successes.AsReadOnly(), failures.AsReadOnly());
    }

    /// <summary>
    /// Partitions a Task-wrapped collection of Results into two separate collections: successes and failures.
    /// Awaits the task, then processes all items without short-circuiting.
    /// </summary>
    /// <typeparam name="T">The type of values in the results.</typeparam>
    /// <param name="source">The task containing a collection of results to partition.</param>
    /// <returns>A task containing a tuple with all successful values and all errors encountered.</returns>
    public static async Task<(IReadOnlyList<T> Successes, IReadOnlyList<Error> Failures)> Partition<T>(
        this Task<IEnumerable<Result<T>>> source) =>
            (await source.ConfigureAwait(false)).Partition();
}
