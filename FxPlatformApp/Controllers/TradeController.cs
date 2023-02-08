using FxPlatformApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace FxPlatformApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions tradingOptions;
        private readonly IStocksService stocksService;
        private readonly IFinnhubService finnhubService;
        private readonly IConfiguration configuration;

        /// <summary>
        /// TradeController constructor that executes when a new object is created for
        /// the class
        /// </summary>
        /// <param name="stocksService">Inject an object of StockService</param>
        /// <param name="options">Inject an object of TradeOptions</param>
        /// <param name="finnhubService">Inject an object of FinnhubService</param>
        /// <param name="configuration">Inject an object of IConfiguration</param>
        public TradeController(IStocksService stocksService, IFinnhubService finnhubService, IConfiguration configuration, IOptions<TradingOptions> options)
        {
            this.stocksService = stocksService;
            this.finnhubService = finnhubService;
            this.configuration = configuration;
            tradingOptions = options.Value;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("~[controller]")]
        public async Task<IActionResult> Index()
        {
            //reset symbol if none exists
            if (string.IsNullOrEmpty(tradingOptions.DefaultStockSymbol))
                tradingOptions.DefaultStockSymbol = "MSFT";
            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await finnhubService.GetCompanyProfile(tradingOptions.DefaultStockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await finnhubService.GetStockPriceQuote(tradingOptions.DefaultStockSymbol);

            //model object
            StockTrade stockTrade = new StockTrade { StockSymbol = tradingOptions.DefaultStockSymbol};

            //load data from finnhubService into model object
            if(companyProfileDictionary!= null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade
                {
                    StockSymbol = companyProfileDictionary["ticker"].ToString(),
                    StockName = companyProfileDictionary["name"].ToString(),
                    Quantity = tradingOptions.DefaultOrderQuantity ?? 0, 
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()),
                };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = configuration["FinnhubToken"];

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
