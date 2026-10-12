using JENSENonline.Api.Data;
using JENSENonline.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JENSENonline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction("GetById", "Products", new { id = product.Id }, product);
        }

        [HttpPut("products/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updated)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = updated.Name;
            product.Price = updated.Price;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("products/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .ToListAsync();

            return Ok(orders.Select(o => new
            {
                o.Id,
                o.TotalPrice,
                o.CreatedAt,
                UserEmail = o.User.Email,
                Products = o.Items.Select(i => new
                {
                    i.ProductId,
                    Name = i.Product.Name,
                    Price = i.Price
                })
            }));
        }
    }
}
