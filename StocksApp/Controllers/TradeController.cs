using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        [Route("trade")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
