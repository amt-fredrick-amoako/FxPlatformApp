using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.FinnhubService
{
    public interface IFinnhubStocksService
    {
        /// <summary>
        /// Defines the method to get a list of Stocks
        /// </summary>
        /// <returns>returns a list of dictionaries of stocks</returns>
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}
