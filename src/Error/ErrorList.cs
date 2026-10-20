namespace BetterResult;

/// <summary>
/// Several errors reported together, built with <c>Combine</c>. The type is the one every error
/// shares, otherwise <see cref="ErrorType.Failure"/>. Never nested, a combined list is flattened.
/// </summary>
public sealed record ErrorList : Error
{
    internal ErrorList(IReadOnlyList<Error> errors)
        : base(SharedType(errors), "General.Multiple", $"{errors.Count} errors occurred") =>
        Errors = errors;

    /// <summary>
    /// The individual errors, in the order they were combined. Always two or more.
    /// </summary>
    public IReadOnlyList<Error> Errors { get; }

    /// <summary>
    /// Compares like <see cref="Error.Equals(Error)"/>, and the individual errors item by item.
    /// </summary>
    public bool Equals(ErrorList? other) => base.Equals(other) && Errors.SequenceEqual(other!.Errors);

    /// <inheritdoc/>
    public override int GetHashCode() => base.GetHashCode();

    private static ErrorType SharedType(IReadOnlyList<Error> errors) =>
        errors.All(e => e.Type == errors[0].Type) ? errors[0].Type : ErrorType.Failure;
}
