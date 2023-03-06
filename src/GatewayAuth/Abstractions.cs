using System.Security.Claims;

namespace DIT.Authentication.GatewayAuth.Abstractions;

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
