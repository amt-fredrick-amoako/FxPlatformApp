using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RepositoryContracts;
using ServiceContracts;

namespace FxPlatformApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SelectedStockViewComponent> _logger;

        public SelectedStockViewComponent(IOptions<TradingOptions> options, IFinnhubService finnhubService, IConfiguration configuration, ILogger<SelectedStockViewComponent> logger)
        {
            //inject business logic and configuration   
            _tradingOptions = options.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            //log viewComponent information
            _logger.LogInformation("ViewComponent Triggered");
            _logger.LogInformation("{SelectedStockViewComponent}.{InvokeAsync}", nameof(SelectedStockViewComponent), nameof(InvokeAsync));

            //initialize a dictionary
            Dictionary<string, object>? companyProfile = null;

            if (stockSymbol != null)
            {
                //store response from FinnhubService.GetCompanyProfile into company profile
                companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
                //store response from FinnhubService.GetCompanyProfile into company profile
                Dictionary<string, object>? stockPrice = await _finnhubService.GetStockPriceQuote(stockSymbol);
                if (companyProfile != null && stockPrice != null)
                {
                    companyProfile.Add("price", stockPrice["c"]);
                }
            }

            if (companyProfile != null && companyProfile.ContainsKey("logo"))
                return View(companyProfile);
            else
                return Content("");
        }
    }
}
