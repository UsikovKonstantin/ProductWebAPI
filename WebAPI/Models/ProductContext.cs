using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
	/// <summary>
	/// Database context class.
	/// </summary>
	public class ProductContext : DbContext
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="options">additional options</param>
		public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

		/// <summary>
		/// DbSet of suppliers.
		/// </summary>
		public DbSet<Supplier> Suppliers { get; set; }
		/// <summary>
		/// DbSet of categories.
		/// </summary>
		public DbSet<Category> Categories { get; set; }
		/// <summary>
		/// DbSet of products.
		/// </summary>
		public DbSet<Product> Products { get; set; }
	}
}