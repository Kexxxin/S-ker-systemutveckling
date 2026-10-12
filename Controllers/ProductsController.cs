using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JENSENonline.Api.Data;
using JENSENonline.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JENSENonline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext db, ILogger<ProductsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _db.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (_db.Products.Any(x => x.Name == product.Name))
                return BadRequest("En produkt med detta namn finns redan.");

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Admin skapade produkt {Name} med ID {Id}", product.Name, product.Id);

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product updated)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = updated.Name;
            product.Price = updated.Price;
            product.ImageUrl = updated.ImageUrl;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Admin uppdaterade produkt {Id}", id);

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Admin tog bort produkt {Id}", id);

            return Ok();
        }
    }
}
