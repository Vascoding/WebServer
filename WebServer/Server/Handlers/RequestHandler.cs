namespace WebServer.Server.Handlers
{
    using System;
    using Handlers.Contracts;
    using HTTP;
    using HTTP.Contracts;


    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            this.handlingFunc = handlingFunc;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            string sessionIdToSend = null;

            if (!context.Request.CookieCollection.ContainsKey(SessionStore.sessionCookieKey))
            {
                var sessionId = Guid.NewGuid().ToString();
                    
                context.Request.Session = SessionStore.Get(sessionId);

                sessionIdToSend = sessionId;
            }

            var response = this.handlingFunc(context.Request);

            if (sessionIdToSend != null)
            {
                response.Headers.Add(new HttpHeader(
                    HttpHeader.SetCookie,
                    $"{SessionStore.sessionCookieKey}={sessionIdToSend}; HttpOnly; path=/"));
            }

            if (!response.Headers.ContainsKey(HttpHeader.ContentType))
            {
                response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
            }

            foreach (var cookie in response.Cookies)
            {
                response.Headers.Add(new HttpHeader(HttpHeader.SetCookie, cookie.ToString()));
            }

            return response;
        }
    }
}