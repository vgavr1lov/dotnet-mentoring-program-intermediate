using DataAccessLib.Interfaces;
using DataAccessLib.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
   private readonly IUnitOfWork _unitOfWork;
   public ProductsController(IUnitOfWork unitOfWork)
   {
      _unitOfWork = unitOfWork;
   }
   [HttpGet]
   [Route("{id}")]
   public ActionResult<Product> GetProduct(int id)
   {
      var product = _unitOfWork.Products.Read(id);
      if (product == null)
         return NotFound();

      return Ok(product);
   }

   [HttpGet]
   public ActionResult<List<Product>> GetProducts(
       [FromQuery] int pageNumber = 0,
       [FromQuery] int pageSize = 10,
       [FromQuery] int? categoryId = null)
   {
      var products = _unitOfWork.Products.Read()
          .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
          .Skip(pageNumber * pageSize)
          .Take(pageSize)
          .ToList();
      if (products == null || products.Count == 0)
         return NotFound();

      return Ok(products);
   }

   [HttpPost]
   public ActionResult<List<Product>> PostProduct(Product product)
   {
      if (!ModelState.IsValid)
         return BadRequest();

      _unitOfWork.Products.Create(product);
      _unitOfWork.Save();

      return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
   }

   [HttpPut]
   [Route("{id}")]
   public ActionResult<Product> PutProduct(int id, Product product)
   {
      if (id != product.ProductId)
         return BadRequest("ID mismatch");

      if (!ModelState.IsValid)
         return BadRequest();

      _unitOfWork.Products.Update(product);
      _unitOfWork.Save();

      return NoContent();
   }

   [HttpDelete]
   [Route("{id}")]
   public ActionResult<Product> DeleteProduct(int id)
   {
      var product = _unitOfWork.Products.Read(id);
      if (product == null)
         return NotFound();

      _unitOfWork.Products.Delete(id);
      _unitOfWork.Save();

      return Ok(product);
   }


}
