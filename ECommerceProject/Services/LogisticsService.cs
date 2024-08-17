using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ECommerceProject.Services
{
    public class LogisticsService
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
        }

        private void LogError(string errorMessage)
        {
            Console.WriteLine($"Logistics Service Error: {errorMessage}");
        }
    }
}