using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Extensions;
using Services;

namespace FxPlatformTests.ServiceTests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;
        private readonly IFixture _fixture;

        public StocksServiceTest()
        {
            _fixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;
            _stocksService = new StocksService(_stocksRepository);
        }
        #region CreateBuyOrder


        [Fact]
        public async void CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Mock
            BuyOrder buyOrder = _fixture.Build<BuyOrder>().Create();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act


            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //action should be expected
            await action.Should().ThrowAsync<ArgumentNullException>();
        }



        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public void CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);


            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public void CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public void CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint buyOrderPrice)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, buyOrderPrice)
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () => { await _stocksService.CreateBuyOrder(buyOrderRequest); };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public void CreateBuyOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderPrice)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, buyOrderPrice)
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () => { await _stocksService.CreateBuyOrder(buyOrderRequest); };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().With(temp => temp.StockSymbol, null as string)
                .Create();
            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () => { await _stocksService.CreateBuyOrder(buyOrderRequest); };
            action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public void CreateBuyOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () => { await _stocksService.CreateBuyOrder(buyOrderRequest); };
            action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse buyOrderResponseFromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);

            buyOrder.BuyOrderID = buyOrderResponseFromCreate.BuyOrderID;

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();
            //Assert

            //actual should be expected
            buyOrderResponseFromCreate.Should().NotBe(Guid.Empty);
            buyOrderResponseFromCreate.Should().Be(buyOrderResponse);
        }


        #endregion




        #region CreateSellOrder


        [Fact]
        public void CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;
            //Mock
            SellOrder sellOrder = _fixture.Build<SellOrder>().Create();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert, action should be expected
            action.Should().ThrowAsync<ArgumentNullException>();
        }



        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public void CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, sellOrderQuantity)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public void CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, sellOrderQuantity)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public void CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint sellOrderPrice)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, sellOrderPrice)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public void CreateSellOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderPrice)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, sellOrderPrice)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public void CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>().
                With(temp => temp.StockSymbol, null as string)
                .Create();
            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);


            //Act
            Func<Task> action = async () => { await _stocksService.CreateSellOrder(sellOrderRequest); };
            action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public void CreateSellOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>().
                With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                .Create();
            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);


            //Act
            Func<Task> action = async () => { await _stocksService.CreateSellOrder(sellOrderRequest); };
            action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async void CreateSellOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, "MSFT")
                .With(temp => temp.StockName, "Microsoft")
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("2024-12-31"))
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);


            //Act
            SellOrderResponse sellOrderResponseFromCreate = await _stocksService.CreateSellOrder(sellOrderRequest);
            sellOrder.SellOrderID = sellOrderResponseFromCreate.SellOrderID;
            SellOrderResponse sellOrderResponseExpected = sellOrder.ToSellOrderResponse();


            //Assert
            sellOrderResponseFromCreate.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrderResponseFromCreate.Should().Be(sellOrderResponseExpected);
        }


        #endregion




        #region GetBuyOrders

        //The GetAllBuyOrders() should return an empty list by default
        [Fact]
        public async void GetAllBuyOrders_DefaultList_ToBeEmpty()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrdersFromGet = await _stocksService.GetBuyOrders();

            //Assert
            buyOrdersFromGet.Should().BeEmpty();
        }


        [Fact]
        public async void GetAllBuyOrders_WithFewBuyOrders_ToBeSuccessful()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>
            {
                _fixture.Build<BuyOrder>().Create(),
                _fixture.Build<BuyOrder>().Create(),
            };

            List<BuyOrderResponse> buyOrderResponsesExpected = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
            List<BuyOrderResponse> buyOrderResponsesFromAdd = new List<BuyOrderResponse>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> buyOrderReponsesFromGet = await _stocksService.GetBuyOrders();

            //Assert
            buyOrderReponsesFromGet.Should().BeEquivalentTo(buyOrderResponsesExpected);
        }

        #endregion




        #region GetSellOrders

        //The GetAllSellOrders() should return an empty list by default
        [Fact]
        public async void GetAllSellOrders_DefaultList_ToBeEmpty()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            List<SellOrderResponse> sellOrdersFromGet = await _stocksService.GetSellOrders();

            //Assert
            sellOrdersFromGet.Should().BeEmpty();
        }


        [Fact]
        public async void GetAllSellOrders_WithFewSellOrders_ToBeSuccessful()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>
            {
                _fixture.Build<SellOrder>().Create(),
                _fixture.Build<SellOrder>().Create(),
            };

            List<SellOrderResponse> sellOrderResponsesExpected = sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
            List<SellOrderResponse> sellOrderResponsesFromAdd = new List<SellOrderResponse>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> sellOrderReponsesFromGet = await _stocksService.GetSellOrders();

            //Assert
            sellOrderReponsesFromGet.Should().BeEquivalentTo(sellOrderResponsesExpected);
        }

        #endregion

    }
}

