namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    public Result<TNextValue> Bind<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        return onSuccess(Value);
    }


    public async Task<Result<TNextValue>> BindAsync<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onValue)
    {
        if (IsFailure)
        {
            return Error;
        }

        return await onValue(Value).ConfigureAwait(false);
    }
}