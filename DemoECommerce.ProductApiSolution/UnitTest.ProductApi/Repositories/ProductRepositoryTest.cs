using ProductApi.Infrastructure.Data;
using System.Drawing.Text;
using ProductApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using FluentAssertions;
using System.Linq.Expressions;

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

        [Fact]
        public async Task CreateAsync_WhenProductAlreadyExist_ReturnErrorResponse()
        {
            var exisitingProduct = new Product { Name = "ExistingProduct" };
            productDbContext.Products.Add(exisitingProduct);
            await productDbContext.SaveChangesAsync();

            var result = await productRepository.CreateAsync(exisitingProduct);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("ExistingProduct already added");
        }

        [Fact]
        public async Task CreateAsync_WhenProductDoesNotExist_AddProductAndReturnErrorResponse()
        {
            var product = new Product() { Name = "New Product" };
            var result = await productRepository.CreateAsync(product);

            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("New Product added to database successfully");
        }

        [Fact]
        public async Task DeleteAsync_WhenProductIFound_ReturnsSuccessResponse()
        {
            var Product = new Product() { Id = 1, Name = "Existing Product", Price = 78.67m, Quantity = 5 };
            productDbContext.Products.Add(Product);
            await productDbContext.SaveChangesAsync();

            var result = await productRepository.DeleteAsync(Product);

            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Existing Product is deleted successfully");
        }

        [Fact]
        public async Task DeleteAsync_WhenProductIsNotFound_ReturnsNotFoundResponse()
        {
            var Product = new Product() { Id = 2, Name = "NonExisting Product", Price = 78.67m, Quantity = 5 };
            var result = await productRepository.DeleteAsync(Product);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Trim().Should().Be("NonExisting Product not found");
        }

        [Fact]
        public async Task FindById_WhenProductisFound_ReturnsProduct()
        {
            var Product = new Product() { Id = 1, Name = "Existing Product", Price = 78.67m, Quantity = 5 };
            productDbContext.Products.Add(Product);
            await productDbContext.SaveChangesAsync();

            var result = await productRepository.FindByIdAsync(Product.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Existing Product");
        }

        [Fact]
        public async Task FindIdAsync_WhenProductIsNotFound_ReturnNull()
        {
            var result = await productRepository.FindByIdAsync(99);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_WhenProductsAreFound_ReturnProducts()
        {
            var products = new List<Product>()
            {
                new () {Id = 1, Name = "Product 1" },
                new () {Id = 2, Name = "Product 2"}
            };

            productDbContext.Products.AddRange(products);
            await productDbContext.SaveChangesAsync();

            var result = await productRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
        }

        [Fact]
        public async Task GetAllAsync_WhenProductAreNotFound_ReturnNull()
        {
            var result = await productRepository.GetAllAsync();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsFound_ReturnProduct()
        {
            var product = new Product() { Id = 1, Name = "Product 1" };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 1";
            var result = await productRepository.GetByAsync(predicate);

            result.Should().NotBeNull();
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsNotFound_ReturnNull()
        {
            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 2";
            var result = await productRepository.GetByAsync(predicate);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateProduct_WhenProductIsUpdatedSuccessfully_ReturnSuccessResponse()
        {
            var product = new Product { Id = 1, Name = "Product 1" };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            var result = await productRepository.UpdateAsync(product);

            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product 1 is updated successfully");
        }

        [Fact]
        public async Task UpdateAsync_WhenProductIsNotFound_ReturnErrorResponse()
        {
            var updateProduct = new Product { Id = 99, Name = "Product 22" }; // ndryshuar id

            var result = await productRepository.UpdateAsync(updateProduct);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product 22 not found");
        }
    }
}
