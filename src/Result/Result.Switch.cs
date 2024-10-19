namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result"/>.
    /// If the state is success, the provided action <paramref name="onSuccess"/> is executed.
    /// If the state is a failure, the provided action <paramref name="onFailure"/> is executed.
    /// </summary>
    /// <param name="onSuccess">The action to execute if the state is success.</param>
    /// <param name="onFailure">The action to execute if the state is a failure.</param>
    public void Switch(Action onSuccess, Action<Error> onFailure)
    {
        if (IsFailure)
        {
            onFailure(Error);
            return;
        }

        onSuccess();
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="Result"/>.
    /// If the state is success, the provided action <paramref name="onSuccess"/> is executed asynchronously.
    /// If the state is a failure, the provided action <paramref name="onFailure"/> is executed asynchronously.
    /// </summary>
    /// <param name="onSuccess">The asynchronous action to execute if the state is success.</param>
    /// <param name="onFailure">The asynchronous action to execute if the state is a failure.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchAsync(Func<Task> onSuccess, Func<Error, Task> onFailure)
    {
        if (IsFailure)
        {
            await onFailure(Error).ConfigureAwait(false);
            return;
        }

        await onSuccess().ConfigureAwait(false);
    }
}

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided action <paramref name="onSuccess"/> is executed.
    /// If the state is an error, the provided action <paramref name="onFailure"/> is executed.
    /// </summary>
    /// <param name="onSuccess">The action to execute if the state is a value.</param>
    /// <param name="onFailure">The action to execute if the state is an error.</param>
    public void Switch<TNextValue>(Action<TValue> onSuccess, Action<Error> onFailure)
    {
        if (IsFailure)
        {
            onFailure(Error);
            return;
        }

        onSuccess(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided action <paramref name="onSuccess"/> is executed asynchronously.
    /// If the state is an error, the provided action <paramref name="onFailure"/> is executed asynchronously.
    /// </summary>
    /// <param name="onSuccess">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onFailure">The asynchronous action to execute if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchAsync<TNextValue>(Func<TValue, Task> onSuccess, Func<Error, Task> onFailure)
    {
        if (IsFailure)
        {
            await onFailure(Error).ConfigureAwait(false);
            return;
        }

        await onSuccess(Value).ConfigureAwait(false);
    }
}