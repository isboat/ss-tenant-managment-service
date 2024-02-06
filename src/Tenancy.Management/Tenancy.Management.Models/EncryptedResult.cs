namespace Tenancy.Management.Models
{
    public class EncryptedResult
    {
        public string? Hashed { get; set; }

        public string? UsedSalt { get; set; }
    }
}
