namespace WebServer.Server.HTTP.Response
{
    using System;
    using Enums;
    using global::WebServer.Server.Common;

    public class InternelServerErrorResponse : ViewResponse
    {
        public InternelServerErrorResponse(Exception e)
            : base(HttpStatusCode.InternalServerError, new InternalServerErrorView(e))
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}