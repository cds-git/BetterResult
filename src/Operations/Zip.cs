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
/// <c>Zip</c> for results still inside tasks, 2 to 8 of them. The tasks are awaited in turn, so start
/// them all before the call and they run at the same time. For two, the second can also be a result.
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

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3}(Result{T1}, Result{T2}, Result{T3})"/>
    public static async Task<Result<(T1, T2, T3)>> ZipAsync<T1, T2, T3>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        return first.Zip(second, third);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, TResult}(Result{T1}, Result{T2}, Result{T3}, Func{T1, T2, T3, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, T3, TResult>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Func<T1, T2, T3, TResult> selector)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        return first.Zip(second, third, selector);
    }
    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4}(Result{T1}, Result{T2}, Result{T3}, Result{T4})"/>
    public static async Task<Result<(T1, T2, T3, T4)>> ZipAsync<T1, T2, T3, T4>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, TResult}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Func{T1, T2, T3, T4, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, T3, T4, TResult>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Func<T1, T2, T3, T4, TResult> selector)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, selector);
    }
    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5})"/>
    public static async Task<Result<(T1, T2, T3, T4, T5)>> ZipAsync<T1, T2, T3, T4, T5>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5, TResult}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5}, Func{T1, T2, T3, T4, T5, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, T3, T4, T5, TResult>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask,
        Func<T1, T2, T3, T4, T5, TResult> selector)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth, selector);
    }
    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5, T6}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5}, Result{T6})"/>
    public static async Task<Result<(T1, T2, T3, T4, T5, T6)>> ZipAsync<T1, T2, T3, T4, T5, T6>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask,
        Task<Result<T6>> sixthTask)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        var sixth = await sixthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth, sixth);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5, T6, TResult}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5}, Result{T6}, Func{T1, T2, T3, T4, T5, T6, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, T3, T4, T5, T6, TResult>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask,
        Task<Result<T6>> sixthTask,
        Func<T1, T2, T3, T4, T5, T6, TResult> selector)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        var sixth = await sixthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth, sixth, selector);
    }
    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5, T6, T7}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5}, Result{T6}, Result{T7})"/>
    public static async Task<Result<(T1, T2, T3, T4, T5, T6, T7)>> ZipAsync<T1, T2, T3, T4, T5, T6, T7>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask,
        Task<Result<T6>> sixthTask,
        Task<Result<T7>> seventhTask)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        var sixth = await sixthTask.ConfigureAwait(false);
        var seventh = await seventhTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth, sixth, seventh);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5, T6, T7, TResult}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5}, Result{T6}, Result{T7}, Func{T1, T2, T3, T4, T5, T6, T7, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask,
        Task<Result<T6>> sixthTask,
        Task<Result<T7>> seventhTask,
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        var sixth = await sixthTask.ConfigureAwait(false);
        var seventh = await seventhTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth, sixth, seventh, selector);
    }
    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5, T6, T7, T8}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5}, Result{T6}, Result{T7}, Result{T8})"/>
    public static async Task<Result<(T1, T2, T3, T4, T5, T6, T7, T8)>> ZipAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask,
        Task<Result<T6>> sixthTask,
        Task<Result<T7>> seventhTask,
        Task<Result<T8>> eighthTask)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        var sixth = await sixthTask.ConfigureAwait(false);
        var seventh = await seventhTask.ConfigureAwait(false);
        var eighth = await eighthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth, sixth, seventh, eighth);
    }

    /// <inheritdoc cref="ZipExtensions.Zip{T1, T2, T3, T4, T5, T6, T7, T8, TResult}(Result{T1}, Result{T2}, Result{T3}, Result{T4}, Result{T5}, Result{T6}, Result{T7}, Result{T8}, Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult})"/>
    public static async Task<Result<TResult>> ZipAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Task<Result<T1>> firstTask,
        Task<Result<T2>> secondTask,
        Task<Result<T3>> thirdTask,
        Task<Result<T4>> fourthTask,
        Task<Result<T5>> fifthTask,
        Task<Result<T6>> sixthTask,
        Task<Result<T7>> seventhTask,
        Task<Result<T8>> eighthTask,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
    {
        var first = await firstTask.ConfigureAwait(false);
        var second = await secondTask.ConfigureAwait(false);
        var third = await thirdTask.ConfigureAwait(false);
        var fourth = await fourthTask.ConfigureAwait(false);
        var fifth = await fifthTask.ConfigureAwait(false);
        var sixth = await sixthTask.ConfigureAwait(false);
        var seventh = await seventhTask.ConfigureAwait(false);
        var eighth = await eighthTask.ConfigureAwait(false);
        return first.Zip(second, third, fourth, fifth, sixth, seventh, eighth, selector);
    }
}
