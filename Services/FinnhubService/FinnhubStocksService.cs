using Exceptions;
using RepositoryContracts;
using ServiceContracts.FinnhubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FinnhubService
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubStocksService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
			{
                List<Dictionary<string, string>>? responseDictionary = await _finnhubRepository.GetStocks();

                return responseDictionary;
            }
			catch (Exception ex)
			{

                FinnhubException finnhubException = new("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }
    }
}
