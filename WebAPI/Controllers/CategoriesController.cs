using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
	/// <summary>
	/// Categories controller.
	/// </summary>
	[ApiController]
	[Route("api/categories")]
	public class CategoriesController : Controller
	{
		private readonly ProductContext productContext;
		/// <summary>
		/// Controller constructor.
		/// </summary>
		/// <param name="productContext">database context</param>
		public CategoriesController(ProductContext productContext)
		{
			this.productContext = productContext;
		}



		/// <summary>
		///		Get a list of all categories.
		/// </summary>
		/// <returns>list of all categories</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/categories
		/// 
		///		Sample Response:
		///		[
		///			{
		///				"id": 1,
		///				"name": "Компьютерная техника"
		///			},
		///			{
		///				"id": 2,
		///				"name": "Офис и канцелярия"
		///			},
		///			{
		///				"id": 3,
		///				"name": "Мелкая бытовая техника"
		///			}
		///		]
		/// </remarks>
		/// <response code="200">Success</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IAsyncEnumerable<CategoryResponse> GetCategories()
		{
			return productContext.Categories.Select(c => c.ToCategoryResponse()).AsAsyncEnumerable();
		}



		/// <summary>
		///		Get a category by id.
		/// </summary>
		/// <param name="id">category id</param>
		/// <returns>category</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/categories/{id}
		/// 
		///		Sample Response:
		///		{
		///			"id": 2,
		///			"name": "Офис и канцелярия"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Category with this id was not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCategory(int id)
		{
			Category? category = await productContext.Categories.FindAsync(id);

			if (category == null)
				return NotFound();

			return Ok(category.ToCategoryResponse());
		}



		/// <summary>
		///		Save category.
		/// </summary>
		/// <param name="categoryRequest">category to save</param>
		/// <returns>saved category</returns>
		/// <remarks>
		///		Sample Request:
		///		POST /api/categories
		///		{
		///			"name": "newCategory"
		///		}
		///		
		///		Sample Response:
		///		{
		///			"id": 14,
		///			"name": "newCategory"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid body parameters</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SaveCategory([FromBody] CategoryRequest categoryRequest)
		{
			Category category = categoryRequest.ToCategory();
			await productContext.Categories.AddAsync(category);
			await productContext.SaveChangesAsync();
			return Ok(category.ToCategoryResponse());
		}



		/// <summary>
		///		Update category.
		/// </summary>
		/// <param name="category">category to update</param>
		/// <returns>updated category</returns>
		/// <remarks>
		///		Sample Request:
		///		PUT /api/categories
		///		{
		///			"id": 2,
		///			"name": "newCategory"
		///		}
		///		
		///		Sample Response:
		///		{
		///			"id": 2,
		///			"name": "newCategory"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid body parameters</response>
		/// <response code="404">Category with this id was not found</response>
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateCategory([FromBody] CategoryResponse category)
		{
			Category? foundCategory = await productContext.Categories.FindAsync(category.Id);

			if (foundCategory == null)
				return NotFound();

			//productContext.Categories.Update(category);
			productContext.Entry(foundCategory).CurrentValues.SetValues(category.ToCategory());
			await productContext.SaveChangesAsync();
			return Ok(category);
		}



		/// <summary>
		///		Patch category.
		/// </summary>
		/// <param name="id">category id</param>
		/// <param name="patchDoc">fields to patch</param>
		/// <returns>patched category</returns>
		/// <remarks>
		///		Sample Request:
		///		PATCH /api/categories/{id}
		///		[
		///			{
		///				"op": "replace",
		///				"path": "name",
		///				"value": "newCategoryName"
		///			}
		///		]
		///		
		///		Sample Response:
		///		{
		///			"id": 15,
		///			"name": "newCategoryName"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Category with this id was not found</response>
		/// <response code="500">Not valid body parameters</response>
		[HttpPatch("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> PatchCategory(int id, JsonPatchDocument<Category> patchDoc)
		{
			Category? category = await productContext.Categories.FindAsync(id);

			if (category == null)
				return NotFound();

			patchDoc.ApplyTo(category);
			await productContext.SaveChangesAsync();
			return Ok(category.ToCategoryResponse());
		}



		/// <summary>
		///		Delete category.
		/// </summary>
		/// <param name="id">category id</param>
		/// <returns>deleted category</returns>
		/// <remarks>
		///		Sample Request:
		///		DELETE /api/categories/{id}
		///		
		///		Sample Response:
		///		{
		///			"id": 14,
		///			"name": "CategoryName"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Category with this id was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			Category? category = await productContext.Categories.FindAsync(id);

			if (category == null)
				return NotFound();

			productContext.Categories.Remove(category);
			await productContext.SaveChangesAsync();
			return Ok(category.ToCategoryResponse());
		}
	}
}