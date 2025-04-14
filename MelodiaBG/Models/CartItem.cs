namespace MelodiaBG.Models
{
    public class CartItem
    {
        public int InstrumentId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
