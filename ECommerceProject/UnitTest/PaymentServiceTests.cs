using ECommerceProject.Models;
using ECommerceProject.Services;
using System.Threading.Tasks;
using Xunit;

public class PaymentServiceTests
{
    private readonly IPaymentService _paymentService;

    public PaymentServiceTests()
    {
        _paymentService = new PaymentService();
    }

    [Fact]
    public async Task ProcessPayment_ShouldReturnTrue_WhenPaymentIsSuccessful()
    {
        var order = new Order { OrderId = 1, TotalAmount = 100.00M };
        var result = await _paymentService.ProcessPayment(order);
        Assert.True(result);
        Assert.Equal("Paid", order.Status);
    }

    [Fact]
    public async Task ProcessPayment_ShouldThrowPaymentException_OnFailure()
    {
        var order = new Order { OrderId = 1, TotalAmount = 100.00M };
        await Assert.ThrowsAsync<PaymentException>(() => _paymentService.ProcessPayment(order));
    }
}
