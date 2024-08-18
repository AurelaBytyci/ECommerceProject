using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ECommerceProject.Models;
using Newtonsoft.Json;

namespace ECommerceProject.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<bool> ProcessPayment(Order order)
        {
            try
            {
                var serializedPayment = SerializePaymentDetails(order);
                var apiResponse = await CallPaymentGatewayAsync(serializedPayment);

                if (apiResponse.IsSuccessStatusCode)
                {
                    order.Status = "Paid";
                    SaveTransactionDetails(await apiResponse.Content.ReadAsStringAsync());
                    return true;
                }
                else
                {
                    order.Status = "Payment Failed";
                    LogPaymentError(apiResponse.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new PaymentException("Payment processing failed.", ex);
            }
        }

        private string SerializePaymentDetails(object paymentDetails)
        {
            return JsonConvert.SerializeObject(paymentDetails);
        }

        private async Task<HttpResponseMessage> CallPaymentGatewayAsync(string serializedPayment)
        {
            using (var client = new HttpClient())
            {
                return await client.PostAsync("https://paymentgateway.com/api/payments", new StringContent(serializedPayment, Encoding.UTF8, "application/json"));
            }
        }

        private void SaveTransactionDetails(string transactionDetails)
        {
            // Logic to save transaction details to the database or another storage system
        }

        private void LogPaymentError(string errorMessage)
        {
            Console.WriteLine($"Payment Service Error: {errorMessage}");
        }
    }
}