using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System.Net.Http;
using System.Text.Json;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        /*
                public async Task<string> MakeRequest(string url, string? symbol = null)
                {
                    //create http client
                    HttpClient httpClient = _httpClientFactory.CreateClient();

                    //create http request
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{url}{symbol}&token={_configuration["FinnhubToken"]}")
                    };

                    //send request
                    HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

                    //read response body
                    string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

                    return responseBody;
                }*/

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
        {
            //create http client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //create http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            //send request
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            //read response body
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            //convert response body from JSON to Dictionary
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");
            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            //return dictionary to the caller
            return responseDictionary;

        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol)
        {
            //create http client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //create http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            //send request
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            //read response body
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            //convert response body from JSON into Dictionary
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDictionary == null) throw new InvalidOperationException();
            if (responseDictionary.ContainsKey("error")) throw new InvalidOperationException($"{responseDictionary["error"]}");

            //return response dictionary to the caller
            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            //create http client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //create a request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}")
            };

            //send request
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            //read from response body
            string responseBody = await new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEndAsync();

            List<Dictionary<string, string>>? responseDictionaryList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);



            if (responseDictionaryList != null) return responseDictionaryList;
            throw new InvalidOperationException("No response from server");

        }

        public async Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch)
        {
            //create client
            HttpClient client = _httpClientFactory.CreateClient();

            //create request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FinnhubToken"]}")
            };

            //send request
            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);

            //read response
            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            //serialize into dictionary
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response) ?? throw new InvalidOperationException("No response from server");
            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
            return responseDictionary;


        }
    }
}