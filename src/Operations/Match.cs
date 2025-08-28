namespace BetterResult;

public partial record Result
{
    /// <summary>
    /// Matches on the result state and executes the appropriate handler function.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure. Receives the error as a parameter.</param>
    /// <returns>The result of executing the appropriate handler function.</returns>
    public U Match<U>(Func<U> onSuccess, Func<Error, U> onFailure) =>
       IsSuccess ? onSuccess() : onFailure(Error);

    /// <summary>
    /// Asynchronously matches on the result state with an asynchronous success handler and synchronous failure handler.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure. Receives the error as a parameter.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public async Task<U> MatchAsync<U>(Func<Task<U>> onSuccessAsync, Func<Error, U> onFailure) =>
        IsSuccess ? await onSuccessAsync().ConfigureAwait(false) : onFailure(Error);

    /// <summary>
    /// Asynchronously matches on the result state with a synchronous success handler and asynchronous failure handler.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure. Receives the error as a parameter.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public async Task<U> MatchAsync<U>(Func<U> onSuccess, Func<Error, Task<U>> onFailureAsync) =>
        IsSuccess ? onSuccess() : await onFailureAsync(Error).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously matches on the result state with asynchronous handlers for both success and failure cases.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public async Task<U> MatchAsync<U>(Func<Task<U>> onSuccessAsync, Func<Error, Task<U>> onFailureAsync) =>
        IsSuccess ? await onSuccessAsync() : await onFailureAsync(Error).ConfigureAwait(false);
}

public partial record Result<T>
{
    /// <summary>
    /// Matches on the result state and executes the appropriate handler function.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccess">The function to execute if the result is successful. Receives the result value as a parameter.</param>
    /// <param name="onFailure">The function to execute if the result is a failure. Receives the error as a parameter.</param>
    /// <returns>The result of executing the appropriate handler function.</returns>
    public U Match<U>(Func<T, U> onSuccess, Func<Error, U> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);

    /// <summary>
    /// Asynchronously matches on the result state with an asynchronous success handler and synchronous failure handler.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful. Receives the result value as a parameter.</param>
    /// <param name="onFailure">The function to execute if the result is a failure. Receives the error as a parameter.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public async Task<U> MatchAsync<U>(Func<T, Task<U>> onSuccessAsync, Func<Error, U> onFailure) =>
        IsSuccess ? await onSuccessAsync(Value).ConfigureAwait(false) : onFailure(Error);

    /// <summary>
    /// Asynchronously matches on the result state with a synchronous success handler and asynchronous failure handler.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccess">The function to execute if the result is successful. Receives the result value as a parameter.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure. Receives the error as a parameter.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public async Task<U> MatchAsync<U>(Func<T, U> onSuccess, Func<Error, Task<U>> onFailureAsync) =>
        IsSuccess ? onSuccess(Value) : await onFailureAsync(Error).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously matches on the result state with asynchronous handlers for both success and failure cases.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful. Receives the result value as a parameter.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure. Receives the error as a parameter.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public async Task<U> MatchAsync<U>(Func<T, Task<U>> onSuccessAsync, Func<Error, Task<U>> onFailureAsync) =>
        IsSuccess ? await onSuccessAsync(Value) : await onFailureAsync(Error).ConfigureAwait(false);
}

/// <summary>
/// Provides extension methods for asynchronously matching on Task-wrapped Result instances.
/// </summary>
public static class MatchExtensions
{
    /// <summary>
    /// Asynchronously matches on a Task containing a Result with synchronous handlers.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<U>(
       this Task<Result> result, Func<U> onSuccess, Func<Error, U> onFailure) =>
           (await result.ConfigureAwait(false)).Match(onSuccess, onFailure);

    /// <summary>
    /// Asynchronously matches on a Task containing a Result with an asynchronous success handler and synchronous failure handler.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<U>(
       this Task<Result> result, Func<Task<U>> onSuccessAsync, Func<Error, U> onFailure) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccessAsync, onFailure);

    /// <summary>
    /// Asynchronously matches on a Task containing a Result with a synchronous success handler and asynchronous failure handler.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<U>(
       this Task<Result> result, Func<U> onSuccess, Func<Error, Task<U>> onFailureAsync) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccess, onFailureAsync);

    /// <summary>
    /// Asynchronously matches on a Task containing a Result with asynchronous handlers for both success and failure cases.
    /// </summary>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<U>(
       this Task<Result> result, Func<Task<U>> onSuccessAsync, Func<Error, Task<U>> onFailureAsync) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccessAsync, onFailureAsync);

    /// <summary>
    /// Asynchronously matches on a Task containing a Result with synchronous handlers.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, U> onSuccess, Func<Error, U> onFailure) =>
           (await result.ConfigureAwait(false)).Match(onSuccess, onFailure);

    /// <summary>
    /// Asynchronously matches on a Task containing a Result with an asynchronous success handler and synchronous failure handler.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, Task<U>> onSuccessAsync, Func<Error, U> onFailure) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccessAsync, onFailure);

    /// <summary>
    /// Asynchronously matches on a Task containing a Result with a synchronous success handler and asynchronous failure handler.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, U> onSuccess, Func<Error, Task<U>> onFailureAsync) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccess, onFailureAsync);

    /// <summary>
    /// Asynchronously matches on a Task containing a Result with asynchronous handlers for both success and failure cases.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <typeparam name="U">The type of the return value from the match operation.</typeparam>
    /// <param name="result">The task containing the result to match on.</param>
    /// <param name="onSuccessAsync">The asynchronous function to execute if the result is successful.</param>
    /// <param name="onFailureAsync">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task containing the result of executing the appropriate handler function.</returns>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, Task<U>> onSuccessAsync, Func<Error, Task<U>> onFailureAsync) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccessAsync, onFailureAsync);
}