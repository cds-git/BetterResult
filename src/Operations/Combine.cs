namespace BetterResult;

public static partial class Result
{
    // 2 parameters
    /// <summary>
    /// Combines two independent Results into a single Result containing a tuple of both values.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
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
    /// Combines two independent Results and applies a selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
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
    /// <summary>
    /// Combines three independent Results into a single Result containing a tuple of all values.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    /// <summary>
    /// Combines three independent Results and applies a selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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
    /// <summary>
    /// Combines four independent Results into a single Result containing a tuple of all values.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    /// <summary>
    /// Combines four independent Results and applies a selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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
    /// <summary>
    /// Combines five independent Results into a single Result containing a tuple of all values.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    /// <summary>
    /// Combines five independent Results and applies a selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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
    /// <summary>
    /// Combines six independent Results into a single Result containing a tuple of all values.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    /// <summary>
    /// Combines six independent Results and applies a selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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
    /// <summary>
    /// Combines seven independent Results into a single Result containing a tuple of all values.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    /// <summary>
    /// Combines seven independent Results and applies a selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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
    /// <summary>
    /// Combines eight independent Results into a single Result containing a tuple of all values.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    /// <summary>
    /// Combines eight independent Results and applies a selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    // Async selector overloads (2-8 parameters)

    // 2 parameters with async selector
    /// <summary>
    /// Combines two independent Results and applies an async selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
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

    // 3 parameters with async selector
    /// <summary>
    /// Combines three independent Results and applies an async selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    // 4 parameters with async selector
    /// <summary>
    /// Combines four independent Results and applies an async selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    // 5 parameters with async selector
    /// <summary>
    /// Combines five independent Results and applies an async selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    // 6 parameters with async selector
    /// <summary>
    /// Combines six independent Results and applies an async selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    // 7 parameters with async selector
    /// <summary>
    /// Combines seven independent Results and applies an async selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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

    // 8 parameters with async selector
    /// <summary>
    /// Combines eight independent Results and applies an async selector function to produce a new Result.
    /// Returns success only if all Results are successful, otherwise returns the first error encountered.
    /// </summary>
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
