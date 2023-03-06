using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DIT.Authentication.GatewayAuth;

public class GatewayAuthOptions : AuthenticationSchemeOptions
{
    [Required]
    public string Certificate { get; set; } = string.Empty;

    [Required]
    public string UserHeader { get; set; } = GatewayAuthDefaults.UserHeader;

    [Required]
    public string SignatureHeader { get; set; } = GatewayAuthDefaults.SignatureHeader;

    public Func<HttpContext, AuthenticateResult>? EmptyHeadersHandler { get; set; }
}
