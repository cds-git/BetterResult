namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    public Result<TValue> Tap<TNextValue>(Action<TValue> action)
    {
        if (IsFailure)
        {
            return Error;
        }

        action(Value);

        return this;
    }

    public async Task<Result<TValue>> TapAsync<TNextValue>(Func<TValue, Task> action)
    {
        if (IsFailure)
        {
            return Error;
        }

        await action(Value).ConfigureAwait(false);

        return this;
    }
}