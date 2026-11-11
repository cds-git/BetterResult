namespace BetterResult;

/// <summary>
/// Turns a nullable value into a result, failing when it is null.
/// </summary>
public static class ToResultExtensions
{
    /// <summary>
    /// Succeeds with the value, or fails with <paramref name="error"/> when it is null.
    /// </summary>
    public static Result<T> ToResult<T>(this T? value, Error error) where T : class =>
        value is null ? Result<T>.Failure(error) : Result<T>.Success(value);

    /// <inheritdoc cref="ToResult{T}(T, Error)"/>
    public static Result<T> ToResult<T>(this T? value, Error error) where T : struct =>
        value is null ? Result<T>.Failure(error) : Result<T>.Success(value.Value);

    /// <inheritdoc cref="ToResult{T}(T, Error)"/>
    public static async Task<Result<T>> ToResultAsync<T>(this Task<T?> value, Error error) where T : class =>
        (await value.ConfigureAwait(false)).ToResult(error);

    /// <inheritdoc cref="ToResult{T}(T, Error)"/>
    public static async Task<Result<T>> ToResultAsync<T>(this Task<T?> value, Error error) where T : struct =>
        (await value.ConfigureAwait(false)).ToResult(error);
}
