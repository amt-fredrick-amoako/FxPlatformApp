using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Extensions;
using Services.Helpers;
using System.Linq.Expressions;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StocksService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }



        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            //validation: buyOrderRequest should not be null
            if(request == null) throw new ArgumentNullException(nameof(request));
            //model validation
            ValidationHelpers.ModelValidation(request);

            //convert buyOrderRequest into BuyOrder type
            BuyOrder buyOrder = request.ToBuyOrder();
            //generate BuyOrderID
            buyOrder.BuyOrderID = Guid.NewGuid();
            //add buy order object to the orders list
            _buyOrders.Add(buyOrder);

            //convert and return buyOrder as BuyOrderResponse type
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrder)
        {
            //validation: sellOrderRequest should not be null
            if(sellOrder == null) throw new ArgumentException(nameof(sellOrder));
            //model validation using validation helper
            ValidationHelpers.ModelValidation(sellOrder);

            //convert sellOrderRequest into SellOrder type
            SellOrder order = sellOrder.ToSellOrder();

            //generate a new guid for the order id
            order.SellOrderID = Guid.NewGuid();

            //add order to sellOders list
            _sellOrders.Add(order);

            //convert order to sellOrderResponse type
            SellOrderResponse sellOrderResponse = order.ToSellOrderResponse();
            return sellOrderResponse;
        }

        public Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<BuyOrderResponse>> GetAllSellOrders()
        {
            throw new NotImplementedException();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return _buyOrders
                .OrderByDescending(buyOrder => buyOrder.DateAndTimeOfOrder)
                .Select(buyOrder => buyOrder.ToBuyOrderResponse())
                .ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return _sellOrders
                .OrderByDescending(sellOrder => sellOrder.DateAndTimeOfOrder)
                .Select(sellOrder => sellOrder.ToSellOrderResponse())
                .ToList();
        }
    }
}
