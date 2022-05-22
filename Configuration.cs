namespace BlogApi;

public static class Configuration
{
    public static string JwtKey = "LPR56WwCxHt4&zuobf9N8fiHMKEg%yT77EFM";

    public static string ApiKeyName = "api_key";

    public static string ApiKey = "curso_api_@6^pEg/WVwjS[~8T";

    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}