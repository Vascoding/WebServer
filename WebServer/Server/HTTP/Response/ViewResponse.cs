namespace WebServer.Server.HTTP.Response
{
    using Server.Enums;
    using Server.Contracts;
    using Exeptions;
    

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;
        
        public ViewResponse(HttpStatusCode statusCode, IView view)
        {
            this.ValidateStatusCode(statusCode);

            this.view = view;
            this.StatusCode = statusCode;

            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var statusCodeNum = (int) statusCode;
            if (statusCodeNum < 299 && statusCodeNum > 400)
            {
                throw new InvalidResponseExeption("View response needs a status code below 300 and above 400 (inclusive).");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.view.View()}";
        }
    }
}