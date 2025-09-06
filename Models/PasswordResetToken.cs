namespace efcoreApi.Models
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RegisterId { get; set; }
        public virtual Register Register { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
    }

}
