using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
	/// <summary>
	/// Product class.
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Product id.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Product name.
		/// </summary>
		[Required]
		public string Name { get; set; } = string.Empty;
		/// <summary>
		/// Product price.
		/// </summary>
		[Range(1, 100000)]
		[Column(TypeName = "decimal(8, 2)")]
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
		/// Supplier of the product.
		/// </summary>
		public Supplier? Supplier { get; set; }
		/// <summary>
		/// Category of the product.
		/// </summary>
		public Category? Category { get; set; }

		/// <summary>
		/// Convert Product to ProductResponse.
		/// </summary>
		/// <returns>ProductResponse</returns>
		public ProductResponse ToProductResponse()
		{
			return new ProductResponse
			{
				Id = Id,
				Name = Name,
				Price = Price,
				SupplierId = SupplierId,
				CategoryId = CategoryId
			};
		}
	}
}