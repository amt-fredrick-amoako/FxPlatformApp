using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Diagnostics;

namespace FxPlatformTests
{
    public class StocksServiceTest
    {
        //private members
        private readonly IStocksService _stocksService;

        public StocksServiceTest()
        {
            _stocksService = new StocksService();
        }

        [Fact]
        public void BuyOrderRequest_Is_Null()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;
            //Act
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Assert
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });

        }

        [Theory]
        [InlineData(0)]
        //Throws ArgumentException when buy order quantity is supplied as 0
        public void BuyOrder_Quantity_is_Zero(uint quantity)
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = quantity };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(100001)]
        //Throws ArgumentException when buy order quantity is supplied as 0
        public void BuyOrder_Quantity_is_Greater_Than_Max(uint quantity)
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = quantity };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(0)]
        //Throws ArgumentException when buy order price is zero
        public void BuyOrderPrice_Is_Supplied_As_Zero(uint price)
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest { StockName = "Microsoft", StockSymbol = "MSFT", Price = price, Quantity = 1 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(100001)]
        public void BuyOrderPrice_Is_Greater_Than_Max(uint price)
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest { StockName = "Microsoft", StockSymbol = "MSFT", Price = price, Quantity = 1 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                //Assert
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        //Throws argument exception when stocksymbol is supplied as null
        public void BuyOrder_StockSymbol_Is_Supplied_As_Null()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest { StockName = "Microsoft", StockSymbol = null, Price = 1, Quantity = 1 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        //Throws argument exception when date time order is supplied to be older than 2000-01-01
        public void DateTimeOfOrder_Is_Less_Than_Year_2000()
        {
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest { };
        }
    }
}