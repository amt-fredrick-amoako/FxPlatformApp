using ServiceContracts.DTO;

namespace FxPlatformApp.Models
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; }
        public List<SellOrderResponse> SellOrders { get; set; }
    }
}
