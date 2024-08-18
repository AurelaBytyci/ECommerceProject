using ECommerceProject.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class LogisticsService : ILogisticsService
{
    public bool ArrangeShipment(string shipmentDetails)
    {
        throw new System.NotImplementedException();
    }

    public async Task<string> GetOrderStatus(int orderId)
    {
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
    private void LogError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }
}
