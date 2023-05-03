using Entities;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.Extensions;
using ServiceContracts.StocksService;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StockService
{
    public class BuyOrdersService : IBuyOrdersService
    {
        private readonly IStocksRepository _stocksRepository;

        public BuyOrdersService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            try
            {
                //validation: buyOrderRequest should not be null
                if (request == null) throw new ArgumentNullException(nameof(request));
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
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            try
            {
                List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
                return buyOrders.Select(order => order.ToBuyOrderResponse()).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
