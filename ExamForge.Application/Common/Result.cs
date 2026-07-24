namespace ExamForge.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new ArgumentException("A successful result cannot have an error.");
        if (!isSuccess && error == Error.None)
            throw new ArgumentException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error ?? Error.None;
    }
    public static Result Success()
    {
        return new Result(true, Error.None);
    }
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }



}
