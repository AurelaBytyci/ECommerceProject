using System.Threading.Tasks;
using ECommerceProject.Models;

namespace ECommerceProject.Services
{
    public interface IPaymentService
    {
        bool ProcessPayment(string paymentDetails);
        Task<bool> ProcessPayment(Order order);
    }
}