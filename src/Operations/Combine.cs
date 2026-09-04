namespace BetterResult;

public static partial class Result
{
    // 2 parameters
    /// <summary>
    /// Combines independent results into one holding a tuple of every value. Short-circuits on the
    /// first failure and returns its error. Overloads take 2 to 8 results.
    /// </summary>
    public static Result<(T1, T2)> Combine<T1, T2>(
        Result<T1> result1,
        Result<T2> result2)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        return (result1.Value, result2.Value);
    }

    /// <summary>
    /// Combines independent results and feeds the values to <paramref name="selector"/>. Short-circuits
    /// on the first failure and returns its error. Overloads take 2 to 8 results.
    /// </summary>
    public static Result<TResult> Combine<T1, T2, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Func<T1, T2, TResult> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        return selector(result1.Value, result2.Value);
    }

    // 3 parameters
    /// <inheritdoc cref="Combine{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3)> Combine<T1, T2, T3>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        return (result1.Value, result2.Value, result3.Value);
    }

    /// <inheritdoc cref="Combine{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Combine<T1, T2, T3, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Func<T1, T2, T3, TResult> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        return selector(result1.Value, result2.Value, result3.Value);
    }

    // 4 parameters
    /// <inheritdoc cref="Combine{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4)> Combine<T1, T2, T3, T4>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        return (result1.Value, result2.Value, result3.Value, result4.Value);
    }

    /// <inheritdoc cref="Combine{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Combine<T1, T2, T3, T4, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Func<T1, T2, T3, T4, TResult> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        return selector(result1.Value, result2.Value, result3.Value, result4.Value);
    }

    // 5 parameters
    /// <inheritdoc cref="Combine{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5)> Combine<T1, T2, T3, T4, T5>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        return (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value);
    }

    /// <inheritdoc cref="Combine{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Combine<T1, T2, T3, T4, T5, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Func<T1, T2, T3, T4, T5, TResult> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        return selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value);
    }

    // 6 parameters
    /// <inheritdoc cref="Combine{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5, T6)> Combine<T1, T2, T3, T4, T5, T6>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        return (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value);
    }

    /// <inheritdoc cref="Combine{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Combine<T1, T2, T3, T4, T5, T6, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Func<T1, T2, T3, T4, T5, T6, TResult> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        return selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value);
    }

    // 7 parameters
    /// <inheritdoc cref="Combine{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5, T6, T7)> Combine<T1, T2, T3, T4, T5, T6, T7>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Result<T7> result7)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        if (result7.IsFailure) return result7.Error;
        return (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value);
    }

    /// <inheritdoc cref="Combine{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Combine<T1, T2, T3, T4, T5, T6, T7, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Result<T7> result7,
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        if (result7.IsFailure) return result7.Error;
        return selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value);
    }

    // 8 parameters
    /// <inheritdoc cref="Combine{T1, T2}(Result{T1}, Result{T2})"/>
    public static Result<(T1, T2, T3, T4, T5, T6, T7, T8)> Combine<T1, T2, T3, T4, T5, T6, T7, T8>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Result<T7> result7,
        Result<T8> result8)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        if (result7.IsFailure) return result7.Error;
        if (result8.IsFailure) return result8.Error;
        return (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value, result8.Value);
    }

    /// <inheritdoc cref="Combine{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    public static Result<TResult> Combine<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Result<T7> result7,
        Result<T8> result8,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        if (result7.IsFailure) return result7.Error;
        if (result8.IsFailure) return result8.Error;
        return selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value, result8.Value);
    }

    // Async selector overloads

    // 2 parameters
    /// <summary>
    /// Same as <see cref="Combine{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, TResult})"/>
    /// but with an async selector. Overloads take 2 to 8 results.
    /// </summary>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Func<T1, T2, Task<TResult>> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        var value = await selector(result1.Value, result2.Value).ConfigureAwait(false);
        return value;
    }

    // 3 parameters
    /// <inheritdoc cref="CombineAsync{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, Task{TResult}})"/>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Func<T1, T2, T3, Task<TResult>> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        var value = await selector(result1.Value, result2.Value, result3.Value).ConfigureAwait(false);
        return value;
    }

    // 4 parameters
    /// <inheritdoc cref="CombineAsync{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, Task{TResult}})"/>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, T4, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Func<T1, T2, T3, T4, Task<TResult>> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        var value = await selector(result1.Value, result2.Value, result3.Value, result4.Value).ConfigureAwait(false);
        return value;
    }

    // 5 parameters
    /// <inheritdoc cref="CombineAsync{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, Task{TResult}})"/>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, T4, T5, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Func<T1, T2, T3, T4, T5, Task<TResult>> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        var value = await selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value).ConfigureAwait(false);
        return value;
    }

    // 6 parameters
    /// <inheritdoc cref="CombineAsync{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, Task{TResult}})"/>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, T4, T5, T6, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Func<T1, T2, T3, T4, T5, T6, Task<TResult>> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        var value = await selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value).ConfigureAwait(false);
        return value;
    }

    // 7 parameters
    /// <inheritdoc cref="CombineAsync{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, Task{TResult}})"/>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Result<T7> result7,
        Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        if (result7.IsFailure) return result7.Error;
        var value = await selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value).ConfigureAwait(false);
        return value;
    }

    // 8 parameters
    /// <inheritdoc cref="CombineAsync{T1, T2, TResult}(Result{T1}, Result{T2}, Func{T1, T2, Task{TResult}})"/>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Result<T7> result7,
        Result<T8> result8,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> selector)
    {
        if (result1.IsFailure) return result1.Error;
        if (result2.IsFailure) return result2.Error;
        if (result3.IsFailure) return result3.Error;
        if (result4.IsFailure) return result4.Error;
        if (result5.IsFailure) return result5.Error;
        if (result6.IsFailure) return result6.Error;
        if (result7.IsFailure) return result7.Error;
        if (result8.IsFailure) return result8.Error;
        var value = await selector(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value, result8.Value).ConfigureAwait(false);
        return value;
    }
}
