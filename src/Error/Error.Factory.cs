namespace BetterResult;

public readonly partial record struct Error
{
    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unexpected, code, description, metadata);

    public static Error Validation(
        string code = "General.Validation",
        string description = "Validation error has occurred.",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Validation, code, description, metadata);

    public static Error NotFound(
        string code = "General.NotFound",
        string description = "Not Found",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.NotFound, code, description, metadata);

    public static Error Conflict(
        string code = "General.Conflict",
        string description = "Conflict",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Conflict, code, description, metadata);

    public static Error Failure(
           string code = "General.Failure",
           string description = "Failure",
           Dictionary<string, object>? metadata = null) =>
               new(ErrorType.Failure, code, description, metadata);

    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "Unauthorized",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unauthorized, code, description, metadata);

    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "Forbidden",
        Dictionary<string, object>? metadata = null) =>
        new(ErrorType.Forbidden, code, description, metadata);

    public static Error Unavailable(
        string code = "General.Unavailable",
        string description = "The requested service was unavailable",
        Dictionary<string, object>? metadata = null) =>
        new(ErrorType.Unavailable, code, description, metadata);
}