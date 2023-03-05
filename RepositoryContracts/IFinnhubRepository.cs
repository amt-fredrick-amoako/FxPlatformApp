namespace RepositoryContracts
{
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Defines a method that gets profile of a company
        /// </summary>
        /// <param name="stockSymbol">string parameter of the symbol of the company profile required</param>
        /// <returns>a dictionary with a string and object as the key value pair</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol);

        /// <summary>
        /// Defines a method that gets current price stock
        /// </summary>
        /// <param name="stockSymbol">string parameter of the symbol of theprice stock required</param>
        /// <returns>a dictionary with a string and object as the key value pair</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol);

        /// <summary>
        /// Defines the method to get a list of Stocks
        /// </summary>
        /// <returns>returns a list of dictionaries of stocks</returns>
        Task<List<Dictionary<string, string>>?> GetStocks();

        /// <summary>
        /// Defines a method to search stocks
        /// </summary>
        /// <param name="stockSymbolToSearch"></param>
        /// <returns>returns search results</returns>
        Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch);
    }
}