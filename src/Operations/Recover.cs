namespace BetterResult;

public readonly partial record struct Result<T>
{
    /// <summary>
    /// Turns any failure into whatever <paramref name="recovery"/> returns, which may be another
    /// failure. A success passes through untouched.
    /// </summary>
    public Result<T> Recover(Func<Error, Result<T>> recovery)
    {
        if (IsSuccess) return this;
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
    /// Recovers only when the error is a <typeparamref name="TError"/>. Anything else passes through
    /// untouched. Name the type on the lambda parameter, as in <c>(UserNotFound e) =&gt; e.UserId</c>,
    /// or as the type argument.
    /// </summary>
    public Result<T> Recover<TError>(Func<TError, Result<T>> recovery) where TError : Error
    {
        if (IsSuccess) return this;
        return Error is TError error ? recovery(error) : this;
    }

    /// <inheritdoc cref="Recover(Func{Error, Result{T}})"/>
    public async Task<Result<T>> RecoverAsync(Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Recover(Func{Error, bool}, Func{Error, Result{T}})"/>
    public async Task<Result<T>> RecoverAsync(Func<Error, bool> predicate, Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        if (!predicate(Error)) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Recover{TError}(Func{TError, Result{T}})"/>
    public async Task<Result<T>> RecoverAsync<TError>(Func<TError, Task<Result<T>>> recoveryAsync) where TError : Error
    {
        if (IsSuccess) return this;
        return Error is TError error ? await recoveryAsync(error).ConfigureAwait(false) : this;
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

    /// <inheritdoc cref="Result{T}.Recover(Func{Error, bool}, Func{Error, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, bool> predicate,
        Func<Error, Result<T>> recovery)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(predicate, recovery);
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

    /// <inheritdoc cref="Result{T}.Recover{TError}(Func{TError, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T, TError>(
        this Task<Result<T>> resultTask,
        Func<TError, Result<T>> recovery) where TError : Error
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(recovery);
    }

    /// <inheritdoc cref="Result{T}.Recover{TError}(Func{TError, Result{T}})"/>
    public static async Task<Result<T>> RecoverAsync<T, TError>(
        this Task<Result<T>> resultTask,
        Func<TError, Task<Result<T>>> recoveryAsync) where TError : Error
    {
        var result = await resultTask.ConfigureAwait(false);
        return await result.RecoverAsync(recoveryAsync).ConfigureAwait(false);
    }
}
