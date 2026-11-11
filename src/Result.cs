using System.Text;

namespace BetterResult;

/// <summary>
/// Holds either a value or an <see cref="Error"/>, never both. A <c>default</c> result is a failure
/// with the <c>General.Uninitialized</c> error.
/// </summary>
/// <typeparam name="T">The type of the underlying <see cref="Value"/>.</typeparam>
public readonly partial record struct Result<T>
{
    private readonly T? _value;

    /// <summary>
    /// The error, <see cref="Result.SuccessMarker"/> on success, null for <c>default</c>. There is no
    /// success flag, so a result over a reference type stays 16 bytes and passes in registers.
    /// </summary>
    private readonly Error? _error;

    private Result(T? value)
    {
        _value = value;
        _error = Result.SuccessMarker;
    }

    private Result(Error error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error));

        _value = default;
        _error = error;
    }

    /// <summary>
    /// True when the result holds a value.
    /// </summary>
    public bool IsSuccess => ReferenceEquals(_error, Result.SuccessMarker);

    /// <summary>
    /// True when the result holds an error.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// The error. Check <see cref="IsFailure"/> first.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is a success.</exception>
    public Error Error => IsFailure
        ? _error ?? Result.Uninitialized
        : throw new InvalidOperationException("Cannot access the error when the result is of type success. Check IsFailure before accessing Error!");

    /// <summary>
    /// The value. Check <see cref="IsSuccess"/> first.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is a failure.</exception>
    public T Value => IsFailure
        ? throw new InvalidOperationException($"Cannot access the value when result is of type failure. Check IsFailure before accessing value! {Error}")
        : _value!;

    internal Error? ErrorOrNull => IsFailure ? Error : null;

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
    private bool PrintMembers(StringBuilder builder)
    {
        if (IsFailure)
            builder.Append("IsFailure = True, Error = ").Append(Error);
        else
            builder.Append("IsSuccess = True, Value = ").Append(_value);
        return true;
    }
}
