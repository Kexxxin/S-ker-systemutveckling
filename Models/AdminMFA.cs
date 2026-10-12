namespace JENSENonline.Api.Models
{
    public class AdminMFA
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string Code { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; } = false;
    }
}
