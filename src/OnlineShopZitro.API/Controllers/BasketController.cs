using Microsoft.AspNetCore.Mvc;
using OnlineShopZitro.Application.DTOs;
using OnlineShopZitro.Application.Interfaces;

namespace OnlineShopZitro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketController> _logger;

    public BasketController(
        IBasketService basketService,
        ILogger<BasketController> logger)
    {
        _basketService = basketService;
        _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToBasket([FromBody] AddToBasketRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId) ||
                string.IsNullOrWhiteSpace(request.ProductId))
            {
                return BadRequest(new { success = false, message = "اطلاعات ورودی نامعتبر است" });
            }

            var result = await _basketService.AddToBasketAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در افزودن محصول به سبد خرید");
            return StatusCode(500, new { success = false, message = "خطای سرور" });
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasket(string userId)
    {
        try
        {
            var result = await _basketService.GetBasketAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"خطا در دریافت سبد خرید کاربر {userId}");
            return StatusCode(500, new { success = false, message = "خطای سرور" });
        }
    }
}