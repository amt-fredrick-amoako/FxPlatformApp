using FxPlatformApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace FxPlatformApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradeOptions;

        //DI for services
        public StocksController(IOptions<TradingOptions> options, IFinnhubService finnhubService)
        {
            _tradeOptions = options.Value;
            _finnhubService = finnhubService;
        }

        [Route("/")]
        [Route("[action]/{stock}")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            //get response from API server
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            if (stocksDictionary != null)
                if (!showAll && _tradeOptions.Top25PopularStocks is not null)
                {
                    //split strings by the "," delimiter into an array of strings
                    string[]? Top25PopularStocksList = _tradeOptions.Top25PopularStocks.Split(',');

                    if (Top25PopularStocksList is not null)
                        //check whether Top25PopularStocksList has the stock symbol
                        stocksDictionary = stocksDictionary
                            .Where(stock => Top25PopularStocksList.Contains(Convert.ToString(stock["symbol"])))
                            .ToList();
                }
            //convert dictionary objects into stock objects
            stocks = stocksDictionary.Select(stock => new Stock() 
            { 
                StockName = Convert.ToString(stock["description"]), 
                StockSymbol = Convert.ToString(stock["symbol"]) 
            }).ToList();

            ViewBag.stock = stock;

            return View(stocks);
        }
    }
}
