using Microsoft.AspNetCore.Mvc;

public class SentimentController : Controller
{
    private readonly SentimentAnalysisService _service = new();

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string opinion)
    {
        var result = _service.Predict(opinion);
        ViewBag.Resultado = result;
        return View();
    }
}
