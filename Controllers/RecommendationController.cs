using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class RecommendationController : Controller
{
    private readonly ProductRecommendationService _service = new();

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string userId)
    {
        var allProducts = new List<string> { "P1", "P2", "P3", "P4", "P5" };
        var result = _service.RecommendProducts(userId, allProducts);
        ViewBag.Resultado = result;
        return View();
    }
}
