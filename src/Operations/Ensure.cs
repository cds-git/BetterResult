namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Fails with <paramref name="error"/> if <paramref name="predicate"/> rejects the value. A failure
    /// passes through and the predicate never runs.
    /// </summary>
    public Result<T> Ensure(Func<T, bool> predicate, Error error)
    {
        if (IsFailure) return Error;

        return predicate(Value) ? this : error;
    }

    /// <summary>
    /// Fails with an error built from the value if <paramref name="predicate"/> rejects it. Use this
    /// overload when the message needs the value for context.
    /// </summary>
    public Result<T> Ensure(Func<T, bool> predicate, Func<T, Error> errorFactory)
    {
        if (IsFailure) return Error;

        return predicate(Value) ? this : errorFactory(Value);
    }

    /// <inheritdoc cref="Ensure(Func{T, bool}, Error)"/>
    public async Task<Result<T>> EnsureAsync(Func<T, Task<bool>> predicate, Error error)
    {
        if (IsFailure) return Error;

        return await predicate(Value).ConfigureAwait(false) ? this : error;
    }

    /// <inheritdoc cref="Ensure(Func{T, bool}, Func{T, Error})"/>
    public async Task<Result<T>> EnsureAsync(Func<T, Task<bool>> predicate, Func<T, Error> errorFactory)
    {
        if (IsFailure) return Error;

        return await predicate(Value).ConfigureAwait(false) ? this : errorFactory(Value);
    }
}

/// <summary>
/// <c>Ensure</c> for results that are still inside a task.
/// </summary>
public static class EnsureExtensions
{
    /// <inheritdoc cref="Result{T}.Ensure(Func{T, bool}, Error)"/>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, bool> predicate, Error error) =>
            (await result.ConfigureAwait(false)).Ensure(predicate, error);

    /// <inheritdoc cref="Result{T}.Ensure(Func{T, bool}, Func{T, Error})"/>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, bool> predicate, Func<T, Error> errorFactory) =>
            (await result.ConfigureAwait(false)).Ensure(predicate, errorFactory);

    /// <inheritdoc cref="Result{T}.Ensure(Func{T, bool}, Error)"/>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, Task<bool>> predicate, Error error) =>
            await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);

    /// <inheritdoc cref="Result{T}.Ensure(Func{T, bool}, Func{T, Error})"/>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<T, Error> errorFactory) =>
            await (await result.ConfigureAwait(false)).EnsureAsync(predicate, errorFactory).ConfigureAwait(false);
}
