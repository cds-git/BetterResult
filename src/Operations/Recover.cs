namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Turns any failure into whatever <paramref name="recovery"/> returns. A success passes through
    /// untouched.
    /// </summary>
    /// <remarks>
    /// The recovery function may return another failure, which is useful for fallbacks that can
    /// themselves fail. Use the <see cref="ErrorType"/>, error-type set, or predicate overloads to
    /// recover from specific errors only. Unlike <c>MapError</c>, which only relabels an error,
    /// <c>Recover</c> can get back to a success.
    /// </remarks>
    public Result<T> Recover(Func<Error, Result<T>> recovery)
    {
        if (IsSuccess) return this;
        return recovery(Error);
    }

    /// <summary>
    /// Recovers only when the error is of <paramref name="errorType"/>. Anything else passes through
    /// untouched.
    /// </summary>
    public Result<T> Recover(ErrorType errorType, Func<Error, Result<T>> recovery)
    {
        if (IsSuccess) return this;
        if (Error.Type != errorType) return this;
        return recovery(Error);
    }

    /// <summary>
    /// Recovers only when the error is one of <paramref name="errorTypes"/>. Anything else passes
    /// through untouched.
    /// </summary>
    public Result<T> Recover(IEnumerable<ErrorType> errorTypes, Func<Error, Result<T>> recovery)
    {
        if (IsSuccess) return this;
        if (!errorTypes.Contains(Error.Type)) return this;
        return recovery(Error);
    }

    /// <summary>
    /// Recovers only when the error satisfies <paramref name="predicate"/>. Anything else passes
    /// through untouched.
    /// </summary>
    public Result<T> Recover(Func<Error, bool> predicate, Func<Error, Result<T>> recovery)
    {
        if (IsSuccess) return this;
        if (!predicate(Error)) return this;
        return recovery(Error);
    }

    /// <summary>
    /// Falls back to <paramref name="fallbackValue"/> when the error is of <paramref name="errorType"/>.
    /// Anything else passes through untouched.
    /// </summary>
    public Result<T> Recover(ErrorType errorType, T fallbackValue)
    {
        if (IsSuccess) return this;
        if (Error.Type != errorType) return this;
        return fallbackValue;
    }

    /// <summary>
    /// Falls back to <paramref name="fallbackValue"/> when the error is one of
    /// <paramref name="errorTypes"/>. Anything else passes through untouched.
    /// </summary>
    public Result<T> Recover(IEnumerable<ErrorType> errorTypes, T fallbackValue)
    {
        if (IsSuccess) return this;
        if (!errorTypes.Contains(Error.Type)) return this;
        return fallbackValue;
    }

    /// <inheritdoc cref="Recover(Func{Error, Result{T}})"/>
    public async Task<Result<T>> RecoverAsync(Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Recover(ErrorType, Func{Error, Result{T}})"/>
    public async Task<Result<T>> RecoverAsync(ErrorType errorType, Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        if (Error.Type != errorType) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Recover(IEnumerable{ErrorType}, Func{Error, Result{T}})"/>
    public async Task<Result<T>> RecoverAsync(IEnumerable<ErrorType> errorTypes, Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        if (!errorTypes.Contains(Error.Type)) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Recover(Func{Error, bool}, Func{Error, Result{T}})"/>
    public async Task<Result<T>> RecoverAsync(Func<Error, bool> predicate, Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        if (!predicate(Error)) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }
}

/// <summary>
/// <c>Recover</c> for results that are still inside a task.
/// </summary>
public static class RecoverExtensions
{
    /// <inheritdoc cref="Result{T}.Recover(Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, Result<T>> recovery)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(recovery);
    }

    /// <inheritdoc cref="Result{T}.Recover(Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, Task<Result<T>>> recoveryAsync)
    {
        var result = await resultTask.ConfigureAwait(false);
        return await result.RecoverAsync(recoveryAsync).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{T}.Recover(ErrorType, Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        ErrorType errorType,
        Func<Error, Result<T>> recovery)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(errorType, recovery);
    }

    /// <inheritdoc cref="Result{T}.Recover(IEnumerable{ErrorType}, Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        IEnumerable<ErrorType> errorTypes,
        Func<Error, Result<T>> recovery)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(errorTypes, recovery);
    }

    /// <inheritdoc cref="Result{T}.Recover(Func{Error, bool}, Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, bool> predicate,
        Func<Error, Result<T>> recovery)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(predicate, recovery);
    }

    /// <inheritdoc cref="Result{T}.Recover(ErrorType, T)"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        ErrorType errorType,
        T fallbackValue)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(errorType, fallbackValue);
    }

    /// <inheritdoc cref="Result{T}.Recover(IEnumerable{ErrorType}, T)"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        IEnumerable<ErrorType> errorTypes,
        T fallbackValue)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(errorTypes, fallbackValue);
    }

    /// <inheritdoc cref="Result{T}.Recover(ErrorType, Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        ErrorType errorType,
        Func<Error, Task<Result<T>>> recoveryAsync)
    {
        var result = await resultTask.ConfigureAwait(false);
        return await result.RecoverAsync(errorType, recoveryAsync).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{T}.Recover(IEnumerable{ErrorType}, Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        IEnumerable<ErrorType> errorTypes,
        Func<Error, Task<Result<T>>> recoveryAsync)
    {
        var result = await resultTask.ConfigureAwait(false);
        return await result.RecoverAsync(errorTypes, recoveryAsync).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{T}.Recover(Func{Error, bool}, Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, bool> predicate,
        Func<Error, Task<Result<T>>> recoveryAsync)
    {
        var result = await resultTask.ConfigureAwait(false);
        return await result.RecoverAsync(predicate, recoveryAsync).ConfigureAwait(false);
    }
}
