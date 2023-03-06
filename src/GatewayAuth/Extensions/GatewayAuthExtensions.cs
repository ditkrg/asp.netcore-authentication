using DIT.Authentication.GatewayAuth.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace DIT.Authentication.GatewayAuth.Extensions;

public static partial class GatewayAuthExtensions
{
    /// <summary>
    /// Enables Gateway authentication using the default scheme <see cref="GatewayAuthDefaults.AuthenticationScheme"/>.
    /// <para>
    /// Gateway authentication performs authentication by extracting and validating a user and signature from the <see cref="GatewayAuthOptions.UserHeader"/> and <see cref="GatewayAuthOptions.SignatureHeader"/> request headers.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddGateway(this AuthenticationBuilder builder)
        => builder.AddGateway(configSectionPath: GatewayAuthDefaults.ConfigurationSection);

    /// <summary>
    /// Enables Gateway authentication using the default scheme <see cref="GatewayAuthDefaults.AuthenticationScheme"/>.
    /// <para>
    /// Gateway authentication performs authentication by extracting and validating a user and signature from the <see cref="GatewayAuthOptions.UserHeader"/> and <see cref="GatewayAuthOptions.SignatureHeader"/> request headers.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddGateway(this AuthenticationBuilder builder, Action<GatewayAuthOptions> configureOptions)
        => builder.AddGateway(configSectionPath: GatewayAuthDefaults.ConfigurationSection, configureOptions);

    /// <summary>
    /// Enables Gateway authentication using a pre-defined scheme.
    /// <para>
    /// Gateway authentication performs authentication by extracting and validating a user and signature from the <see cref="GatewayAuthOptions.UserHeader"/> and <see cref="GatewayAuthOptions.SignatureHeader"/> request headers.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <param name="configSectionPath">The section path in the configuration.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddGateway(this AuthenticationBuilder builder, string configSectionPath)
        => builder.AddGateway(configSectionPath, authenticationScheme: GatewayAuthDefaults.AuthenticationScheme);

    /// <summary>
    /// Enables Gateway authentication using a pre-defined scheme.
    /// <para>
    /// Gateway authentication performs authentication by extracting and validating a user and signature from the <see cref="GatewayAuthOptions.UserHeader"/> and <see cref="GatewayAuthOptions.SignatureHeader"/> request headers.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <param name="configSectionPath">The section path in the configuration.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddGateway(this AuthenticationBuilder builder, string configSectionPath, Action<GatewayAuthOptions> configureOptions)
        => builder.AddGateway(configSectionPath, authenticationScheme: GatewayAuthDefaults.AuthenticationScheme, displayName: null, configureOptions: configureOptions);

    /// <summary>
    /// Enables Gateway authentication using the specified scheme.
    /// <para>
    /// Gateway authentication performs authentication by extracting and validating a user and signature from the <see cref="GatewayAuthOptions.UserHeader"/> and <see cref="GatewayAuthOptions.SignatureHeader"/> request headers.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <param name="configSectionPath">The section path in the configuration.</param>
    /// <param name="authenticationScheme">The authentication scheme.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddGateway(this AuthenticationBuilder builder, string configSectionPath, string authenticationScheme)
        => builder.AddGateway(configSectionPath, authenticationScheme, displayName: null, configureOptions: null);

    /// <summary>
    /// Enables Gateway authentication using the specified scheme.
    /// <para>
    /// Gateway authentication performs authentication by extracting and validating a user and signature from the <see cref="GatewayAuthOptions.UserHeader"/> and <see cref="GatewayAuthOptions.SignatureHeader"/> request headers.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <param name="authenticationScheme">The authentication scheme.</param>
    /// <param name="displayName">The display name for the authentication handler.</param>
    /// <param name="configureOptions">A delegate that allows configuring <see cref="GatewayAuthOptions"/>.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddGateway(this AuthenticationBuilder builder, string configSectionPath, string authenticationScheme, string? displayName, Action<GatewayAuthOptions>? configureOptions)
    {
        builder.Services
            .AddOptions<GatewayAuthOptions>()
            .BindConfiguration(configSectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            ;

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<GatewayAuthOptions>, PostConfigureOptions>());
        builder.Services.TryAddSingleton<ISignatureValidator, CertificateSignatureValidator>();

        return builder.AddScheme<GatewayAuthOptions, GatewayAuthHandler>(authenticationScheme, displayName, configureOptions);
    }

}
