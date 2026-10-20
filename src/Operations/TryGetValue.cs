using System.Diagnostics.CodeAnalysis;

namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Returns true and the value on success, or false and the error on failure. The side that is not
    /// populated is <c>default</c>.
    /// </summary>
    public bool TryGetValue([MaybeNullWhen(false)] out T value, out Error error)
    {
        value = IsSuccess ? _value! : default;
        error = IsFailure ? (Error)_error! : default;
        return IsSuccess;
    }
}
