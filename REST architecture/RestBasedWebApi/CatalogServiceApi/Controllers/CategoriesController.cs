using DataAccessLib;
using DataAccessLib.Interfaces;
using DataAccessLib.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
   private readonly IUnitOfWork _unitOfWork;
   public CategoriesController(IUnitOfWork unitOfWork)
   {
      _unitOfWork = unitOfWork;
   }
   [HttpGet]
   [Route("{id}")]
   public ActionResult<Category> GetCategory(int id)
   {
      var category = _unitOfWork.Categories.Read(id);
      if (category == null)
         return NotFound();

      return Ok(category);
   }

   [HttpGet]
   public ActionResult<List<Category>> GetCategories()
   {
      var categories = _unitOfWork.Categories.Read();
      if (categories == null || categories.Count == 0)
         return NotFound();

      return Ok(categories);
   }

   [HttpPost]
   public ActionResult<Category> PostCategory(Category category)
   {
      if (!ModelState.IsValid)
         return BadRequest();

      _unitOfWork.Categories.Create(category);
      _unitOfWork.Save();

      return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
   }

   [HttpPut]
   [Route("{id}")]
   public ActionResult<Category> PutCategory(int id, Category category)
   {
      if (id != category.CategoryId)
         return BadRequest("ID mismatch");

      if (!ModelState.IsValid)
         return BadRequest();

      _unitOfWork.Categories.Update(category);
      _unitOfWork.Save();

      return NoContent();
   }

   [HttpDelete]
   [Route("{id}")]
   public ActionResult<Category> DeleteCategory(int id)
   {
      var category = _unitOfWork.Categories.Read(id);
      if (category == null)
         return NotFound();

      _unitOfWork.Categories.Delete(id);
      _unitOfWork.Save();

      return Ok(category);
   }
}
