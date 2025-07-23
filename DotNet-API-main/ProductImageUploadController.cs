
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ProductController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto dto)
        {
            try
            {
                string imagePath = "No Image Found";

                if (dto.ProdImage != null && dto.ProdImage.Length > 0)
                {
                    var uploads = Path.Combine(_environment.WebRootPath ?? "wwwroot", "images");
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ProdImage.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ProdImage.CopyToAsync(stream);
                    }

                    imagePath = $"/images/{fileName}";
                }

                // Simulate saving to database
                var product = new Product
                {
                    ProdName = dto.ProdName,
                    ProdPrice = dto.ProdPrice,
                    ProdDesc = dto.ProdDesc,
                    CategoryId = dto.CategoryId,
                    ProdImage = imagePath
                };

                // _context.Products.Add(product);
                // _context.SaveChanges();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ProductDto
    {
        [Required]
        public string ProdName { get; set; }

        [Required]
        public string ProdPrice { get; set; }

        public string? ProdDesc { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile? ProdImage { get; set; }
    }

    public class Product
    {
        public string ProdName { get; set; }
        public string ProdPrice { get; set; }
        public string? ProdDesc { get; set; }
        public int CategoryId { get; set; }
        public string? ProdImage { get; set; }
    }
}




// [HttpPost("upload")]
// public async Task<IActionResult> UploadFile(IFormFile formFile)
// {
//     if (formFile != null && formFile.Length > 0)
//     {
//         var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
//         var fileName = Path.GetRandomFileName();
//         var filePath = Path.Combine(uploadsPath, fileName);

//         using (var stream = System.IO.File.Create(filePath))
//         {
//             await formFile.CopyToAsync(stream);
//         }

//         return Ok("File uploaded!");
//     }

//     return BadRequest("Invalid file.");
// }

