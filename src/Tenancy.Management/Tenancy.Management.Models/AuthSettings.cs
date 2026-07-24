namespace Tenancy.Management.Models
{
    public class AuthSettings
    {
        public string? Username { get; set; } = "admin";
        public string? Passwd { get; set;}
        public string? PasswordPepper { get; set; }
        public int InviteTokenExpiryHours { get; set; } = 24;
    }
}
