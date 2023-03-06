using System.Security.Claims;
using System.Text.Json;
using DIT.Authentication.GatewayAuth.Abstractions;

namespace DIT.Authentication.GatewayAuth;

public sealed class Base64JsonClaimsProvider<TUserModel> : IClaimsProvider
{

    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly Func<TUserModel, IEnumerable<Claim>> _claimsFactory;
    private readonly IUserInjector<TUserModel>? _userInjector;

    public Base64JsonClaimsProvider(JsonSerializerOptions serializerOptions, Func<TUserModel, IEnumerable<Claim>> claimsFactory, IUserInjector<TUserModel>? userInjector = null)
    {
        _userInjector = userInjector;
        _claimsFactory = claimsFactory;
        _jsonSerializerOptions = serializerOptions;
    }

    public async Task<ClaimsIdentity> GetClaimsAsync(string userHeader)
    {
        var decoded = Convert.FromBase64String(userHeader);
        var resp = JsonSerializer.Deserialize<TUserModel>(decoded, _jsonSerializerOptions);

        if (resp is null)
            throw new GatewayAuthException(GatewayAuthErrorCode.invalid_user_model, "Unable to deserialize user model");

        if (_userInjector is not null)
            await _userInjector.SetUserAsync(resp);

        var claims = _claimsFactory.Invoke(resp);
        var identity = new ClaimsIdentity(claims, nameof(GatewayAuthHandler));

        return identity;
    }
}
