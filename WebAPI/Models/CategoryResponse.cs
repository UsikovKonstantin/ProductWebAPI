using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	/// <summary>
	/// Category class (for responses).
	/// </summary>
	public class CategoryResponse
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
		/// Convert CategoryResponse to Category.
		/// </summary>
		/// <returns>Category</returns>
		public Category ToCategory()
		{
			return new Category
			{
				Id = Id,
				Name = Name
			};
		}
	}
}