using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public TradeController(IConfiguration configuration, FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _configuration = configuration;
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }

        [Route("trade")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);
            Dictionary<string, object>? stockPriceDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

            StockTrade companyProfile = new StockTrade()
            {
                StockSymbol = Convert.ToString(companyProfileDictionary["ticker"].ToString()),
                StockName = Convert.ToString(companyProfileDictionary["name"].ToString()),
                Price = Convert.ToDouble(stockPriceDictionary["c"].ToString())
            };

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(companyProfile);
        }
    }
}
