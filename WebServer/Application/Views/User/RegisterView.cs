namespace WebServer.Application.Views.User
{
    using System.Text;
    using Server.Contracts;
    public class RegisterView : IView
    {
        public string View()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<body>");
            sb.AppendLine("   <form method=\"POST\">");
            sb.AppendLine("       Name</br>");
            sb.AppendLine("       <input type=\"text\" name=\"name\" /></br>");
            sb.AppendLine("       <input type=\"submit\" />");
            sb.AppendLine("    </form>");
            sb.AppendLine("</body>");

            
            return sb.ToString().Trim();
        }
    }
}