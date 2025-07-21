namespace SyncUpC.Domain.Entities.Base;

public class RefreshToken : BaseEntity<string>
{

    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => RevokedAt == null && !IsExpired;
    public string UserId { get; set; } = null!; // Suponiendo que el ID de usuario es string
}
