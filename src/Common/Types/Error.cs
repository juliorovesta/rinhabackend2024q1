namespace Common.Types;

public sealed record Error(string Code, string? Message = null) : IEquatable<Error>
{
    public static implicit operator Error(string code) => new(code);

    public static implicit operator Error((string code, string? message) error) =>
        new(error.code, error.message);
}
