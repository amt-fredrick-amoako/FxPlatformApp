using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.StocksService
{
    public interface ISellOrdersService
    {
        /// <summary>
        /// Takes a SellOrderRequest object and returns a SellOrderResponse
        /// </summary>
        /// <param name="request">SellOrderRequest parameter</param>
        /// <returns>New object of a SellOrderResponse</returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrder);

        /// <summary>
        /// Get all SellOrders in a form of a SellOrderResponse object type
        /// </summary>
        /// <returns>A list of SellOrderResponse</returns>
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
