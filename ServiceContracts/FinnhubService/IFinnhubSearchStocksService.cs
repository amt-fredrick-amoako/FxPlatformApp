using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.FinnhubService
{
    public interface IFinnhubSearchStocksService
    {
        /// <summary>
        /// Defines a method to search stocks
        /// </summary>
        /// <param name="stockSymbolToSearch"></param>
        /// <returns>returns search results</returns>
        Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch);
    }
}
