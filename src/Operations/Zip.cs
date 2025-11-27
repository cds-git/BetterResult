namespace BetterResult;

/// <summary>
/// Provides extension methods for combining Results using Zip.
/// </summary>
public static class ZipExtensions
{
    /// <summary>
    /// Combines this Result with another Result into a single Result containing a tuple of both values.
    /// Returns success only if both Results are successful, otherwise returns the first error encountered.
    /// This is a fluent wrapper around Result.Combine for 2 parameters.
    /// </summary>
    /// <typeparam name="T">The type of value in the first Result.</typeparam>
    /// <typeparam name="TOther">The type of value in the other Result.</typeparam>
    /// <param name="result">The first Result to combine.</param>
    /// <param name="other">The other Result to combine with.</param>
    /// <returns>A Result containing a tuple of both values if both succeed, or the first error encountered.</returns>
    public static Result<(T, TOther)> Zip<T, TOther>(this Result<T> result, Result<TOther> other) =>
        Result.Combine(result, other);

    /// <summary>
    /// Combines this Result with another Result and applies a selector function to produce a new Result.
    /// Returns success only if both Results are successful, otherwise returns the first error encountered.
    /// This is a fluent wrapper around Result.Combine for 2 parameters with projection.
    /// </summary>
    /// <typeparam name="T">The type of value in the first Result.</typeparam>
    /// <typeparam name="TOther">The type of value in the other Result.</typeparam>
    /// <typeparam name="TResult">The type of the result after applying the selector.</typeparam>
    /// <param name="result">The first Result to combine.</param>
    /// <param name="other">The other Result to combine with.</param>
    /// <param name="selector">The function to apply to both values.</param>
    /// <returns>A Result containing the transformed value if both succeed, or the first error encountered.</returns>
    public static Result<TResult> Zip<T, TOther, TResult>(this Result<T> result, Result<TOther> other, Func<T, TOther, TResult> selector) =>
        Result.Combine(result, other, selector);
}

/// <summary>
/// Provides extension methods for combining Task-wrapped Results using Zip.
/// </summary>
public static class ZipTaskExtensions
{
    /// <summary>
    /// Combines a Task-wrapped Result with another Result into a single Result containing a tuple of both values.
    /// Awaits the task, then combines with the other Result.
    /// </summary>
    public static async Task<Result<(T, TOther)>> Zip<T, TOther>(
        this Task<Result<T>> resultTask,
        Result<TOther> other)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Zip(other);
    }

    /// <summary>
    /// Combines a Task-wrapped Result with another Result and applies a selector function.
    /// Awaits the task, then combines with the other Result.
    /// </summary>
    public static async Task<Result<TResult>> Zip<T, TOther, TResult>(
        this Task<Result<T>> resultTask,
        Result<TOther> other,
        Func<T, TOther, TResult> selector)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Zip(other, selector);
    }

    /// <summary>
    /// Combines a Task-wrapped Result with another Task-wrapped Result into a single Result containing a tuple of both values.
    /// Awaits the first task, then awaits the second task, then combines.
    /// </summary>
    public static async Task<Result<(T, TOther)>> Zip<T, TOther>(
        this Task<Result<T>> resultTask,
        Task<Result<TOther>> otherTask)
    {
        var result = await resultTask.ConfigureAwait(false);
        var other = await otherTask.ConfigureAwait(false);
        return result.Zip(other);
    }

    /// <summary>
    /// Combines a Task-wrapped Result with another Task-wrapped Result and applies a selector function.
    /// Awaits the first task, then awaits the second task, then combines.
    /// </summary>
    public static async Task<Result<TResult>> Zip<T, TOther, TResult>(
        this Task<Result<T>> resultTask,
        Task<Result<TOther>> otherTask,
        Func<T, TOther, TResult> selector)
    {
        var result = await resultTask.ConfigureAwait(false);
        var other = await otherTask.ConfigureAwait(false);
        return result.Zip(other, selector);
    }

    /// <summary>
    /// Combines a Task-wrapped Result with another Task-wrapped Result and applies an async selector function.
    /// Awaits both tasks, then applies the async selector.
    /// </summary>
    public static async Task<Result<TResult>> ZipAsync<T, TOther, TResult>(
        this Task<Result<T>> resultTask,
        Task<Result<TOther>> otherTask,
        Func<T, TOther, Task<TResult>> selector)
    {
        var result = await resultTask.ConfigureAwait(false);
        var other = await otherTask.ConfigureAwait(false);
        
        if (result.IsFailure) return result.Error;
        if (other.IsFailure) return other.Error;
        
        var value = await selector(result.Value, other.Value).ConfigureAwait(false);
        return value;
    }

    /// <summary>
    /// Combines a Result with another Task-wrapped Result and applies an async selector function.
    /// Awaits the task, then applies the async selector.
    /// </summary>
    public static async Task<Result<TResult>> ZipAsync<T, TOther, TResult>(
        this Result<T> result,
        Task<Result<TOther>> otherTask,
        Func<T, TOther, Task<TResult>> selector)
    {
        var other = await otherTask.ConfigureAwait(false);
        
        if (result.IsFailure) return result.Error;
        if (other.IsFailure) return other.Error;
        
        var value = await selector(result.Value, other.Value).ConfigureAwait(false);
        return value;
    }
}
