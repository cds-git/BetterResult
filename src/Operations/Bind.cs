namespace BetterResult;

public partial record Result
{
    /// <summary>
    /// Chains another operation that returns a Result. If the current result is successful, 
    /// executes the bind function; otherwise, propagates the current error.
    /// </summary>
    /// <param name="bind">The function to execute if the current result is successful.</param>
    /// <returns>The result of the bind function if successful, or the current error if failed.</returns>
    public Result Bind(Func<Result> bind) =>
        IsSuccess ? bind() : Error;

    /// <summary>
    /// Asynchronously chains another operation that returns a Result. If the current result is successful,
    /// executes the asynchronous bind function; otherwise, propagates the current error.
    /// </summary>
    /// <param name="bindAsync">The asynchronous function to execute if the current result is successful.</param>
    /// <returns>A task containing the result of the bind function if successful, or the current error if failed.</returns>
    public async Task<Result> BindAsync(Func<Task<Result>> bindAsync) =>
        IsSuccess ? await bindAsync().ConfigureAwait(false) : Error;
}

public partial record Result<T>
{
    /// <summary>
    /// Chains another operation that transforms the value and returns a Result. If the current result is successful,
    /// executes the bind function with the current value; otherwise, propagates the current error.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="bind">The function to execute with the current value if the result is successful.</param>
    /// <returns>The result of the bind function if successful, or the current error if failed.</returns>
    public Result<U> Bind<U>(Func<T, Result<U>> bind) =>
        IsSuccess ? bind(Value) : Error;

    /// <summary>
    /// Asynchronously chains another operation that transforms the value and returns a Result. If the current result is successful,
    /// executes the asynchronous bind function with the current value; otherwise, propagates the current error.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="bindAsync">The asynchronous function to execute with the current value if the result is successful.</param>
    /// <returns>A task containing the result of the bind function if successful, or the current error if failed.</returns>
    public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> bindAsync) =>
        IsSuccess ? await bindAsync(Value).ConfigureAwait(false) : Error;
}

/// <summary>
/// Provides extension methods for asynchronously binding operations on Task-wrapped Result instances.
/// </summary>
public static class BindExtensions
{
    /// <summary>
    /// Asynchronously chains a synchronous bind operation on a Task-wrapped Result.
    /// </summary>
    /// <param name="result">The task containing the result to bind on.</param>
    /// <param name="bind">The function to execute if the result is successful.</param>
    /// <returns>A task containing the result of the bind operation if successful, or the original error if failed.</returns>
    public static async Task<Result> BindAsync(
        this Task<Result> result, Func<Result> bind) =>
            (await result.ConfigureAwait(false)).Bind(bind);

    /// <summary>
    /// Asynchronously chains an asynchronous bind operation on a Task-wrapped Result.
    /// </summary>
    /// <param name="result">The task containing the result to bind on.</param>
    /// <param name="bindAsync">The asynchronous function to execute if the result is successful.</param>
    /// <returns>A task containing the result of the bind operation if successful, or the original error if failed.</returns>
    public static async Task<Result> BindAsync(
        this Task<Result> result, Func<Task<Result>> bindAsync) =>
            await (await result.ConfigureAwait(false)).BindAsync(bindAsync).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously chains a synchronous bind operation on a Task-wrapped Result with value transformation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to bind on.</param>
    /// <param name="bind">The function to execute with the current value if the result is successful.</param>
    /// <returns>A task containing the result of the bind operation if successful, or the original error if failed.</returns>
    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> result, Func<T, Result<U>> bind) =>
            (await result.ConfigureAwait(false)).Bind(bind);

    /// <summary>
    /// Asynchronously chains an asynchronous bind operation on a Task-wrapped Result with value transformation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to bind on.</param>
    /// <param name="bindAsync">The asynchronous function to execute with the current value if the result is successful.</param>
    /// <returns>A task containing the result of the bind operation if successful, or the original error if failed.</returns>
    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<Result<U>>> bindAsync) =>
            await (await result.ConfigureAwait(false)).BindAsync(bindAsync).ConfigureAwait(false);
}