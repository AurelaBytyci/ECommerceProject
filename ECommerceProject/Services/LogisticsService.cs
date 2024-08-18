using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECommerceProject.Services
{
    public interface ILogisticsService
    {
        bool ArrangeShipment(string shipmentDetails);
        Task<string> GetOrderStatus(int orderId);
    }

    public class LogisticsService : ILogisticsService
    {
        public bool ArrangeShipment(string shipmentDetails)
        {
            try
            {
                var serializedDetails = SerializeDetails(shipmentDetails);
                var apiResponse = CallLogisticsApi(serializedDetails);

                if (apiResponse.IsSuccessStatusCode)
                {
                    SaveTrackingInfo(apiResponse.Content.ReadAsStringAsync().Result);
                    return true;
                }
                else
                {
                    LogError(apiResponse.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> GetOrderStatus(int orderId)
        {
            // Logic to retrieve the order status from a third-party API
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://logisticsapi.com/api/shipments/{orderId}/status");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    LogError(response.ReasonPhrase);
                    return "Error retrieving order status";
                }
            }
        }

        private string SerializeDetails(string details)
        {
            return JsonConvert.SerializeObject(details);
        }

        private HttpResponseMessage CallLogisticsApi(string serializedDetails)
        {
            using (var client = new HttpClient())
            {
                return client.PostAsync("https://logisticsapi.com/api/shipments", new StringContent(serializedDetails, Encoding.UTF8, "application/json")).Result;
            }
        }

        private void SaveTrackingInfo(string trackingInfo)
        {
            // Save tracking information to the database or another storage system
        }

        private void LogError(string errorMessage)
        {
            Console.WriteLine($"Logistics Service Error: {errorMessage}");
        }
    }
}