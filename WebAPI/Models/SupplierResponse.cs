using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	/// <summary>
	/// Supplier class (for responses).
	/// </summary>
	public class SupplierResponse
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
		/// Convert SupplierResponse to Supplier.
		/// </summary>
		/// <returns>Supplier</returns>
		public Supplier ToSupplier()
		{
			return new Supplier
			{
				Id = Id,
				Name = Name,
				City = City
			};
		}
	}
}