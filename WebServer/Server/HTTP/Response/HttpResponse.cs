namespace WebServer.Server.HTTP.Response
{
    using Server.Enums;
    using System.Text;
    using Server.HTTP.Contracts;
    
    public abstract class HttpResponse : IHttpResponse
    {
        private string statusCodeMessage => this.StatusCode.ToString();

        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
        }
        

        public IHttpHeaderCollection Headers { get; protected set; }

        public IHttpCookieCollection Cookies { get; protected set; }

        public HttpStatusCode StatusCode { get; protected set; }

        public override string ToString()
        {
            var response = new StringBuilder();
            var statusCodeNum = (int)this.StatusCode;
            response.AppendLine($"HTTP/1.1 {statusCodeNum} {this.statusCodeMessage}");
            response.AppendLine($"{this.Headers}");

            return response.ToString();
        }

        protected void AddHeader(string location, string redirectUrl)
        {
            this.Headers.Add(new HttpHeader(location, redirectUrl));
        }
    }
}