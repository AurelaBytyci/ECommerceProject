using Microsoft.EntityFrameworkCore;
using Xunit;
using ECommerceProject.Controllers;
using ECommerceProject.Models;
using ECommerceProject.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductsControllerTests
{
    private readonly ApplicationDbContext _context;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new ApplicationDbContext(options);
        _controller = new ProductsController(_context);
    }
    
    [Fact]
    public async Task GetProducts_ReturnsEmptyList_WhenNoProducts()
    {
        var result = await _controller.GetProducts(null, null, 1, 10);
        Assert.Empty(result.Value);
    }
}