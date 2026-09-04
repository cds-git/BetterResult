namespace BetterResult;

/// <summary>
/// A fluent two-result <c>Combine</c>. Use <c>Result.Combine</c> when you have three or more at once.
/// </summary>
public static class ZipExtensions
{
    /// <summary>
    /// Combines two results into one holding a tuple of both values, or the first error.
    /// </summary>
    public static Result<(T, TOther)> Zip<T, TOther>(this Result<T> result, Result<TOther> other) =>
        Result.Combine(result, other);

    /// <summary>
    /// Combines two results and feeds both values to <paramref name="selector"/>, or returns the first
    /// error.
    /// </summary>
    public static Result<TResult> Zip<T, TOther, TResult>(this Result<T> result, Result<TOther> other, Func<T, TOther, TResult> selector) =>
        Result.Combine(result, other, selector);
}

/// <summary>
/// <see cref="ZipExtensions.Zip{T, TOther}"/> for results that are still inside a task. Each task is
/// awaited in turn, so start them beforehand if you want them running in parallel.
/// </summary>
public static class ZipTaskExtensions
{
    /// <inheritdoc cref="ZipExtensions.Zip{T, TOther}(Result{T}, Result{TOther})"/>
    public static async Task<Result<(T, TOther)>> Zip<T, TOther>(
        this Task<Result<T>> resultTask,
        Result<TOther> other)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Zip(other);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T, TOther, TResult}(Result{T}, Result{TOther}, Func{T, TOther, TResult})"/>
    public static async Task<Result<TResult>> Zip<T, TOther, TResult>(
        this Task<Result<T>> resultTask,
        Result<TOther> other,
        Func<T, TOther, TResult> selector)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Zip(other, selector);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T, TOther}(Result{T}, Result{TOther})"/>
    public static async Task<Result<(T, TOther)>> Zip<T, TOther>(
        this Task<Result<T>> resultTask,
        Task<Result<TOther>> otherTask)
    {
        var result = await resultTask.ConfigureAwait(false);
        var other = await otherTask.ConfigureAwait(false);
        return result.Zip(other);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T, TOther, TResult}(Result{T}, Result{TOther}, Func{T, TOther, TResult})"/>
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
    /// Combines two results and feeds both values to an async <paramref name="selector"/>, or returns
    /// the first error.
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

    /// <inheritdoc cref="ZipAsync{T, TOther, TResult}(Task{Result{T}}, Task{Result{TOther}}, Func{T, TOther, Task{TResult}})"/>
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
