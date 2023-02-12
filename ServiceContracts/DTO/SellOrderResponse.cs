namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string StockSymbol { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }



        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not SellOrderResponse) return false;

            SellOrderResponse response = (SellOrderResponse)obj;
            return BuyOrderID == response.BuyOrderID
                && StockSymbol == response.StockSymbol
                && StockName == response.StockName
                && Price == response.Price
                && TradeAmount == response.TradeAmount;

        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Override the ToString method
        /// </summary>
        /// <returns>new string with</returns>
        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderID}, " +
                $"Stock Symbol {StockSymbol}," +
                $"Stock Name: {StockName}," +
                $"Date and Time of Buy Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}," +
                $"Quantity: {Quantity}," +
                $"Trade Amount {TradeAmount}";
        }
    }
}
