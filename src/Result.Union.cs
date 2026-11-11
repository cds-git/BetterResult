#if NET11_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BetterResult;

/// <summary>
/// A C# union of <typeparamref name="T"/> and <see cref="Error"/>, so a switch on the result matches
/// the value or the error directly and is exhaustive without a discard arm.
/// </summary>
[Union]
public sealed partial record Result<T> : Result<T>.IUnionMembers
{
    /// <summary>
    /// The union members the compiler uses for conversions and pattern matching. Call the members on
    /// <see cref="Result{T}"/> instead.
    /// </summary>
    public interface IUnionMembers
    {
        /// <summary>Wraps a value in a successful result.</summary>
        static Result<T> Create(T value) => new(value);

        /// <summary>Wraps an error in a failed result.</summary>
        static Result<T> Create(Error value) => new(value);

        /// <summary>The value or the error. Null only for a success holding null.</summary>
        object? Value { get; }

        /// <summary>False only for a success holding null.</summary>
        bool HasValue { get; }

        /// <summary>True for a success holding a non null value.</summary>
        bool TryGetValue([MaybeNullWhen(false)] out T value);

        /// <summary>True for a failure.</summary>
        bool TryGetValue([MaybeNullWhen(false)] out Error value);
    }

    object? IUnionMembers.Value => IsFailure ? _error : _value;

    bool IUnionMembers.HasValue => IsFailure || _value is not null;

    bool IUnionMembers.TryGetValue([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return IsSuccess && _value is not null;
    }

    bool IUnionMembers.TryGetValue([MaybeNullWhen(false)] out Error value)
    {
        value = _error;
        return IsFailure;
    }
}
#endif
