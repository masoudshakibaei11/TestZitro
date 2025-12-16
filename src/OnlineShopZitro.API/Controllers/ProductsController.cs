using Microsoft.AspNetCore.Mvc;
using OnlineShopZitro.Application.Interfaces;

namespace OnlineShopZitro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(new
            {
                success = true,
                data = products
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در دریافت لیست محصولات");
            return StatusCode(500, new { success = false, message = "خطای سرور" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { success = false, message = "محصول یافت نشد" });

            return Ok(new { success = true, data = product });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"خطا در دریافت محصول {id}");
            return StatusCode(500, new { success = false, message = "خطای سرور" });
        }
    }
}
