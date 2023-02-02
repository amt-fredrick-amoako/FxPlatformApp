using FxPlatformApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FxPlatformApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        [Route("/")]
        [Route("[action]")]
        [Route("~[controller]")]
        public IActionResult Index(StockTrade stockTrade)
        {
            return View(stockTrade);
        }

        [Route("[action]")]
        public IActionResult Orders()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult OrdersPDF()
        {
            return View();
        }
    }
}
