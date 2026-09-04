namespace BetterResult;

/// <summary>
/// Placeholder for operations that succeed without producing a value.
/// Use it as <c>Result&lt;NoValue&gt;</c> where a void return would otherwise go.
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
    /// The single <see cref="NoValue"/> instance.
    /// </summary>
    public static NoValue Instance => default;
}
