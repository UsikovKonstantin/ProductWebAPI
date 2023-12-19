using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	/// <summary>
	/// Category class (for requests).
	/// </summary>
	public class CategoryRequest
	{
		/// <summary>
		/// Category name.
		/// </summary>
		[Required]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Convert CategoryRequest to Category.
		/// </summary>
		/// <returns>Category</returns>
		public Category ToCategory()
		{
			return new Category
			{
				Name = Name
			};
		}
	}
}