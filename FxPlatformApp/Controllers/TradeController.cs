using FxPlatformApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;

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
        /// <param name="tradingOptions">Inject an object of TradeOptions</param>
        /// <param name="finnhubService">Inject an object of FinnhubService</param>
        /// <param name="configuration">Inject an object of IConfiguration</param>
        public TradeController(IStocksService stocksService,
            IFinnhubService finnhubService,
            IConfiguration configuration,
            IOptions<TradingOptions> tradingOptions)
        {
            this.stocksService = stocksService;
            this.finnhubService = finnhubService;
            this.configuration = configuration;
            this.tradingOptions = tradingOptions.Value;
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
            Dictionary<string, object>? companyProfileDictionary = await finnhubService
                .GetCompanyProfile(tradingOptions.DefaultStockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await finnhubService
                .GetStockPriceQuote(tradingOptions.DefaultStockSymbol);

            //model object
            StockTrade stockTrade = new StockTrade { StockSymbol = tradingOptions.DefaultStockSymbol };

            //load data from finnhubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
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

        //TODO Implement method
        [Route("[action]")]

        public async Task<IActionResult> Orders()
        {
            //Invoke methods in the stockservice
            List<BuyOrderResponse> buyOrders = await stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await stocksService.GetSellOrders();

            //create a model object
            Orders orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };

            ViewBag.TradingOptions = tradingOptions;

            return View(orders);
        }

        //TODO Implement method
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            //update date of order
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //validate model after updating the date
            ModelState.Clear();
            TryValidateModel(buyOrderRequest);

            //when modelstate is invalid
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade
                {
                    StockName = buyOrderRequest.StockName,
                    StockSymbol = buyOrderRequest.StockSymbol,
                    Quantity = buyOrderRequest.Quantity,

                };

                return View("Index", stockTrade);
            }

            //invoke stockservice when modelstate has no errors
            BuyOrderResponse buyOrderResponse = await stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        //TODO implement method
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            //initialize the current time of sellOrder
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //validate the model after updating the date time
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);


            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                   .SelectMany(value => value.Errors)
                   .Select(error => error.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade
                {
                    StockName = sellOrderRequest.StockName,
                    StockSymbol = sellOrderRequest.StockSymbol,
                    Quantity = sellOrderRequest.Quantity,

                };

                return View("Index", stockTrade);


            }

            SellOrderResponse sellOrderResponse = await stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        //TODO Implement method
        [Route("[action]")]
        public IActionResult OrdersPDF()
        {
            return View();
        }
    }
}
