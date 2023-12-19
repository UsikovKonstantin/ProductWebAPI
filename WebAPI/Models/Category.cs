using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	/// <summary>
	/// Category class.
	/// </summary>
	public class Category
	{
		/// <summary>
		/// Category id.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Category name.
		/// </summary>
		[Required]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Products by category.
		/// </summary>
		public IEnumerable<Product>? Products { get; set; }

		/// <summary>
		/// Convert Category to CategoryResponse.
		/// </summary>
		/// <returns>CategoryResponse</returns>
		public CategoryResponse ToCategoryResponse()
		{
			return new CategoryResponse
			{
				Id = Id,
				Name = Name,
			};
		}
	}
}