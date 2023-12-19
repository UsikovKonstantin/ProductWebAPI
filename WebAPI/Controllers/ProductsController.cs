using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
	/// <summary>
	/// Products controller.
	/// </summary>
	[ApiController]
	[Route("api/products")]
	public class ProductsController : Controller
	{
		private readonly ProductContext productContext;
		/// <summary>
		/// Controller constructor.
		/// </summary>
		/// <param name="productContext">database context</param>
		public ProductsController(ProductContext productContext)
        {
			this.productContext = productContext;
		}



		/// <summary>
		///		Get a list of all products.
		/// </summary>
		/// <returns>list of all products</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/products
		/// 
		///		Sample Response:
		///		[
		///			{
		///				"id": 1,
		///				"name": "Кухонный комбайн KitchenAid 5KSM156",
		///				"price": 123.00,
		///				"supplierId": 3,
		///				"categoryId": 3
		///			},
		///			{
		///				"id": 2,
		///				"name": "Видеокарта Asus GeForce GT 1030",
		///				"price": 456.00,
		///				"supplierId": 1,
		///				"categoryId": 1
		///			},
		///			{
		///				"id": 3,
		///				"name": "Ноутбук HP ENVY 13-ad000",
		///				"price": 789.00,
		///				"supplierId": 2,
		///				"categoryId": 1
		///			}
		///		]
		/// </remarks>
		/// <response code="200">Success</response>
		[HttpGet]
		public IAsyncEnumerable<ProductResponse> GetProducts() 
		{
			return productContext.Products.Select(p => p.ToProductResponse()).AsAsyncEnumerable();
		}



		/// <summary>
		///		Get a list of products by category id.
		/// </summary>
		/// <param name="id">category id</param>
		/// <returns>list of products</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/products/category/{id}
		/// 
		///		Sample Response:
		///		[
		///			{
		///				"id": 1,
		///				"name": "Кухонный комбайн KitchenAid 5KSM156",
		///				"price": 1.00,
		///				"supplierId": 3,
		///				"categoryId": 3
		///			},
		///			{
		///				"id": 4,
		///				"name": "Фен Dewal 03-401",
		///				"price": 1.00,
		///				"supplierId": 5,
		///				"categoryId": 3
		///			}
		///		]
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Category with this id was not found</response>
		[HttpGet("category/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetProductsByCategoryId(int id)
		{
			Category? category = await productContext.Categories.FindAsync(id);

			if (category == null)
				return NotFound();

			return Ok(productContext.Products.Where(p => p.CategoryId == id).Select(p => p.ToProductResponse()));
		}



		/// <summary>
		///		Get a list of products by supplier id.
		/// </summary>
		/// <param name="id">supplier id</param>
		/// <returns>list of products</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/products/supplier/{id}
		/// 
		///		Sample Response:
		///		[
		///			{
		///				"id": 3,
		///				"name": "Ноутбук HP ENVY 13-ad000",
		///				"price": 1.00,
		///				"supplierId": 2,
		///				"categoryId": 1
		///			},
		///			{
		///				"id": 27,
		///				"name": "Кухонный комбайн KitchenAid 5KSM156",
		///				"price": 1000.00,
		///				"supplierId": 2,
		///				"categoryId": 3
		///			}
		///		]
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Supplier with this id was not found</response>
		[HttpGet("supplier/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetProductsBySupplierId(int id)
		{
			Supplier? supplier = await productContext.Suppliers.FindAsync(id);

			if (supplier == null)
				return NotFound();

			return Ok(productContext.Products.Where(p => p.SupplierId == id).Select(p => p.ToProductResponse()));
		}



		/// <summary>
		///		Get a product by id.
		/// </summary>
		/// <param name="id">product id</param>
		/// <returns>product</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/products/{id}
		/// 
		///		Sample Response:
		///		{
		///			"id": 2,
		///			"name": "Видеокарта Asus GeForce GT 1030",
		///			"price": 4567.00,
		///			"supplierId": 1,
		///			"categoryId": 1
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Product with this id was not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetProduct(int id)
		{
			Product? product = await productContext.Products.FindAsync(id);

			if (product == null)
				return NotFound();

			return Ok(product.ToProductResponse());
		}



		/// <summary>
		///		Save product.
		/// </summary>
		/// <param name="productRequest">product to save</param>
		/// <returns>saved product</returns>
		/// <remarks>
		///		Sample Request:
		///		POST /api/products
		///		{
		///			"name": "Видеокарта Asus GeForce GT 1030",
		///			"price": 1.00,
		///			"supplierId": 1,
		///			"categoryId": 1
		///		}
		///		
		///		Sample Response:
		///		{
		///			"id": 21,
		///			"name": "Видеокарта Asus GeForce GT 1030",
		///			"price": 1.00,
		///			"supplierId": 1,
		///			"categoryId": 1
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid body parameters</response>
		/// <response code="404">Supplier or Category with this id was not found</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> SaveProduct([FromBody] ProductRequest productRequest)
		{
			Product product = productRequest.ToProduct();

			Supplier? supplier = await productContext.Suppliers.FindAsync(product.SupplierId);
			if (supplier == null)
				return NotFound();

			Category? category = await productContext.Categories.FindAsync(product.CategoryId);
			if (category == null)
				return NotFound();

			await productContext.Products.AddAsync(product);
			await productContext.SaveChangesAsync();
			return Ok(product.ToProductResponse());
		}



		/// <summary>
		///		Update product.
		/// </summary>
		/// <param name="product">product to update</param>
		/// <returns>updated product</returns>
		/// <remarks>
		///		Sample Request:
		///		PUT /api/products
		///		{
		///			"id": 2,
		///			"name": "newProduct"
		///		}
		///		
		///		Sample Response:
		///		{
		///			"id": 2,
		///			"name": "newProduct"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid body parameters</response>
		/// <response code="404">Product with this id was not found OR 
		/// Supplier or Category with this id was not found</response>
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateProduct([FromBody] ProductResponse product)
		{
			Product? foundProduct = await productContext.Products.FindAsync(product.Id);

			if (foundProduct == null)
				return NotFound();

			Supplier? supplier = await productContext.Suppliers.FindAsync(product.SupplierId);
			if (supplier == null)
				return NotFound();

			Category? category = await productContext.Categories.FindAsync(product.CategoryId);
			if (category == null)
				return NotFound();

			//productContext.Products.Update(product);
			productContext.Entry(foundProduct).CurrentValues.SetValues(product.ToProduct());
			await productContext.SaveChangesAsync();
			return Ok(product);
		}



		/// <summary>
		///		Patch product.
		/// </summary>
		/// <param name="id">product id</param>
		/// <param name="patchDoc">fields to patch</param>
		/// <returns>patched product</returns>
		/// <remarks>
		///		Sample Request:
		///		PATCH /api/products/{id}
		///		[
		///			{
		///				"op": "replace",
		///				"path": "name",
		///				"value": "newProductName"
		///			},
		///			{
		///				"op": "replace",
		///				"path": "price",
		///				"value": "888"
		///			},
		///			{
		///				"op": "replace",
		///				"path": "supplierId",
		///				"value": "2"
		///			},
		///			{
		///				"op": "replace",
		///				"path": "categoryId",
		///				"value": "2"
		///			}
		///		]
		///		
		///		Sample Response:
		///		{
		///			"id": 25,
		///			"name": "newProductName",
		///			"price": 888.0,
		///			"supplierId": 2,
		///			"categoryId": 2
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Product with this id was not found</response>
		/// <response code="500">Not valid body parameters</response>
		[HttpPatch("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> PatchProduct(int id, JsonPatchDocument<Product> patchDoc)
		{
			Product? product = await productContext.Products.FindAsync(id);

			if (product == null)
				return NotFound();

			patchDoc.ApplyTo(product);
			await productContext.SaveChangesAsync();
			return Ok(product.ToProductResponse());
		}



		/// <summary>
		///		Delete product.
		/// </summary>
		/// <param name="id">product id</param>
		/// <returns>deleted product</returns>
		/// <remarks>
		///		Sample Request:
		///		DELETE /api/products/{id}
		///		
		///		Sample Response:
		///		{
		///			"id": 25,
		///			"name": "newProductName",
		///			"price": 888.00,
		///			"supplierId": 3,
		///			"categoryId": 2
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Product with this id was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			Product? product = await productContext.Products.FindAsync(id);

			if (product == null)
				return NotFound();

			productContext.Products.Remove(product);
			await productContext.SaveChangesAsync();
			return Ok(product.ToProductResponse());
		}
	}
}