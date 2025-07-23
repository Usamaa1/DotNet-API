using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly CartContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public ProductController(CartContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            _context = context;
            _config = config;
            _environment = environment;
        }


        [HttpPost("category")]
        public IActionResult addCategory(Category category)
        {

            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return Ok("Category Added");
            }
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> addProduct(ProductForm dto)
        {

            try
            {
                string imagePath = "No Image Found";

                if (dto.ProdImage != null && dto.ProdImage.Length > 0)
                {
                    // Get folder path from config
                    var uploadsPath = _config["StoredFilesPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    var extension = Path.GetExtension(dto.ProdImage.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadsPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await dto.ProdImage.CopyToAsync(stream);
                    }

                    imagePath = $"/uploads/{fileName}";
                }

                var product = new Product
                {
                    ProdName = dto.ProdName,
                    ProdPrice = dto.ProdPrice,
                    ProdDesc = dto.ProdDesc,
                    CategoryId = dto.CategoryId,
                    ProdImage = imagePath
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("catProd")]
        public IActionResult getProductsWithCategory()
        {
            var data = _context.Products
        .Include(p => p.Category)
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            ProdName = p.ProdName,
            ProdPrice = p.ProdPrice,
            ProdDesc = p.ProdDesc,
            ProdImage = p.ProdImage,
            CategoryId = p.CategoryId,
            CategoryName = p.Category != null ? p.Category.CategoryName : "Unknown"
        })
        .ToList();

            return Ok(data);
        }





    }
}
