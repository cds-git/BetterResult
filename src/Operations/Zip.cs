namespace BetterResult;

/// <summary>
/// Combines independent results into one. When any fail, the result holds every error in argument
/// order, combined with <see cref="ErrorExtensions.Combine(IEnumerable{Error})"/>.
/// </summary>
public static class ZipExtensions
{
    /// <summary>
    /// Combines the results into one holding a tuple of every value. Overloads take 2 to 8 results.
    /// </summary>
    public static Result<(T1, T2)> Zip<T1, T2>(
        this Result<T1> first,
        Result<T2> second)
    {
        if (first.IsFailure || second.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull);
        return (first.Value, second.Value);
    }

    /// <summary>
    /// Combines the results and feeds every value to <paramref name="selector"/>, which does not run
    /// when any fail. Overloads take 2 to 8 results.
    /// </summary>
    public static Result<TResult> Zip<T1, T2, TResult>(
        this Result<T1> first,
        Result<T2> second,
        Func<T1, T2, TResult> selector)
    {
        if (first.IsFailure || second.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull);
        return selector(first.Value, second.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3)> Zip<T1, T2, T3>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull);
        return (first.Value, second.Value, third.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Zip<T1, T2, T3, TResult>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Func<T1, T2, T3, TResult> selector)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull);
        return selector(first.Value, second.Value, third.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull);
        return (first.Value, second.Value, third.Value, fourth.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Zip<T1, T2, T3, T4, TResult>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Func<T1, T2, T3, T4, TResult> selector)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull);
        return selector(first.Value, second.Value, third.Value, fourth.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull);
        return (first.Value, second.Value, third.Value, fourth.Value, fifth.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Func<T1, T2, T3, T4, T5, TResult> selector)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull);
        return selector(first.Value, second.Value, third.Value, fourth.Value, fifth.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure || sixth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull, sixth.ErrorOrNull);
        return (first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Zip<T1, T2, T3, T4, T5, T6, TResult>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Func<T1, T2, T3, T4, T5, T6, TResult> selector)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure || sixth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull, sixth.ErrorOrNull);
        return selector(first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5, T6, T7)> Zip<T1, T2, T3, T4, T5, T6, T7>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Result<T7> seventh)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure || sixth.IsFailure || seventh.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull, sixth.ErrorOrNull, seventh.ErrorOrNull);
        return (first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value, seventh.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Result<T7> seventh,
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure || sixth.IsFailure || seventh.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull, sixth.ErrorOrNull, seventh.ErrorOrNull);
        return selector(first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value, seventh.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5, T6, T7, T8)> Zip<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Result<T7> seventh,
        Result<T8> eighth)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure || sixth.IsFailure || seventh.IsFailure || eighth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull, sixth.ErrorOrNull, seventh.ErrorOrNull, eighth.ErrorOrNull);
        return (first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value, seventh.Value, eighth.Value);
    }

    /// <inheritdoc cref="Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Result<T7> seventh,
        Result<T8> eighth,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
    {
        if (first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure || fifth.IsFailure || sixth.IsFailure || seventh.IsFailure || eighth.IsFailure)
            return CollectErrors(first.ErrorOrNull, second.ErrorOrNull, third.ErrorOrNull, fourth.ErrorOrNull, fifth.ErrorOrNull, sixth.ErrorOrNull, seventh.ErrorOrNull, eighth.ErrorOrNull);
        return selector(first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value, seventh.Value, eighth.Value);
    }

    private static Error CollectErrors(params Error?[] errors) =>
        errors.Where(e => e is not null).Select(e => e!).Combine();
}

/// <summary>
/// <c>Zip</c> for two results where the first, or both, are still inside a task. Start both tasks before
/// the call and they run at the same time.
/// </summary>
public static class ZipTaskExtensions
{
    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static async Task<Result<(T1, T2)>> ZipAsync<T1, T2>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        return first.Zip(second);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, TResult>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Func<T1, T2, TResult> selector)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        return first.Zip(second, selector);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2}(Result{T1}, Result{T2})"/>
    public static async Task<Result<(T1, T2)>> ZipAsync<T1, T2>(
        this Task<Result<T1>> firstTask,
        Result<T2> second) =>
            (await firstTask.ConfigureAwait(false)).Zip(second);

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, TResult>(
        this Task<Result<T1>> firstTask,
        Result<T2> second,
        Func<T1, T2, TResult> selector) =>
            (await firstTask.ConfigureAwait(false)).Zip(second, selector);
}
