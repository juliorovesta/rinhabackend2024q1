namespace Common.Types;

public static class Result
{
    public static Result<TValue> FromSuccess<TValue>(TValue value) => value;

    public static Result<dynamic> FromError(Error error) => error;
}

public class Result<TValue> : Result<TValue, Error>
{
    protected Result(TValue value)
        : base(value) { }

    protected Result(Error error)
        : base(error) { }

    public Result<TValue> Match(Func<TValue, Result<TValue>> success) =>
        IsSuccess ? success(Value!) : Error!;

    public Result<TValue> Match(
        Func<TValue, Result<TValue>> success,
        Func<Error, Result<TValue>> failure
    ) => IsSuccess ? success(Value!) : failure(Error!);

    public Result<TNewValue> Match<TNewValue>(Func<TValue, Result<TNewValue>> success) =>
        IsSuccess ? success(Value!) : Error!;

    public Result<TNewValue> Match<TNewValue>(
        Func<TValue, Result<TNewValue>> success,
        Func<Error, Result<TNewValue>> failure
    ) => IsSuccess ? success(Value!) : failure(Error!);

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Error error) => new(error);
}

public class Result<TValue, TError>
{
    public TValue? Value { get; }
    public TError? Error { get; }

    public bool IsSuccess { get; }

    public bool IsFailure { get; }

    protected Result(TValue value)
    {
        IsSuccess = true;
        IsFailure = !IsSuccess;
        Value = value;
        Error = default;
    }

    protected Result(TError error)
    {
        IsFailure = true;
        IsSuccess = !IsFailure;
        Error = error;
        Value = default;
    }

    public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success) =>
        IsSuccess ? success(Value!) : Error!;

    public Result<TValue, TError> Match(
        Func<TValue, Result<TValue, TError>> success,
        Func<TError, Result<TValue, TError>> failure
    ) => IsSuccess ? success(Value!) : failure(Error!);

    public Result<TNewValue, TError> Match<TNewValue>(
        Func<TValue, Result<TNewValue, TError>> success
    ) => IsSuccess ? success(Value!) : Error!;

    public Result<TNewValue, TError> Match<TNewValue>(
        Func<TValue, Result<TNewValue, TError>> success,
        Func<TError, Result<TNewValue, TError>> failure
    ) => IsSuccess ? success(Value!) : failure(Error!);

    public Result<TNewValue, TNewError> Match<TNewValue, TNewError>(
        Func<TValue, Result<TNewValue, TNewError>> success,
        Func<TError, Result<TNewValue, TNewError>> failure
    ) => IsSuccess ? success(Value!) : failure(Error!);

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public static implicit operator Result<TValue, TError>(TError error) => new(error);
}
