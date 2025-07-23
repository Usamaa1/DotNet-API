
// Models/Cart.cs
using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

// Models/CartItem.cs
namespace WebAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}

// DTOs/CartItemDto.cs
namespace WebAPI.DTOs
{
    public class CartItemDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}

// Controllers/CartController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartContext _context;

        public CartController(CartContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] CartItemDto dto)
        {
            try
            {
                var cart = _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefault(c => c.UserId == dto.UserId);

                if (cart == null)
                {
                    cart = new Cart { UserId = dto.UserId };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }

                var existingItem = _context.CartItems
                    .FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == dto.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity += dto.Quantity;
                    _context.CartItems.Update(existingItem);
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity
                    };
                    _context.CartItems.Add(cartItem);
                }

                _context.SaveChanges();

                return Ok(new { message = "Product added to cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
