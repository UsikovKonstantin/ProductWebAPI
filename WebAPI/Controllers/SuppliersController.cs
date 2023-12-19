using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
	/// <summary>
	/// Suppliers controller.
	/// </summary>
	[ApiController]
	[Route("api/suppliers")]
	public class SuppliersController : Controller
	{
		private readonly ProductContext productContext;
		/// <summary>
		/// Controller constructor.
		/// </summary>
		/// <param name="productContext">database context</param>
		public SuppliersController(ProductContext productContext)
		{
			this.productContext = productContext;
		}



		/// <summary>
		///		Get a list of all suppliers.
		/// </summary>
		/// <returns>list of all suppliers</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/suppliers
		/// 
		///		Sample Response:
		///		[
		///			{
		///				"id": 1,
		///				"name": "Calve",
		///				"city": "Moscow"
		///			},
		///			{
		///				"id": 2,
		///				"name": "TESCOMA",
		///				"city": "Tver"
		///			},
		///			{
		///				"id": 3,
		///				"name": "Haier",
		///				"city": "Berlin"
		///			},
		///			{
		///				"id": 4,
		///				"name": "Nescafe",
		///				"city": "Paris"
		///			},
		///			{
		///				"id": 5,
		///				"name": "Be quiet",
		///				"city": "Barcelona"
		///			}
		///		]
		/// </remarks>
		/// <response code="200">Success</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IAsyncEnumerable<SupplierResponse> GetSuppliers()
		{
			return productContext.Suppliers.Select(c => c.ToSupplierResponse()).AsAsyncEnumerable();
		}



		/// <summary>
		///		Get a supplier by id.
		/// </summary>
		/// <param name="id">supplier id</param>
		/// <returns>supplier</returns>
		/// <remarks>
		///		Sample Request:
		///		GET /api/suppliers/{id}
		/// 
		///		Sample Response:
		///		{
		///			"id": 2,
		///			"name": "TESCOMA",
		///			"city": "Tver"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Supplier with this id was not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSupplier(int id)
		{
			Supplier? supplier = await productContext.Suppliers.FindAsync(id);

			if (supplier == null)
				return NotFound();

			return Ok(supplier.ToSupplierResponse());
		}



		/// <summary>
		///		Save supplier.
		/// </summary>
		/// <param name="supplierRequest">supplier to save</param>
		/// <returns>saved supplier</returns>
		/// <remarks>
		///		Sample Request:
		///		POST /api/suppliers
		///		{
		///			"name": "newSupplier",
		///			"city": "Moscow"
		///		}
		///		
		///		Sample Response:
		///		{
		///			"id": 14,
		///			"name": "newSupplier",
		///			"city": "Moscow"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid body parameters</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SaveSupplier([FromBody] SupplierRequest supplierRequest)
		{
			Supplier supplier = supplierRequest.ToSupplier();
			await productContext.Suppliers.AddAsync(supplier);
			await productContext.SaveChangesAsync();
			return Ok(supplier.ToSupplierResponse());
		}



		/// <summary>
		///		Update supplier.
		/// </summary>
		/// <param name="supplier">supplier to update</param>
		/// <returns>updated supplier</returns>
		/// <remarks>
		///		Sample Request:
		///		PUT /api/suppliers
		///		{
		///			"id": 10,
		///			"name": "newName",
		///			"city": "newCity"
		///		}
		///		
		///		Sample Response:
		///		{
		///			"id": 10,
		///			"name": "newName",
		///			"city": "newCity"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid body parameters</response>
		/// <response code="404">Supplier with this id was not found</response>
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateSupplier([FromBody] SupplierResponse supplier)
		{
			Supplier? foundSupplier = await productContext.Suppliers.FindAsync(supplier.Id);

			if (foundSupplier == null)
				return NotFound();

			//productContext.Suppliers.Update(supplier);
			productContext.Entry(foundSupplier).CurrentValues.SetValues(supplier.ToSupplier());
			await productContext.SaveChangesAsync();
			return Ok(supplier);
		}



		/// <summary>
		///		Patch supplier.
		/// </summary>
		/// <param name="id">supplier id</param>
		/// <param name="patchDoc">fields to patch</param>
		/// <returns>patched supplier</returns>
		/// <remarks>
		///		Sample Request:
		///		PATCH /api/suppliers/{id}
		///		[
		///			{
		///				"op": "replace",
		///				"path": "name",
		///				"value": "newSupplierName"
		///			},
		///			{
		///				"op": "replace",
		///				"path": "city",
		///				"value": "newCity"
		///			}
		///		]
		///		
		///		Sample Response:
		///		{
		///			"id": 10,
		///			"name": "newSupplierName",
		///			"city": "newCity"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Supplier with this id was not found</response>
		/// <response code="500">Not valid body parameters</response>
		[HttpPatch("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> PatchSupplier(int id, JsonPatchDocument<Supplier> patchDoc)
		{
			Supplier? supplier = await productContext.Suppliers.FindAsync(id);

			if (supplier == null)
				return NotFound();

			patchDoc.ApplyTo(supplier);
			await productContext.SaveChangesAsync();
			return Ok(supplier.ToSupplierResponse());
		}



		/// <summary>
		///		Delete supplier.
		/// </summary>
		/// <param name="id">supplier id</param>
		/// <returns>deleted supplier</returns>
		/// <remarks>
		///		Sample Request:
		///		DELETE /api/suppliers/{id}
		///		
		///		Sample Response:
		///		{
		///			"id": 10,
		///			"name": "SupplierName",
		///			"city": "City"
		///		}
		/// </remarks>
		/// <response code="200">Success</response>
		/// <response code="400">Not valid id</response>
		/// <response code="404">Supplier with this id was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteSupplier(int id)
		{
			Supplier? supplier = await productContext.Suppliers.FindAsync(id);

			if (supplier == null)
				return NotFound();

			productContext.Suppliers.Remove(supplier);
			await productContext.SaveChangesAsync();
			return Ok(supplier.ToSupplierResponse());
		}
	}
}