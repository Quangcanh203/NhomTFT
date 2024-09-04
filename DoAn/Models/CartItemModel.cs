namespace DoAn.Models
{
	public class CartItemModel
	{
		public long ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantily {  get; set; }
		public decimal Price { get; set; }
		public decimal Total
		{
			get
			{
				return Quantily*Price;
			}
		}
		public string Image {  get; set; }
		public CartItemModel()
		{

		}
		public CartItemModel(ProductModel product)
		{
			ProductId = product.Id;
			ProductName = product.Name;
			Price = product.Price;
			Quantily = 1;
			Image = product.Image;
		}
	}
}
