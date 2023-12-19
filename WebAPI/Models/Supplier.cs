using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	/// <summary>
	/// Supplier class.
	/// </summary>
	public class Supplier
	{
		/// <summary>
		/// Supplier id.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Supplier name.
		/// </summary>
		[Required]
		public string Name { get; set; } = string.Empty;
		/// <summary>
		/// Supplier city.
		/// </summary>
		[Required]
		public string City { get; set; } = string.Empty;

		/// <summary>
		/// Products by supplier.
		/// </summary>
		public IEnumerable<Product>? Products { get; set; }

		/// <summary>
		/// Convert Supplier to SupplierResponse.
		/// </summary>
		/// <returns>SupplierResponse</returns>
		public SupplierResponse ToSupplierResponse()
		{
			return new SupplierResponse
			{
				Id = Id,
				Name = Name,
				City = City
			};
		}
	}
}