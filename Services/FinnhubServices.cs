using ServiceContracts;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;

namespace Services
{
    public class FinnhubServices : IFinnhubServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubServices(IHttpClientFactory httpClientFactory,IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>> GetCompanyProfile(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();

            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Method= HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            HttpResponseMessage httpResponse = await httpClient.SendAsync( requestMessage );

            string responseBody = new StreamReader(httpResponse.Content.ReadAsStream()).ReadToEnd();

            Dictionary<string,object>? response = JsonSerializer.Deserialize<Dictionary<string,object>>(responseBody);

            if(response==null) throw new InvalidOperationException("No response from server");

            if (response.ContainsKey("error")) throw new InvalidOperationException(Convert.ToString(response["error"]));

            return response;
        }

        public async Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            HttpResponseMessage responseMessage = await httpClient.SendAsync( requestMessage );

            string responseBody = new StreamReader(responseMessage.Content.ReadAsStream()).ReadToEnd();

            Dictionary<string,object>? response = JsonSerializer.Deserialize<Dictionary<string,object>>(responseBody);

            if (response == null) throw new InvalidOperationException("No Response from Server");
            if (response.ContainsKey("error")) throw new InvalidOperationException(Convert.ToString(response["error"]));

            return response;
        }
    }
}
