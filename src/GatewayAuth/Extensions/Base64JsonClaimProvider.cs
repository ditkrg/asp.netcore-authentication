using System.Security.Claims;
using System.Text.Json;
using DIT.Authentication.GatewayAuth.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DIT.Authentication.GatewayAuth.Extensions;

public static partial class GatewayAuthExtensions
{
    public static AuthenticationBuilder AddBase64JsonClaimsProvider<TUserModel>(this AuthenticationBuilder builder, Func<TUserModel, IEnumerable<Claim>> claimsFactory)
    {
        builder.Services.TryAddScoped<IClaimsProvider>(sp => new Base64JsonClaimsProvider<TUserModel>(new(JsonSerializerDefaults.Web), claimsFactory, userInjector: sp.GetService<IUserInjector<TUserModel>>()));

        return builder;
    }

    public static AuthenticationBuilder AddBase64JsonClaimsProvider<TUserModel>(this AuthenticationBuilder builder, JsonSerializerOptions jsonSerializerOptions, Func<TUserModel, IEnumerable<Claim>> claimsFactory)
    {
        builder.Services.TryAddScoped<IClaimsProvider>(sp => new Base64JsonClaimsProvider<TUserModel>(jsonSerializerOptions, claimsFactory, userInjector: sp.GetService<IUserInjector<TUserModel>>()));

        return builder;
    }

    public static AuthenticationBuilder AddBase64JsonClaimsProvider<TUserModel>(this AuthenticationBuilder builder, Func<IServiceProvider, JsonSerializerOptions> jsonSerializerOptions, Func<TUserModel, IEnumerable<Claim>> claimsFactory)
    {
        builder.Services.TryAddScoped<IClaimsProvider>(sp => new Base64JsonClaimsProvider<TUserModel>(jsonSerializerOptions(sp), claimsFactory, userInjector: sp.GetService<IUserInjector<TUserModel>>()));

        return builder;
    }

}
