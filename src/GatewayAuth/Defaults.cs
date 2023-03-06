using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DIT.Authentication.GatewayAuth;

public static class GatewayAuthDefaults
{
    public const string AuthenticationScheme = "Gateway";

    public const string ConfigurationSection = "Authentication:Gateway";

    public const string UserHeader = "x-auth-user";

    public const string SignatureHeader = "x-auth-signature";
}
