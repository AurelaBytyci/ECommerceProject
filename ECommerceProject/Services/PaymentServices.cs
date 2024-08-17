using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ECommerceProject.Services
{
    public class PaymentService
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

        private string SerializePaymentDetails(string paymentDetails)
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

        private void SaveTransactionDetails(string transactionDetails)
        {
        }

        private void LogPaymentError(string errorMessage)
        {
            Console.WriteLine($"Payment Service Error: {errorMessage}");
        }
    }
}
