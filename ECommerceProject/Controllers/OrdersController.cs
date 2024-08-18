using ECommerceProject.Models;
using ECommerceProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

[Authorize(Roles = "Admin, Customer, Staff")]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogisticsService _logisticsService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogisticsService logisticsService, ILogger<OrdersController> logger)
    {
        _logisticsService = logisticsService;
        _logger = logger;
    }

    [HttpGet("{orderId}/status")]
    public async Task<IActionResult> GetOrderStatus(int orderId)
    {
        var status = await _logisticsService.GetOrderStatus(orderId);
        if (status == null)
        {
            return NotFound();
        }
        return Ok(status);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        try
        {
            _logger.LogInformation("Creating a new order for CustomerId: {CustomerId}", order.CustomerId);
            // Order creation logic
            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating an order for CustomerId: {CustomerId}", order.CustomerId);
            return StatusCode(500, "Internal server error");
        }
    }
}