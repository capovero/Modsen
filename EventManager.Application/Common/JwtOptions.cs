namespace EventManagement.Application.Common;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenExpiresMinutes { get; set; }
    public int RefreshTokenExpiresDays { get; set; }
}