using CatalogServiceApi.Controllers;
using DataAccessLib.Interfaces;
using DataAccessLib.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatalogServiceApi.Tests;

public class CategoriesControllerTests
{
   private readonly Mock<IUnitOfWork> _mockUnitOfWork;
   private readonly Mock<IRepository<Category>> _mockCategoryRepository;
   private readonly CategoriesController _controller;

   public CategoriesControllerTests()
   {
      _mockUnitOfWork = new Mock<IUnitOfWork>();
      _mockCategoryRepository = new Mock<IRepository<Category>>();
      _mockUnitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
      _controller = new CategoriesController(_mockUnitOfWork.Object);
   }

   [Fact]
   public void GetCategory_ProvideCategoryId_ShouldReturnOkResultWithCategory()
   {
      // Arrange
      var categoryId = 42;

      _mockCategoryRepository
         .Setup(x => x.Read(categoryId))
         .Returns(GetSampleCategory());

      // Act
      var actionResult = _controller.GetCategory(categoryId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var category = Assert.IsType<Category>(okResult.Value);
      Assert.Equal(categoryId, category.CategoryId);
      Assert.Equal("Test Category", category.CategoryName);
   }

   [Fact]
   public void GetCategory_ProvideCategoryId_ShouldReturnNotFound()
   {
      // Arrange
      var categoryId = 42;

      _mockCategoryRepository
         .Setup(x => x.Read(categoryId))
         .Returns((Category?)null);

      // Act
      var actionResult = _controller.GetCategory(categoryId);

      // Assert
      var okResult = Assert.IsType<NotFoundResult>(actionResult.Result);
   }

   [Fact]
   public void GetCategories_ShouldReturnOkResultWithListOfCategories()
   {
      // Arrange
      _mockCategoryRepository
         .Setup(x => x.Read())
         .Returns(GetSampleCategories());

      // Act
      var actionResult = _controller.GetCategories();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var categories = Assert.IsType<List<Category>>(okResult.Value);
      Assert.Equivalent(GetSampleCategories(), categories, strict: true);
   }

   [Fact]
   public void GetCategories_ShouldReturnNotFound()
   {
      // Arrange
      _mockCategoryRepository
         .Setup(x => x.Read())
         .Returns(new List<Category>());

      // Act
      var actionResult = _controller.GetCategories();

      // Assert
      var okResult = Assert.IsType<NotFoundResult>(actionResult.Result);
   }

   [Fact]
   public void PostCategory_ProvideCategoryWithoutId_ShouldReturnCreatedResultWithCategoryId()
   {
      // Arrange
      var categoryId = 42;
      var newCategory = new Category { CategoryName = "Test Category", Description = "Test Description" };

      _mockCategoryRepository
         .Setup(x => x.Create(newCategory))
         .Callback<Category>(category =>
         {
            category.CategoryId = categoryId;
         });

      _mockUnitOfWork
          .Setup(x => x.Save())
          .Verifiable();

      // Act
      var actionResult = _controller.PostCategory(newCategory);

      // Assert
      var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
      var category = Assert.IsType<Category>(createdResult.Value);
      Assert.Equal(categoryId, category.CategoryId);
      Assert.Equal("Test Category", category.CategoryName);

      _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
   }

   [Fact]
   public void PostCategory_ProvideCategoryWithoutName_ShouldReturnBadRequest()
   {
      // Arrange
      _controller.ModelState.AddModelError("CategoryName", "Required");
      var newCategory = new Category { CategoryName = "", Description = "Test Description" };

      // Act
      var actionResult = _controller.PostCategory(newCategory);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestResult>(actionResult.Result);
   }

   [Fact]
   public void PutCategory_ProvideCategoryWithId_ShouldReturnNoContentResult()
   {
      // Arrange
      var sampleCategory = GetSampleCategory();

      _mockCategoryRepository
          .Setup(x => x.Update(sampleCategory))
          .Verifiable();

      _mockUnitOfWork
          .Setup(x => x.Save())
          .Verifiable();

      // Act
      var actionResult = _controller.PutCategory(sampleCategory.CategoryId, sampleCategory);

      // Assert
      var createdResult = Assert.IsType<NoContentResult>(actionResult.Result);

      _mockCategoryRepository.Verify(x => x.Update(sampleCategory), Times.Once);
      _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
   }

   [Fact]
   public void PutCategory_ProvideCategoryWithoutName_ShouldReturnBadRequest()
   {
      // Arrange
      var sampleCategory = new Category { CategoryId = 42, CategoryName = "", Description = "Test Description" };
      _controller.ModelState.AddModelError("CategoryName", "Required");

      // Act
      var actionResult = _controller.PutCategory(sampleCategory.CategoryId, sampleCategory);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestResult>(actionResult.Result);
   }

   [Fact]
   public void PutCategory_ProvideMismatchedId_ShouldReturnBadRequest()
   {
      // Arrange
      var sampleCategory = GetSampleCategory();

      // Act
      var actionResult = _controller.PutCategory(41, sampleCategory);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
   }

   [Fact]
   public void DeleteCategory_ProvideCategoryId_ShouldReturnOkResultWithCategory()
   {
      // Arrange
      var categoryId = 42;

      _mockCategoryRepository
         .Setup(x => x.Read(categoryId))
         .Returns(GetSampleCategory());

      _mockCategoryRepository
          .Setup(x => x.Delete(categoryId))
          .Verifiable();

      _mockUnitOfWork
          .Setup(x => x.Save())
          .Verifiable();

      // Act
      var actionResult = _controller.DeleteCategory(categoryId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var category = Assert.IsType<Category>(okResult.Value);
      Assert.Equal(42, category.CategoryId);
      Assert.Equal("Test Category", category.CategoryName);

      _mockCategoryRepository.Verify(x => x.Delete(categoryId), Times.Once);
      _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
   }

   [Fact]
   public void DeleteCategory_ProvideCategoryId_ShouldReturnNotFound()
   {
      // Arrange
      var categoryId = 42;

      _mockCategoryRepository
         .Setup(x => x.Read(categoryId))
         .Returns((Category?)null);

      // Act
      var actionResult = _controller.DeleteCategory(categoryId);

      // Assert
      var okResult = Assert.IsType<NotFoundResult>(actionResult.Result);
   }

   private Category GetSampleCategory()
   {
      return new Category { CategoryId = 42, CategoryName = "Test Category", Description = "Test Description" };
   }
   private List<Category> GetSampleCategories()
   {
      return new List<Category>
      {
         new Category { CategoryId = 1, CategoryName = "Test Category 1", Description = "Test Description 1" },
         new Category { CategoryId = 2, CategoryName = "Test Category 2", Description = "Test Description 2" },
         new Category { CategoryId = 3, CategoryName = "Test Category 3", Description = "Test Description 3" }
      };
   }

}