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
