using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using RepositoryContracts;
using ServiceContracts;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
        {
            
            Dictionary<string, object?> responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            //return dictionary to the caller
            return responseDictionary;

        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol)
        {
           Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            //return response dictionary to the caller
            return responseDictionary;
        }

        public async Task<List<Dictionary<string,string>>?> GetStocks()
        {
            List<Dictionary<string, string>>? responseDictionary = await _finnhubRepository.GetStocks();

            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            return responseDictionary;
        }
    }
}