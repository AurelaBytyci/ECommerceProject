using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECommerceProject.Services
{
    public interface IPaymentService
    {
        bool ProcessPayment(string paymentDetails);
        Task<bool> ProcessPayment(Order order);
    }

    public class PaymentService : IPaymentService
    {
        public bool ProcessPayment(string paymentDetails)
        {
            try
            {
                var serializedPayment = SerializePaymentDetails(paymentDetails);
                var apiResponse = CallPaymentGateway(serializedPayment);

                if (apiResponse.IsSuccessStatusCode)
                {
                    SaveTransactionDetails(apiResponse.Content.ReadAsStringAsync().Result);
                    return true;
                }
                else
                {
                    LogPaymentError(apiResponse.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogPaymentError(ex.Message);
                return false;
            }
        }

        public async Task<bool> ProcessPayment(Order order)
        {
            try
            {
                var serializedPayment = SerializePaymentDetails(order); // Serialize the order object
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

        private HttpResponseMessage CallPaymentGateway(string serializedPayment)
        {
            using (var client = new HttpClient())
            {
                return client.PostAsync("https://paymentgateway.com/api/payments", new StringContent(serializedPayment, Encoding.UTF8, "application/json")).Result;
            }
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