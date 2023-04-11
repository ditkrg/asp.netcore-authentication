using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DIT.Authentication.GatewayAuth.Abstractions;

public interface IForbidResponseHandler
{
    Task HandleForbiddenAsync(HttpContext context, AuthenticationProperties properties);
}

public interface ISignatureValidator
{

    void Initialize(GatewayAuthOptions options);

    Task<bool> ValidateSignatureAsync(string data, string signature);
}

public interface IClaimsProvider
{
    Task<ClaimsIdentity> GetClaimsAsync(string userHeader);
}

public interface IUserInjector<UserModel>
{
    ValueTask SetUserAsync(UserModel user);
}
