using DIT.Authentication.GatewayAuth.Abstractions;
using Microsoft.Extensions.Options;

namespace DIT.Authentication.GatewayAuth;

public sealed class PostConfigureOptions : IPostConfigureOptions<GatewayAuthOptions>
{
    private readonly ISignatureValidator _signatureValidator;

    public PostConfigureOptions(ISignatureValidator signatureValidator)
    {
        _signatureValidator = signatureValidator;
    }

    public void PostConfigure(string? name, GatewayAuthOptions options)
    {
        if (options.Certificate == null)
            throw new InvalidOperationException("Certificate is null");

        _signatureValidator.Initialize(options);
    }
}
