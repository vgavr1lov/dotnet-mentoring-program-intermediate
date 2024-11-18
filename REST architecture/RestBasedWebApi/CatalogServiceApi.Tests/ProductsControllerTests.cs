using CatalogServiceApi.Controllers;
using DataAccessLib.Interfaces;
using DataAccessLib.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatalogServiceApi.Tests;

public class ProductsControllerTests
{
   private readonly Mock<IUnitOfWork> _mockUnitOfWork;
   private readonly Mock<IRepository<Product>> _mockProductRepository;
   private readonly ProductsController _controller;

   public ProductsControllerTests()
   {
      _mockUnitOfWork = new Mock<IUnitOfWork>();
      _mockProductRepository = new Mock<IRepository<Product>>();
      _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);
      _controller = new ProductsController(_mockUnitOfWork.Object);
   }

   [Fact]
   public void GetProduct_ProvideProductId_ShouldReturnOkResultWithProduct()
   {
      // Arrange
      var productId = 1;

      _mockProductRepository
         .Setup(x => x.Read(productId))
         .Returns(GetSampleProduct());

      // Act
      var actionResult = _controller.GetProduct(productId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var product = Assert.IsType<Product>(okResult.Value);
      Assert.Equal(productId, product.ProductId);
      Assert.Equal("Test Product 1", product.ProductName);
   }

   [Fact]
   public void GetProduct_ProvideProductId_ShouldReturnNotFound()
   {
      // Arrange
      var productId = 1;

      _mockProductRepository
         .Setup(x => x.Read(productId))
         .Returns((Product?)null);

      // Act
      var actionResult = _controller.GetProduct(productId);

      // Assert
      var okResult = Assert.IsType<NotFoundResult>(actionResult.Result);
   }

   [Fact]
   public void GetProducts_ShouldReturnOkResultWithListOfProducts()
   {
      // Arrange
      _mockProductRepository
         .Setup(x => x.Read())
         .Returns(GetSampleProducts());

      // Act
      var actionResult = _controller.GetProducts();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var products = Assert.IsType<List<Product>>(okResult.Value);
      Assert.Equivalent(GetSampleProducts(), products, strict: true);
   }

   [Fact]
   public void GetProducts_ShouldReturnNotFound()
   {
      // Arrange
      _mockProductRepository
         .Setup(x => x.Read())
         .Returns(new List<Product>());

      // Act
      var actionResult = _controller.GetProducts();

      // Assert
      var okResult = Assert.IsType<NotFoundResult>(actionResult.Result);
   }

   [Fact]
   public void PostProduct_ProvideProductWithoutId_ShouldReturnCreatedResultWithProductId()
   {
      // Arrange
      var productId = 1;
      var newProduct = new Product
      {
         ProductName = "Test Product 1",
         SupplierId = 1,
         CategoryId = 1,
         QuantityPerUnit = "20 boxes x 40 bags",
         UnitPrice = 10,
         UnitsInStock = 40,
         UnitsOnOrder = null,
         ReorderLevel = 10,
         Discontinued = false
      };

      _mockProductRepository
         .Setup(x => x.Create(newProduct))
         .Callback<Product>(product =>
         {
            product.ProductId = productId;
         });

      _mockUnitOfWork
          .Setup(x => x.Save())
          .Verifiable();

      // Act
      var actionResult = _controller.PostProduct(newProduct);

      // Assert
      var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
      var product = Assert.IsType<Product>(createdResult.Value);
      Assert.Equal(productId, product.ProductId);
      Assert.Equal("Test Product 1", product.ProductName);

      _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
   }

   [Fact]
   public void PostProduct_ProvideProductWithoutName_ShouldReturnBadRequest()
   {
      // Arrange
      _controller.ModelState.AddModelError("ProductName", "Required");
      var newProduct = new Product
      {
         ProductName = "",
         SupplierId = 1,
         CategoryId = 1,
         QuantityPerUnit = "20 boxes x 40 bags",
         UnitPrice = 10,
         UnitsInStock = 40,
         UnitsOnOrder = null,
         ReorderLevel = 10,
         Discontinued = false
      };

      // Act
      var actionResult = _controller.PostProduct(newProduct);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestResult>(actionResult.Result);
   }

   [Fact]
   public void PutProduct_ProvideProductWithId_ShouldReturnNoContentResult()
   {
      // Arrange
      var sampleProduct = GetSampleProduct();

      _mockProductRepository
          .Setup(x => x.Update(sampleProduct))
          .Verifiable();

      _mockUnitOfWork
          .Setup(x => x.Save())
          .Verifiable();

      // Act
      var actionResult = _controller.PutProduct(sampleProduct.ProductId, sampleProduct);

      // Assert
      var createdResult = Assert.IsType<NoContentResult>(actionResult.Result);

      _mockProductRepository.Verify(x => x.Update(sampleProduct), Times.Once);
      _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
   }

   [Fact]
   public void PutProduct_ProvideProductWithoutName_ShouldReturnBadRequest()
   {
      // Arrange
      var sampleProduct = new Product
      {
         ProductId = 1,
         ProductName = "",
         SupplierId = 1,
         CategoryId = 1,
         QuantityPerUnit = "20 boxes x 40 bags",
         UnitPrice = 10,
         UnitsInStock = 40,
         UnitsOnOrder = null,
         ReorderLevel = 10,
         Discontinued = false
      };
      _controller.ModelState.AddModelError("ProductName", "Required");

      // Act
      var actionResult = _controller.PutProduct(sampleProduct.ProductId, sampleProduct);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestResult>(actionResult.Result);
   }

   [Fact]
   public void PutProduct_ProvideMismatchedId_ShouldReturnBadRequest()
   {
      // Arrange
      var sampleProduct = GetSampleProduct();

      // Act
      var actionResult = _controller.PutProduct(2, sampleProduct);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
   }

   [Fact]
   public void DeleteProduct_ProvideProductId_ShouldReturnOkResultWithProduct()
   {
      // Arrange
      var productId = 1;

      _mockProductRepository
         .Setup(x => x.Read(productId))
         .Returns(GetSampleProduct());

      _mockProductRepository
          .Setup(x => x.Delete(productId))
          .Verifiable();

      _mockUnitOfWork
          .Setup(x => x.Save())
          .Verifiable();

      // Act
      var actionResult = _controller.DeleteProduct(productId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var product = Assert.IsType<Product>(okResult.Value);
      Assert.Equal(1, product.ProductId);
      Assert.Equal("Test Product 1", product.ProductName);

      _mockProductRepository.Verify(x => x.Delete(productId), Times.Once);
      _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
   }

   [Fact]
   public void DeleteProduct_ProvideProductId_ShouldReturnNotFound()
   {
      // Arrange
      var productId = 1;

      _mockProductRepository
         .Setup(x => x.Read(productId))
         .Returns((Product?)null);

      // Act
      var actionResult = _controller.DeleteProduct(productId);

      // Assert
      var okResult = Assert.IsType<NotFoundResult>(actionResult.Result);
   }

   private Product GetSampleProduct()
   {
      return new Product
      {
         ProductId = 1,
         ProductName = "Test Product 1",
         SupplierId = 1,
         CategoryId = 1,
         QuantityPerUnit = "20 boxes x 40 bags",
         UnitPrice = 18,
         UnitsInStock = 40,
         UnitsOnOrder = null,
         ReorderLevel = 10,
         Discontinued = false
      };
   }

   private List<Product> GetSampleProducts()
   {
      return new List<Product>
      {
         new Product
         {
            ProductId = 1,
            ProductName = "Test Product 1",
            SupplierId = 1,
            CategoryId = 1,
            QuantityPerUnit = "20 boxes x 40 bags",
            UnitPrice = 10,
            UnitsInStock = 40,
            UnitsOnOrder = null,
            ReorderLevel = 10,
            Discontinued = false
         },
         new Product
         {
            ProductId = 2,
            ProductName = "Test Product 2",
            SupplierId = 2,
            CategoryId = 2,
            QuantityPerUnit = "20 boxes x 40 bags",
            UnitPrice = 11,
            UnitsInStock = 40,
            UnitsOnOrder = null,
            ReorderLevel = 10,
            Discontinued = false
         },
         new Product
         {
            ProductId = 3,
            ProductName = "Test Product 3",
            SupplierId = 3,
            CategoryId = 3,
            QuantityPerUnit = "20 boxes x 40 bags",
            UnitPrice = 12,
            UnitsInStock = 40,
            UnitsOnOrder = null,
            ReorderLevel = 10,
            Discontinued = false
         }
      };
   }
}
