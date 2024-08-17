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
                // Serialize payment details into the required format (e.g., JSON)
                var serializedPayment = SerializePaymentDetails(paymentDetails);

                // Call the payment gateway API
                var apiResponse = CallPaymentGateway(serializedPayment);

                // Process the API response
                if (apiResponse.IsSuccessStatusCode)
                {
                    // Handle successful payment
                    SaveTransactionDetails(apiResponse.Content.ReadAsStringAsync().Result);
                    return true;
                }
                else
                {
                    // Handle payment failure
                    LogPaymentError(apiResponse.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the payment process
                LogPaymentError(ex.Message);
                return false;
            }
        }

        private string SerializePaymentDetails(string paymentDetails)
        {
            // Convert payment details to JSON or other required format
            return JsonConvert.SerializeObject(paymentDetails);
        }

        private HttpResponseMessage CallPaymentGateway(string serializedPayment)
        {
            // Example HTTP call to the payment gateway API
            using (var client = new HttpClient())
            {
                // Assume the payment gateway has an endpoint like "/api/payments"
                return client.PostAsync("https://paymentgateway.com/api/payments", new StringContent(serializedPayment, Encoding.UTF8, "application/json")).Result;
            }
        }

        private void SaveTransactionDetails(string transactionDetails)
        {
            // Save the transaction details to the database
            // This could involve calling a repository method to save the transaction
            // Example: _transactionRepository.Save(transactionDetails);
        }

        private void LogPaymentError(string errorMessage)
        {
            // Log any payment errors for debugging or auditing
            Console.WriteLine($"Payment Service Error: {errorMessage}");
        }
    }
}
