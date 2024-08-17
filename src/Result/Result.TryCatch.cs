namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    public Result<TNextValue> TryCatch<TNextValue>(Func<TValue, TNextValue> onSuccess, Error error)
    {
        try
        {
            if (IsFailure)
            {
                return Error;
            }

            return onSuccess(Value);
        }
        catch
        {
            return Result<TNextValue>.Failure(error);
        }
    }

    public async Task<Result<TNextValue>> TryCatchAsync<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Error error)
    {
        try
        {
            if (IsFailure)
            {
                return Error;
            }

            return await onSuccess(Value).ConfigureAwait(false);
        }
        catch
        {
            return Result<TNextValue>.Failure(error);
        }
    }
}