using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.FinnhubService
{
    public interface IFinnhubStockPriceQuoteService
    {
        /// <summary>
        /// Defines a method that gets current price stock
        /// </summary>
        /// <param name="stockSymbol">string parameter of the symbol of theprice stock required</param>
        /// <returns>a dictionary with a string and object as the key value pair</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol);
    }
}
