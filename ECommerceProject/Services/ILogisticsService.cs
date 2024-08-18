namespace ECommerceProject.Services
{
    public interface ILogisticsService
    {
        bool ArrangeShipment(string shipmentDetails);
        Task<string> GetOrderStatus(int orderId);
    }
}
