namespace JWTAuthenticationDemo.Models
{
    public class SessionVariables
    {
        public const string SessionUsername = "SessionKeyUserName";
        public const string SessionId = "SessionKeySessionId";
    }

    public enum SessionKey
    {
        SessionKeyUserName=0,
        SessionKeySessionId=1
    }
}
