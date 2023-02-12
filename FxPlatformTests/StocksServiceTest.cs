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

        #region CreateBuyOrderTests
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
            Assert.ThrowsAsync<ArgumentException>(async () =>
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
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"),
                Price = 1,
                Quantity = 1
            };

            //Act
            Assert.ThrowsAsync<ArgumentException>(
                async () =>
                {
                    //Assert
                    await _stocksService.CreateBuyOrder(buyOrderRequest);
                });


        }

        [Fact]
        public async void Create_Valid_BuyOrder()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-12-31"),
                Price = 1,
                Quantity = 1
            };

            //Act
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            //Assert
            Assert.NotEqual(Guid.Empty, buyOrderResponse.BuyOrderID);
        }

        #endregion

        #region CreateSellOrderTests

        [Fact]
        //Throws argumentnull exception when sell order request is supplied as null
        public void Create_SellOrder_Null_SellOrderRequest()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Act
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Assert
               await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(0)]
        //Throws argument exception when sellOrderQuantity is supplied as 0
        public void Create_SellOrder_With_SellOrderQuantityAs_0(uint sellOrderQuantity)
        {
            //Act
            SellOrderRequest sellOrderRequest = new()
            {
                Quantity = sellOrderQuantity,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
            };

            //Arrange
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }
        
        [Theory]
        [InlineData(100001)]
        //Throws argument exception when sellOrderQuantity is supplied as 100001
        public void Create_SellOrder_With_SellOrderQuantity_Greater_Than_Max(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new()
            {
                Quantity = sellOrderQuantity,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
            };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }
        
        [Theory]
        [InlineData(0)]
        //Throws argument exception when SellOrderPrice is supplied as 0
        public void Create_SellOrder_With_SellOrderPrice_Is_0(uint sellOrderPrice)
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new()
            {
                Quantity = 1,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = sellOrderPrice,
            };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }
        
        [Theory]
        [InlineData(100001)]
        //Throws argument exception when SellOrderPrice is supplied as 100001
        public void Create_SellOrder_With_SellOrderPrice_Greater_Than_Max(uint sellOrderPrice)
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new()
            {
                Quantity = 1,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = sellOrderPrice,
            };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        //It should throw an ArgumentException error when stock symbol is supplied as null
        public void CreateSellOrder_With_NullStockSymbol()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new()
            {
                Quantity = 1,
                StockSymbol = null,
                StockName = "Microsoft",
                Price = 1,
            };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        //It should throw ArgumentException when date and time of order is supplied as 1999-12-31
        public void CreateSellOrder_With_Older_Date_and_Time_Of_Order()
        {
            //Arrage
            SellOrderRequest sellOrderRequest = new()
            {
                Quantity = 1,
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"),
            };

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Assert
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        //It should not throw any exception when all values are valid
        public async void CreateSellOrder_With_Valid_Values()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                DateAndTimeOfOrder = Convert.ToDateTime("2000-12-31"),
                Quantity = 1,
            };

            //Assert
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            //Act
            Assert.NotEqual(Guid.Empty, sellOrderResponse.BuyOrderID);
        }



        #endregion
    }
}