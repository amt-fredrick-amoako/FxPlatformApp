using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Extensions;
using Services.Helpers;
using System.Linq.Expressions;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;

        public StocksService(IStocksRepository context)
        {
            _stocksRepository = context;  
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
            await _stocksRepository.CreateBuyOrder(buyOrder);

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
            await _stocksRepository.CreateSellOrder(order);

            //convert order to sellOrderResponse type
            SellOrderResponse sellOrderResponse = order.ToSellOrderResponse();
            return sellOrderResponse;
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(order => order.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();
            return sellOrders.Select(order => order.ToSellOrderResponse()).ToList();
        }
    }
}
