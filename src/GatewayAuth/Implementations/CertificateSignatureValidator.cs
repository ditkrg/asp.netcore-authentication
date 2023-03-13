using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DIT.Authentication.GatewayAuth.Abstractions;

namespace DIT.Authentication.GatewayAuth;

internal sealed class CertificateSignatureValidator : ISignatureValidator
{
    private RSA _rsa = default!;

    public CertificateSignatureValidator() { }

    public void Initialize(GatewayAuthOptions options)
    {
        if (_rsa is not null) return;

        if (string.IsNullOrWhiteSpace(options.Certificate))
            throw new InvalidOperationException("Certificate is null or whitespace");

        var certificate = new X509Certificate2(Encoding.ASCII.GetBytes(options.Certificate));
        _rsa = certificate.GetRSAPublicKey() ?? throw new InvalidOperationException("Could not get RSA public key from certificate");
    }

    public Task<bool> ValidateSignatureAsync(string data, string signature)
    {
        if (_rsa == null) throw new InvalidOperationException("RSA is null");

        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signatureBytes = Convert.FromBase64String(signature);

        var isValid = _rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return Task.FromResult(isValid);
    }
}
