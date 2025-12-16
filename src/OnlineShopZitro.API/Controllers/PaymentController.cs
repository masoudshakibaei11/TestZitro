using Microsoft.AspNetCore.Mvc;
using OnlineShopZitro.Application.DTOs;
using OnlineShopZitro.Application.Interfaces;

namespace OnlineShopZitro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(
        IPaymentService paymentService,
        ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartPayment([FromBody] StartPaymentRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                return BadRequest(new { success = false, message = "UserId Is Requred" });
            }

            var result = await _paymentService.StartPaymentAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Payment");
            return StatusCode(500, new { success = false, message = "InternalServerErore" });
        }
    }
}