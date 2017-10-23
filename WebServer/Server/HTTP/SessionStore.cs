namespace WebServer.Server.HTTP
{
    using System.Collections.Concurrent;
    public static class SessionStore
    {
        public const string sessionCookieKey = "MY-SID";
        public const string CurrentUserKey = "^%CurrentUser_User_Session_Key%^";

        private static readonly ConcurrentDictionary<string, HttpSession> sessions = new ConcurrentDictionary<string, HttpSession>();

        public static HttpSession Get(string id) => sessions.GetOrAdd(id, _ => new HttpSession(id));
    }
}