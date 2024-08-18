using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ECommerceProject.Data;
using ECommerceProject.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(
        string searchString,
        string sortOrder,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var customers = from c in _context.Customers
                        select c;

        if (!string.IsNullOrEmpty(searchString))
        {
            customers = customers.Where(c => c.LastName.Contains(searchString));
        }

        switch (sortOrder)
        {
            case "name_desc":
                customers = customers.OrderByDescending(c => c.LastName);
                break;
            default:
                customers = customers.OrderBy(c => c.LastName);
                break;
        }

        return await customers.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return customer;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, Customer customer)
    {
        if (id != customer.CustomerId)
        {
            return BadRequest();
        }

        _context.Entry(customer).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool CustomerExists(int id)
    {
        return _context.Customers.Any(e => e.CustomerId == id);
    }
}
