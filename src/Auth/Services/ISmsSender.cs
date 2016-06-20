using System.Threading.Tasks;

namespace Auth.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
