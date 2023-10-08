using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using StockApp.Models;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFinnhubServices _finnhubServices;
//        private readonly TradingOptions _tradeOptions;
        public string DefaultStockSymbol = "MSFT";

        public TradeController(IConfiguration configuration,IFinnhubServices finnhubServices)
        {
            _configuration = configuration;
            _finnhubServices = finnhubServices;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        public async Task<ActionResult> Index()
        {
            if (string.IsNullOrEmpty(DefaultStockSymbol)) DefaultStockSymbol = "MSFT";

            Dictionary<string, object>? companyProfile = await _finnhubServices.GetCompanyProfile(DefaultStockSymbol);
            Dictionary<string, object>? stockQuote = await _finnhubServices.GetStockPriceQuote(DefaultStockSymbol);
            
            StockTrade stockTrade = new StockTrade(){StockSymbol = DefaultStockSymbol};

            if(companyProfile != null && stockQuote != null)
            {
                stockTrade.StockName = Convert.ToString(companyProfile["name"]);
                stockTrade.Price = Convert.ToDouble(stockQuote["c"].ToString());
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }
    }
}
