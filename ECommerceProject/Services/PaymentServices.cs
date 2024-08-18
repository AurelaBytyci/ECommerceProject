using System;
using System.Threading.Tasks;
using ECommerceProject.Models;

namespace ECommerceProject.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(Order order);
    }

    public class PaymentService : IPaymentService
    {
        public async Task<bool> ProcessPayment(Order order)
        {
            try
            {
                // Simulate the payment process without calling an external gateway.
                await Task.Delay(1000); // Simulate a delay for processing

                order.Status = "Paid"; // Assume payment is successful
                SaveTransactionDetails(order);
                return true;
            }
            catch (Exception ex)
            {
                throw new PaymentException("Payment processing failed.", ex);
            }
        }

        private void SaveTransactionDetails(Order order)
        {
            // Logic to save transaction details to the database or another storage system
            Console.WriteLine($"Transaction for Order ID {order.OrderId} has been processed successfully.");
        }
    }

    public class PaymentException : Exception
    {
        public PaymentException() { }

        public PaymentException(string message) : base(message) { }

        public PaymentException(string message, Exception inner) : base(message, inner) { }
    }
}