namespace JENSENonline.Api.Services
{
    public interface IAppEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }
}
