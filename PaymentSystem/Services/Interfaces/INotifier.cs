using System;
using System.Threading.Tasks;

namespace PaymentSystem.Services.Interfaces
{
    public interface INotifier
    {
        Task SendAsyncNotification(Uri target, String message);
    }
}