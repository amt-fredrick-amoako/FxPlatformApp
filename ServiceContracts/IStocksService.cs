using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IStocksService
    {
        /// <summary>
        /// Takes a BuyOrderRequest object and returns a BuyOrderResponse
        /// </summary>
        /// <param name="request">BuyOrderRequest parameter</param>
        /// <returns>New object of a BuyOrderResponse</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request);

        /// <summary>
        /// Get all BuyOrders in a form of a BuyOrderResponse object type
        /// </summary>
        /// <returns>A list of BuyOrderResponse</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders();

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
