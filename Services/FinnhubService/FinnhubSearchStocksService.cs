﻿using Exceptions;
using RepositoryContracts;
using ServiceContracts.FinnhubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FinnhubService
{
    public class FinnhubSearchStocksService : IFinnhubSearchStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubSearchStocksService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch)
        {
            try
			{
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

                return responseDictionary;
            }
			catch (Exception ex)
			{
                FinnhubException finnhubException = new("Unable to connect to finnhub", ex);
				throw finnhubException;
			};
        }
    }
}
