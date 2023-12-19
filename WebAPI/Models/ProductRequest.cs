using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	/// <summary>
	/// Product class (for requests).
	/// </summary>
	public class ProductRequest
	{
		/// <summary>
		/// Product name.
		/// </summary>
		[Required]
		public string Name { get; set; } = string.Empty;
		/// <summary>
		/// Product price.
		/// </summary>
		[Range(1, 100000)]
		public decimal Price { get; set; }
		/// <summary>
		/// Id of supplier.
		/// </summary>
		[Range(1, int.MaxValue)]
		public int SupplierId { get; set; }
		/// <summary>
		/// Id of category.
		/// </summary>
		[Range(1, int.MaxValue)]
		public int CategoryId { get; set; }

		/// <summary>
		/// Convert ProductRequest to Product.
		/// </summary>
		/// <returns>Product</returns>
		public Product ToProduct()
		{
			return new Product
			{
				Name = Name,
				Price = Price,
				SupplierId = SupplierId,
				CategoryId = CategoryId
			};
		}
	}
}