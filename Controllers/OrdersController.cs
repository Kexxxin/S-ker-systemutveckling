using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JENSENonline.Api.Data;
using JENSENonline.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using JENSENonline.Api.DTOs;

namespace JENSENonline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var products = await _db.Products
                .Where(p => dto.ProductIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != dto.ProductIds.Count)
                return BadRequest("En eller flera produkter saknas");

            var order = new Order
            {
                UserId = userId!,
                TotalPrice = products.Sum(p => p.Price)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var p in products)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = p.Id,
                    Price = p.Price
                });
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                order.Id,
                order.TotalPrice,
                order.CreatedAt,
                Products = products.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl
                })
            });
        }



        [Authorize]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return Ok(orders.Select(o => new
            {
                o.Id,
                o.TotalPrice,
                o.CreatedAt,
                Products = o.Items.Select(i => new
                {
                    i.ProductId,
                    Name = i.Product.Name,
                    Price = i.Price,
                    ImageUrl = i.Product.ImageUrl

                })
            }));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
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
                    Price = i.Price,
                    ImageUrl = i.Product.ImageUrl

                })
            }));
        }


    }
}
