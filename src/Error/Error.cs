namespace BetterResult;

public readonly partial record struct Error
{
    private Error(ErrorType type, string code, string description, Dictionary<string, object>? metadata)
    {
        Type = type;
        Code = code;
        Description = description;
        Metadata = metadata;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public Dictionary<string, object>? Metadata { get; }
}