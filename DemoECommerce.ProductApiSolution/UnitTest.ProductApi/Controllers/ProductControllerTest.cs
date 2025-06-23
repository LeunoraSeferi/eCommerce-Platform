using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Presentation.Controllers;
using eCommerce.SharedLibrary.Responses;
using Response = eCommerce.SharedLibrary.Responses.Response;
using Xunit;

namespace UnitTest.ProductApi.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProduct productInterface;
        private readonly ProductsController productsController;

        public ProductControllerTest()
        {
            productInterface = A.Fake<IProduct>();
            productsController = new ProductsController(productInterface);
        }

        [Fact]
        public async Task GetProduct_WhenProductExists_ReturnOkResponseWithProduct()
        {
            var products = new List<Product>()
            {
                new(){Id = 1, Name ="Product 1", Quantity = 10, Price = 100.70m, Image = "img1.jpg"},
                new(){Id = 2, Name ="Product 2", Quantity = 110, Price = 1004.70m, Image = "img2.jpg"}
            };

            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            var result = await productsController.GetProducts();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedProducts = okResult.Value as IEnumerable<ProductDTO>;

            returnedProducts.Should().NotBeNull();
            returnedProducts.Should().HaveCount(2);
            returnedProducts!.First().Id.Should().Be(1);
            returnedProducts!.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task GetProducts_WhenNoProductsExist_ReturnNotFoundResponse()
        {
            var products = new List<Product>();
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            var result = await productsController.GetProducts();

            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = notFoundResult.Value as string;
            message.Should().Be("No products detected in the database");
        }

        [Fact]
        public async Task CreateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m, "img.jpg");
            productsController.ModelState.AddModelError("Name", "Required");

            var result = await productsController.CreateProduct(productDTO);

            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateProduct_WhenCreateIsSucssesful_ReturnOkResponse()
        {
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m, "img.jpg");
            var response = new Response(true, "Created");

            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.CreateProduct(productDTO);

            var okResult = result.Result as OkObjectResult;
            okResult!.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult!.Message.Should().Be("Created");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task CreateProduct_WhenCreateFails_ReturnBadRequestResponse()
        {
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m, "img.jpg");
            var response = new Response(false, "Failed");

            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.CreateProduct(productDTO);

            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Failed");
            responseResult?.Flag.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateProduct_WhenUpdateIsSuccsesful_ReturnOkResponse()
        {
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m, "img.jpg");
            var response = new Response(true, "Update");

            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);

            var okResult = result.Result as ObjectResult;
            okResult!.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult!.Message.Should().Be("Update");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateProduct_WhenUpdateFails_ReturnsBadRequestResponse()
        {
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m, "img.jpg");
            var response = new Response(false, "Update Failed");

            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);

            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Update Failed");
            responseResult?.Flag.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteProduct_WhenDeleteIsSuccsesful_ReturnOkResponse()
        {
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m, "img.jpg");
            var response = new Response(true, "Delete succsesfully");

            A.CallTo(() => productInterface.DeleteAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.DeleteProduct(productDTO);

            var okResult = result.Result as ObjectResult;
            okResult!.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult!.Message.Should().Be("Delete succsesfully");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteProduct_WhenDeleteFails_ReturnsBadRequestResponse()
        {
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m, "img.jpg");
            var response = new Response(false, "Delete Failed");

            A.CallTo(() => productInterface.DeleteAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.DeleteProduct(productDTO);

            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult.Should().NotBeNull();
            responseResult!.Message.Should().Be("Delete Failed");
            responseResult?.Flag.Should().BeFalse();
        }
    }
}
