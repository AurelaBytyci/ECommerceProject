namespace UnitTest
{
    public class ProductControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductsController _controller;

        public ProductControllerTests()
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
            var result = await _controller.GetProducts();
            Assert.Empty(result.Value);
        } 
    }
}