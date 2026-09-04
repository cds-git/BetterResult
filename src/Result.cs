using System.Text;

namespace BetterResult;

/// <summary>
/// Holds either a value or an <see cref="Error"/>, never both.
/// </summary>
/// <typeparam name="T">The type of the underlying <see cref="Value"/>.</typeparam>
public partial record Result<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    private Result(T? value)
    {
        IsFailure = false;
        _value = value;
        _error = null;
    }

    private Result(Error error)
    {
        if (error.Code is null)
            throw new ArgumentException(
                "Cannot create a failure Result from a default(Error). Construct the Error via one of the factory methods (Error.Failure, Error.Validation, Error.Create, ...).",
                nameof(error));

        IsFailure = true;
        _value = default;
        _error = error;
    }

    /// <summary>
    /// True when the result holds a value.
    /// </summary>
    public bool IsSuccess => !IsFailure;

    /// <summary>
    /// True when the result holds an error.
    /// </summary>
    public bool IsFailure { get; }

    /// <summary>
    /// The error. Check <see cref="IsFailure"/> first.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is a success.</exception>
    public Error Error => IsFailure
        ? (Error)_error!
        : throw new InvalidOperationException("Cannot access the error when the result is of type success. Check IsFailure before accessing Error!");

    /// <summary>
    /// The value. Check <see cref="IsSuccess"/> first.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is a failure.</exception>
    public T Value => IsFailure
        ? throw new InvalidOperationException($"Cannot access the value when result is of type failure. Check IsFailure before accessing value! {Error}")
        : _value!;

    /// <summary>
    /// Wraps a value in a successful result.
    /// </summary>
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary>
    /// Wraps an error in a failed result.
    /// </summary>
    public static implicit operator Result<T>(Error error) => new(error);

    /// <summary>
    /// Creates a successful result holding <paramref name="value"/>.
    /// </summary>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a failed result holding <paramref name="error"/>.
    /// </summary>
    public static Result<T> Failure(Error error) => new(error);

    /// <summary>
    /// Prints only the side that is populated. The record-synthesized version reads every property,
    /// and one of <see cref="Value"/> and <see cref="Error"/> always throws.
    /// </summary>
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        if (IsFailure)
            builder.Append("IsFailure = True, Error = ").Append(_error);
        else
            builder.Append("IsSuccess = True, Value = ").Append(_value);
        return true;
    }
}
