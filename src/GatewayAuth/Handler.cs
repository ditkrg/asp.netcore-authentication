
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Encodings.Web;
using DIT.Authentication.GatewayAuth.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DIT.Authentication.GatewayAuth;

public class GatewayAuthHandler : AuthenticationHandler<GatewayAuthOptions>
{

    private readonly IClaimsProvider _claimsProvider;
    private readonly ISignatureValidator _signatureValidator;

    public GatewayAuthHandler(
        IClaimsProvider claimsProvider,
        UrlEncoder encoder,
        IOptionsMonitor<GatewayAuthOptions> options,
        ILoggerFactory logger,
        ISignatureValidator signatureValidator,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _claimsProvider = claimsProvider;
        _signatureValidator = signatureValidator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var userHeader = Request.Headers[Options.UserHeader].FirstOrDefault();
        var signatureHeader = Request.Headers[Options.SignatureHeader].FirstOrDefault();

        if (string.IsNullOrEmpty(userHeader) && string.IsNullOrEmpty(signatureHeader))
            return Options.EmptyHeadersHandler?.Invoke(Context) ?? EmptyHeaderHandler.NoResult(Context);

        if (string.IsNullOrEmpty(userHeader))
            return AuthenticateResult.Fail(new GatewayAuthException(GatewayAuthErrorCode.header_missing, "User header is missing"));

        if (string.IsNullOrEmpty(signatureHeader))
            return AuthenticateResult.Fail(new GatewayAuthException(GatewayAuthErrorCode.header_missing, "Signature header is missing"));

        if (!ExtractSignatureValue(signatureHeader, out string? extractedSignature))
            return AuthenticateResult.Fail(new GatewayAuthException(GatewayAuthErrorCode.header_invalid, "Signature header has an empty value"));

        if (!await _signatureValidator.ValidateSignatureAsync(userHeader, extractedSignature))
            return AuthenticateResult.Fail(new GatewayAuthException(GatewayAuthErrorCode.invalid_signature, "Signature is invalid"));

        try
        {
            var claimsIdentity = await _claimsProvider.GetClaimsAsync(userHeader);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (GatewayAuthException e)
        {
            return AuthenticateResult.Fail(e);
        }
    }

    private static bool ExtractSignatureValue(string signatureHeader, [NotNullWhen(true)] out string? signature)
    {
        const string signaturePrefix = "signature=";
        var span = signatureHeader.AsSpan();
        var i = span.IndexOf(signaturePrefix, StringComparison.OrdinalIgnoreCase);

        if (i < 0)
        {
            signature = null;
            return false;
        }

        span = span[(i + signaturePrefix.Length)..];
        var seperatorIndex = span.IndexOf(',');

        if (seperatorIndex >= 0)
            span = span[..seperatorIndex];

        signature = span.ToString();
        return true;
    }

}
