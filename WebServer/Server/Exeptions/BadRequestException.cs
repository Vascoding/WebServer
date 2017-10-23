namespace WebServer.Server.Exeptions
{
    using System;

    public class BadRequestException : Exception
    {
        private readonly string message = "Bad request";

        public BadRequestException()
        {
        }

        public BadRequestException(string message)
        {
            this.message = message;
        }

        public override string Message
        {
            get { return this.message; }
        }
    }
}