using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddToCartController : ControllerBase
    {
        private readonly CartContext _context;

        public AddToCartController(CartContext context)
        {
            _context = context;
        }

        [HttpPost("cart/add")]
        public IActionResult AddToCart([FromBody] CartItemDto dto)
        {
            try
            {
                
                var existing = _context.Carts
                    .FirstOrDefault(c => c.UserId == dto.UserId && c.ProductId == dto.ProductId);

                if (existing != null)
                {
                    existing.Quantity += dto.Quantity;
                }
                else
                {
                    _context.Carts.Add(new Cart
                    {
                        UserId = dto.UserId,
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity
                    });
                }

                _context.SaveChanges();
                return Ok("Product added to cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }


}
