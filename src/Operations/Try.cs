namespace BetterResult;

public static partial class Result
{
    internal static Error CreateExceptionError(Exception ex) =>
        Error.Unexpected("EXCEPTION", ex.Message)
            .WithMetadata("ExceptionType", ex.GetType().Name);

    internal static Error InvokeErrorMapper(Func<Exception, Error> errorMapper, Exception ex)
    {
        var mapped = errorMapper(ex);
        // A mapper returning null would throw out of the catch block via the implicit Error to Result
        // conversion, breaking Try's promise never to throw.
        return mapped ?? CreateExceptionError(ex);
    }

    /// <summary>
    /// Runs <paramref name="operation"/> and converts any exception into an <c>Unexpected</c> error
    /// carrying the exception message and type. <see cref="OperationCanceledException"/> is not caught.
    /// </summary>
    public static Result<T> Try<T>(Func<T> operation)
    {
        try
        {
            return operation();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return CreateExceptionError(ex);
        }
    }

    /// <summary>
    /// Runs <paramref name="operation"/> and turns any exception into an error via
    /// <paramref name="errorMapper"/>. A mapper returning null falls back to the
    /// standard <c>Unexpected</c> error.
    /// </summary>
    public static Result<T> Try<T>(Func<T> operation, Func<Exception, Error> errorMapper)
    {
        try
        {
            return operation();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return InvokeErrorMapper(errorMapper, ex);
        }
    }

    /// <inheritdoc cref="Try{T}(Func{T})"/>
    public static Result<NoValue> Try(Action operation) =>
        Try(() =>
        {
            operation();
            return NoValue.Instance;
        });

    /// <inheritdoc cref="Try{T}(Func{T}, Func{Exception, Error})"/>
    public static Result<NoValue> Try(Action operation, Func<Exception, Error> errorMapper) =>
        Try(() =>
        {
            operation();
            return NoValue.Instance;
        }, errorMapper);

    /// <inheritdoc cref="Try{T}(Func{T})"/>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation().ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return CreateExceptionError(ex);
        }
    }

    /// <inheritdoc cref="Try{T}(Func{T}, Func{Exception, Error})"/>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> operation, Func<Exception, Error> errorMapper)
    {
        try
        {
            return await operation().ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return InvokeErrorMapper(errorMapper, ex);
        }
    }

    /// <inheritdoc cref="Try{T}(Func{T})"/>
    public static Task<Result<NoValue>> TryAsync(Func<Task> operation) =>
        TryAsync(async () =>
        {
            await operation().ConfigureAwait(false);
            return NoValue.Instance;
        });

    /// <inheritdoc cref="Try{T}(Func{T}, Func{Exception, Error})"/>
    public static Task<Result<NoValue>> TryAsync(Func<Task> operation, Func<Exception, Error> errorMapper) =>
        TryAsync(async () =>
        {
            await operation().ConfigureAwait(false);
            return NoValue.Instance;
        }, errorMapper);
}

public partial record Result<T>
{
    /// <summary>
    /// Runs an operation that may throw and converts any exception into an <c>Unexpected</c> error
    /// carrying the exception message and type. A failure passes through and the operation never runs.
    /// </summary>
    /// <remarks>
    /// Stack traces are left out on purpose: they leak file paths and internal call structure if the
    /// error crosses a trust boundary. Use the <c>errorMapper</c> overload if you need them.
    /// <see cref="OperationCanceledException"/> is not caught, so cooperative cancellation keeps working.
    /// </remarks>
    public Result<U> Try<U>(Func<T, U> operation)
    {
        if (IsFailure) return Error;

        try
        {
            return operation(Value);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Result.CreateExceptionError(ex);
        }
    }

    /// <summary>
    /// Runs an operation that may throw and turns any exception into an error via
    /// <paramref name="errorMapper"/>. If the mapper returns null the standard
    /// <c>Unexpected</c> error is used instead, so <c>Try</c> never throws.
    /// </summary>
    public Result<U> Try<U>(Func<T, U> operation, Func<Exception, Error> errorMapper)
    {
        if (IsFailure) return Error;

        try
        {
            return operation(Value);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Result.InvokeErrorMapper(errorMapper, ex);
        }
    }

    /// <inheritdoc cref="Try{U}(Func{T, U})"/>
    public async Task<Result<U>> TryAsync<U>(Func<T, Task<U>> operation)
    {
        if (IsFailure) return Error;

        try
        {
            return await operation(Value).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Result.CreateExceptionError(ex);
        }
    }

    /// <inheritdoc cref="Try{U}(Func{T, U}, Func{Exception, Error})"/>
    public async Task<Result<U>> TryAsync<U>(Func<T, Task<U>> operation, Func<Exception, Error> errorMapper)
    {
        if (IsFailure) return Error;

        try
        {
            return await operation(Value).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Result.InvokeErrorMapper(errorMapper, ex);
        }
    }
}

/// <summary>
/// <c>Try</c> for results that are still inside a task.
/// </summary>
public static class TryExtensions
{
    /// <inheritdoc cref="Result{T}.Try{U}(Func{T, U})"/>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, U> operation) =>
            (await result.ConfigureAwait(false)).Try(operation);

    /// <inheritdoc cref="Result{T}.Try{U}(Func{T, U}, Func{Exception, Error})"/>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, U> operation, Func<Exception, Error> errorMapper) =>
            (await result.ConfigureAwait(false)).Try(operation, errorMapper);

    /// <inheritdoc cref="Result{T}.Try{U}(Func{T, U})"/>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<U>> operation) =>
            await (await result.ConfigureAwait(false)).TryAsync(operation).ConfigureAwait(false);

    /// <inheritdoc cref="Result{T}.Try{U}(Func{T, U}, Func{Exception, Error})"/>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<U>> operation, Func<Exception, Error> errorMapper) =>
            await (await result.ConfigureAwait(false)).TryAsync(operation, errorMapper).ConfigureAwait(false);
}
