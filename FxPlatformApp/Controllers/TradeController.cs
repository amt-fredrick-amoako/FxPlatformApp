using FxPlatformApp.Filters;
using FxPlatformApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
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
        private readonly ILogger<TradeController> _logger;

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
            IOptions<TradingOptions> tradingOptions,
            ILogger<TradeController> logger)
        {
            this.stocksService = stocksService;
            this.finnhubService = finnhubService;
            this.configuration = configuration;
            this.tradingOptions = tradingOptions.Value;
            _logger = logger;
        }

        //[Route("/")]
        //[Route("[action]")]
        [Route("[action]/{stockSymbol}")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            //log information
            _logger.LogInformation("{TradeController}.{Index}", nameof(TradeController), nameof(Index));
            _logger.LogInformation("In TradeController.Index() action method");

            //reset symbol if none exists
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";
            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await finnhubService
                .GetCompanyProfile(stockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await finnhubService
                .GetStockPriceQuote(stockSymbol);

            //model object
            StockTrade stockTrade = new StockTrade { StockSymbol = stockSymbol };

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
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            //invoke stockservice when modelstate has no errors
            BuyOrderResponse buyOrderResponse = await stocksService.CreateBuyOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        //TODO implement method
        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            SellOrderResponse sellOrderResponse = await stocksService.CreateSellOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        //TODO Implement method
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            //get list of orders
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await stocksService.GetBuyOrders());
            orders.AddRange(await stocksService.GetSellOrders());

            orders = orders.OrderByDescending(order => order.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Margins
                {
                    Top = 20,
                    Right = 20,
                    Left = 20,
                    Bottom = 20,
                },
                PageOrientation = Orientation.Portrait
            };
        }
    }
}
