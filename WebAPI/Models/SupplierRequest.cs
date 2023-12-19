using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	/// <summary>
	/// Supplier class (for requests).
	/// </summary>
	public class SupplierRequest
	{
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
		/// Convert SupplierRequest to Supplier.
		/// </summary>
		/// <returns>Supplier</returns>
		public Supplier ToSupplier()
		{
			return new Supplier
			{
				Name = Name,
				City = City,
			};
		}
	}
}