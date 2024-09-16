namespace Ciclo.API.Responses;

public class DevelopmentExceptionResponse : ExceptionResponse
{
    public Exception Exception { get; private set; }

    public DevelopmentExceptionResponse(Exception exception)
    {
        Exception = exception;
    }

    public DevelopmentExceptionResponse(string title, Exception exception) : base(title)
    {
        Exception = exception;
    }
}