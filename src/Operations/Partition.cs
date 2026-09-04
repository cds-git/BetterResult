namespace BetterResult;

/// <summary>
/// Splits a collection of results into successes and failures.
/// </summary>
public static class PartitionExtensions
{
    /// <summary>
    /// Processes every item and returns the values and the errors side by side. Unlike <c>Sequence</c>
    /// and <c>Traverse</c> it does not short-circuit, so use it when partial success is acceptable.
    /// </summary>
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

    /// <inheritdoc cref="Partition{T}(IEnumerable{Result{T}})"/>
    public static async Task<(IReadOnlyList<T> Successes, IReadOnlyList<Error> Failures)> Partition<T>(
        this Task<IEnumerable<Result<T>>> source) =>
            (await source.ConfigureAwait(false)).Partition();
}
