namespace ExamForge.Application.Common;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    public static readonly Error None = new Error(string.Empty, string.Empty);

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public override string ToString()
    {
        return $"{Code}: {Message}";
    }
}
