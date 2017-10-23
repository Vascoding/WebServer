namespace WebServer.Server.Handlers.Contracts
{
    using Server.HTTP.Contracts;
    

    public interface IRequestHandler
    {
        IHttpResponse Handle(IHttpContext context);
    }
}