using System.Threading.Tasks;
using ECommerceProject.Models;

namespace ECommerceProject.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(Order order);
    }
}
