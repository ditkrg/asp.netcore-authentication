using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DIT.Authentication.GatewayAuth;

public static class EmptyHeaderHandler
{
    public static readonly Func<HttpContext, AuthenticateResult> Error = ErrorHandler;

    public static readonly Func<HttpContext, AuthenticateResult> NoResult = NoResultHandler;

    public static readonly Func<HttpContext, AuthenticateResult> Success = SuccessHandler;

    private static AuthenticateResult NoResultHandler(HttpContext _) => AuthenticateResult.NoResult();

    private static AuthenticateResult ErrorHandler(HttpContext _) => AuthenticateResult.Fail("No authentication header found");

    private static AuthenticateResult SuccessHandler(HttpContext _)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "anonymous")
        };

        var identity = new ClaimsIdentity(claims, GatewayAuthDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, GatewayAuthDefaults.AuthenticationScheme);

        return AuthenticateResult.Success(ticket);
    }
}
