namespace ServiceContracts
{
    public interface IFinnhubServices 
    {
        Task<Dictionary<string, object>>? GetCompanyProfile(string stockSymbol);
        Task<Dictionary<string, object>>? GetStockPriceQuote(string stockSymbol);
    }
}