namespace Server.Common.Utils;
public static class EnvUtil
{
    public static bool IsDevelopment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    public static bool IsProduction() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
}