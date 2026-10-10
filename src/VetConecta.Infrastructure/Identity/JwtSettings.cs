namespace VetConecta.Infrastructure.Identity;

/// <summary>Se enlaza desde la seccion "Jwt" de appsettings.json.</summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpiresMinutes { get; set; } = 120;
}
