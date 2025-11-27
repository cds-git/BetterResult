namespace BetterResult;

/// <summary>
/// Represents the absence of a meaningful value.
/// Use with <see cref="Result{T}"/> for operations that don't return a value.
/// </summary>
/// <example>
/// <code>
/// Result&lt;NoValue&gt; DeleteUser(int id)
/// {
///     if (id &lt;= 0) 
///         return Error.Validation("INVALID_ID", "ID must be positive");
///     
///     _repository.Delete(id);
///     return NoValue.Instance;
/// }
/// </code>
/// </example>
public readonly record struct NoValue
{
    /// <summary>
    /// Gets the singleton instance of <see cref="NoValue"/>.
    /// </summary>
    public static NoValue Instance => default;
}