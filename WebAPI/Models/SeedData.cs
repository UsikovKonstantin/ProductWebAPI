using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace Database.Models
{
	/// <summary>
	/// Class for filling the database.
	/// </summary>
    public class SeedData
    {
        private readonly ProductContext context;
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="context">database context</param>
		public SeedData(ProductContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Method for filling the database.
		/// </summary>
		public void SeedDatabase()
		{
			// Apply migrations if there are any.
			context.Database.Migrate();

			if (context.Categories.Count() == 0 &&
				context.Suppliers.Count() == 0 &&
				context.Products.Count() == 0)
			{
				Category c1 = new Category { Name = "Компьютерная техника" };
				Category c2 = new Category { Name = "Офис и канцелярия" };
				Category c3 = new Category { Name = "Мелкая бытовая техника" };

				Supplier s1 = new Supplier { Name = "Calve", City = "Moscow" };
				Supplier s2 = new Supplier { Name = "TESCOMA", City = "Tver" };
				Supplier s3 = new Supplier { Name = "Haier", City = "Berlin" };
				Supplier s4 = new Supplier { Name = "Nescafe", City = "Paris" };
				Supplier s5 = new Supplier { Name = "Be quiet", City = "Barcelona" };

				Product p1 = new Product { Name = "Кухонный комбайн KitchenAid 5KSM156", Price = 1, Category = c3, Supplier = s3 };
				Product p2 = new Product { Name = "Видеокарта Asus GeForce GT 1030", Price = 1, Category = c1, Supplier = s1 };
				Product p3 = new Product { Name = "Ноутбук HP ENVY 13-ad000", Price = 1, Category = c1, Supplier = s2 };
				Product p4 = new Product { Name = "Фен Dewal 03-401", Price = 1, Category = c3, Supplier = s5 };
				Product p5 = new Product { Name = "Кофеварка Gastrorag CM-717", Price = 1, Category = c3, Supplier = s4 };

				context.Categories.AddRange(c1, c2, c3);
				context.Suppliers.AddRange(s1, s2, s3, s4, s5);
				context.Products.AddRange(p1, p2, p3, p4, p5);
			}

			context.SaveChanges();
		}
		
    }
}