using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Extensions;
using Services.Helpers;
using System.Linq.Expressions;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StockMarketDbContext _db;

        public StocksService(StockMarketDbContext context)
        {
            _db = context;  
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
            _db.Add(buyOrder);
            await _db.SaveChangesAsync();

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
            _db.Add(order);
            await _db.SaveChangesAsync();

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
            List<BuyOrder> buyOrders = await _db.BuyOrders
                .OrderByDescending(order => order.DateAndTimeOfOrder)
                .ToListAsync();
            return buyOrders.Select(order => order.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _db.SellOrders
                .OrderByDescending(order =>  order.DateAndTimeOfOrder)
                .ToListAsync();
            return sellOrders.Select(order => order.ToSellOrderResponse()).ToList();
        }
    }
}
