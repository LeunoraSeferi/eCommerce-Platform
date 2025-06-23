using ProductApi.Infrastructure.Data;
using System.Drawing.Text;
using ProductApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using FluentAssertions;
using System.Linq.Expressions;
using Xunit;
using ProductApi.Application.Interfaces;

namespace UnitTest.ProductApi.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly ProductDbContext productDbContext;
        private readonly ProductRepository productRepository;

        public ProductRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductDb").Options;

            productDbContext = new ProductDbContext(options);
            productDbContext.Database.EnsureDeleted();   // rregullon "same key" error
            productDbContext.Database.EnsureCreated();

            productRepository = new ProductRepository(productDbContext);
        }

        //create product

        [Fact]
        public async Task CreateAsync_WhenProductAlreadyExist_ReturnErrorResponse()
        {
            //arrange
            var exisitingProduct = new Product { Name = "ExistingProduct" };
            productDbContext.Products.Add(exisitingProduct);
            await productDbContext.SaveChangesAsync();
            
            //act
            var result = await productRepository.CreateAsync(exisitingProduct);


            //assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("ExistingProduct already added");
        }


        [Fact]
        public async Task CreateAsync_WhenProductDoesNotExist_AddProductAndReturnsSuccessResponse()
        {
            //arrange
            var product = new Product() { Name = "New Product" };

            //act
            var result = await productRepository.CreateAsync(product);


            //assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("New Product added to database successfully");
        }

        //delete product
        [Fact]
        public async Task DeleteAsync_WhenProductIsFound_ReturnsSuccessResponse()
        {
            //arrange
            var product = new Product() { Id = 1, Name = "Existing Product", Price = 78.67m, Quantity = 5 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();


            //act
            var result = await productRepository.DeleteAsync(product);

            //assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Existing Product is deleted successfully");
        }


        [Fact]
        public async Task DeleteAsync_WhenProductIsNotFound_ReturnsNotFoundResponse()
        {
            //arrange
            var product = new Product() { Id = 2, Name = "NonExistingProduct", Price = 78.67m, Quantity = 5 };

            //act
            var result = await productRepository.DeleteAsync(product);


            //assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Trim().Should().Be("NonExistingProduct not found");

        }

        //get product by id
        [Fact]
        public async Task FindByIdAsync_WhenProductIsFound_ReturnsProduct()
        {
            //arrange
            var product = new Product() { Id = 1, Name = "Existing Product", Price = 78.67m, Quantity = 5 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            //act
            var result = await productRepository.FindByIdAsync(product.Id);

            //assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Existing Product");
        }

        [Fact]
        public async Task FindByIdAsync_WhenProductIsNotFound_ReturnNull()
        {
            //act
            var result = await productRepository.FindByIdAsync(99);
            //assert
            result.Should().BeNull();
        }

        //get all products
        [Fact]
        public async Task GetAllAsync_WhenProductsAreFound_ReturnProducts()
        {
            //arrange
            var products = new List<Product>()
            {
                new () {Id = 1, Name = "Product 1" },
                new () {Id = 2, Name = "Product 2"}
            };

            productDbContext.Products.AddRange(products);
            await productDbContext.SaveChangesAsync();

            //act
            var result = await productRepository.GetAllAsync();

            ////assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
        }

        [Fact]
        public async Task GetAllAsync_WhenProductsAreNotFound_ReturnNull()
        {
            //act
            var result = await productRepository.GetAllAsync();

            //assert
            result.Should().NotBeNull();
        }


        //get by any type(int,bool,stirng etc..)
        [Fact]
        public async Task GetByAsync_WhenProductIsFound_ReturnProduct()
        {
            //arrange
            var product = new Product() { Id = 1, Name = "Product 1" };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();
            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 1";

            //act
            var result = await productRepository.GetByAsync(predicate);

            //assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsNotFound_ReturnNull()
        {
            //arrange
            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 2";

            //act
            var result = await productRepository.GetByAsync(predicate);

            //assert
            result.Should().BeNull();
        }

        //update product
        [Fact]
        public async Task UpdateProduct_WhenProductIsUpdatedSuccessfully_ReturnSuccessResponse()
        {
            //arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            //act
            var result = await productRepository.UpdateAsync(product);

            //assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product 1 is updated successfully");
        }


        [Fact]
        public async Task UpdateAsync_WhenProductIsNotFound_ReturnErrorResponse()
        {
            //arrange
            var updateProduct = new Product { Id = 99, Name = "Product 22" }; // ndryshuar id

            //act
            var result = await productRepository.UpdateAsync(updateProduct);

            //assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product 22 not found");
        }



    }
}
