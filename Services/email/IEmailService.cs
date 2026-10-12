using System.Threading.Tasks;

namespace homework.Services.email
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string body);
    }
}