using Entities;
using ServiceContracts.DTO;

namespace ServiceContracts.Extensions
{
    /// <summary>
    /// DTO conversions extension class
    /// </summary>
    public static class DtoConversions
    {
        /// <summary>
        /// Extension method to convert an object of entity model type BuyOrder to DTO BuyOrderResponse
        /// </summary>
        /// <param name="buyOrder">Parameter of type BuyOrder to convert</param>
        /// <returns>New BuyOrderResponse object</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol= buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price= buyOrder.Price,
                TradeAmount = buyOrder.Quantity* buyOrder.Price
            };
        }

        /// <summary>
        /// Extension method to convert an object of entity model type SellOrder to DTO SellOrderResponse
        /// </summary>
        /// <param name="buyOrder">Parameter of type SellOrder to convert</param>
        /// <returns>New SellOrderResponse object</returns>
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse
            {
                BuyOrderID = sellOrder.SellOrderID,
                StockName = sellOrder.StockName,
                StockSymbol = sellOrder.StockSymbol,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity,
                TradeAmount = sellOrder.Price* sellOrder.Quantity
            };
        }
    }
}