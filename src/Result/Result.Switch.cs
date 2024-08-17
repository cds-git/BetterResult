namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    public void Switch<TNextValue>(Action<TValue> onSuccess, Action<Error> onFailure)
    {
        if (IsFailure)
        {
            onFailure(Error);
            return;
        }

        onSuccess(Value);
    }

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