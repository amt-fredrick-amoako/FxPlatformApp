using FxPlatformApp.Controllers;
using FxPlatformApp.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace FxPlatformApp.Filters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public CreateOrderActionFilter()
        {}
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;
                if (orderRequest != null)
                {
                    //update date to the current date
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;
                    //perform model validations after updating the date
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);

                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values
                            .SelectMany(x => x.Errors)
                            .SelectMany(e => e.ErrorMessage);
                        StockTrade stockTrade = new StockTrade
                        {
                            StockName = orderRequest.StockName,
                            StockSymbol = orderRequest.StockSymbol,
                            Quantity = orderRequest.Quantity
                        };
                        context.Result = tradeController.View(nameof(TradeController.Index), stockTrade);
                    }
                    else
                    {
                        await next(); //invokes the subsequent filter or action method
                    }
                }
                else
                {
                    await next(); //invokes the subsequent filter or action method
                }
            }
            else
            {
                await next(); //calls the subsequent filter or action method
            }
        }
    }
}
