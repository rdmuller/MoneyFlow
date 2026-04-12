using System.Diagnostics.CodeAnalysis;

namespace SharedKernel.Abstractions;

public class Result
{
    protected Result(bool isSuccess, List<Error>? errors)
    {
        if (isSuccess && errors?.Count != 0)
            throw new InvalidOperationException("A successful result cannot have an error.");

        if (!isSuccess && errors?.Count == 0)
            throw new InvalidOperationException("A failure result must have an error.");

        Errors = errors;
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public List<Error>? Errors { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(Error error) => new(false, [error]);

    public static Result Failure(List<Error> errors) => new(false, errors);

    public static Result<T> Success<T>(T value) => new(value, true);

    public static Result<T> Failure<T>(Error error) => new(default!, false, error);

    public static Result<T> Failure<T>(List<Error> errors) => new(default!, false, errors);

    public static Result<T> Create<T>(T? value) => value is not null 
        ? Success(value)
        : Failure<T>(Error.NullValue);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    public Result(T? value, bool isSuccess, List<Error>? errors) : base(isSuccess, errors)
    {
        _value = value;
    }

    public Result(T? value, bool isSuccess, Error? error = null) : base(isSuccess, error is not null ? [error] : null)
    {
        _value = value;
    }

    [NotNull]
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<T>(T value) => Create(value);
}