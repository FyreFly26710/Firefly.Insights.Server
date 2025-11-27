namespace Server.Common.Exceptions;

public abstract class HandledException : Exception
{
    protected HandledException(string? message) : base(message) { }

    public abstract int StatusCode { get; }
}

public class ExceptionBadRequest : HandledException
{
    public ExceptionBadRequest(string? message = null) : base(message ?? DefaultMessage) { }
    private const string DefaultMessage = "Bad request";
    public override int StatusCode => 400;
}

public class ExceptionUnauthorized : HandledException
{
    public ExceptionUnauthorized(string? message = null) : base(message ?? DefaultMessage) { }
    private const string DefaultMessage = "Unauthorized";
    public override int StatusCode => 401;
}

public class ExceptionForbidden : HandledException
{
    public ExceptionForbidden(string? message = null) : base(message ?? DefaultMessage) { }
    private const string DefaultMessage = "Forbidden";
    public override int StatusCode => 403;

}

public class ExceptionNotFound : HandledException
{
    public ExceptionNotFound(string? message = null) : base(message ?? DefaultMessage) { }
    private const string DefaultMessage = "Resource Not Found";
    public override int StatusCode => 404;

}
