namespace DIT.Authentication.GatewayAuth;

public static class GatewayAuthDefaults
{
    public const string AuthenticationScheme = "Gateway";

    public const string ConfigurationSection = "Gateway";

    public const string UserHeader = "x-auth-user";

    public const string SignatureHeader = "x-auth-signature";
}
