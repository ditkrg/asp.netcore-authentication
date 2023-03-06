namespace DIT.Authentication.GatewayAuth;

public enum GatewayAuthErrorCode
{
    header_missing,
    header_invalid,
    invalid_signature,
    invalid_user_model,
}

public sealed class GatewayAuthException : Exception
{

    public GatewayAuthErrorCode Error { get; }

    public GatewayAuthException(GatewayAuthErrorCode error, string message) : base(message)
    {
        Error = error;
    }

}
