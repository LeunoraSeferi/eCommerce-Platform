

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


namespace UnitTest.ProductApi.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProduct productInterface;
        private readonly ProductsController productsController;

        public ProductControllerTest()
        {

            // set up dependecy
            productInterface = A.Fake<IProduct>();

            // set up System Under Test - SUT
            productsController = new ProductsController(productInterface);

        }
        // GET ALL PRODUCTS 
        [Fact]
        public async Task GetProduct_WhenProductExists_ReturnOkResponseWithProduct()
        {
            // arrange 
            var products = new List<Product>()
            {
                new(){Id = 1, Name ="Product 1", Quantity = 10, Price = 100.70m },
                new(){Id = 2, Name ="Product 2", Quantity = 110, Price = 1004.70m }

            };


            // set up fake response for GetAllAsync method 
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            // Act
            var result = await productsController.GetProducts();

            // Assert

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
            // Arrange
            var products = new List<Product>();

            // Set up fake response for GetAllAsync();
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            // Act
            var result = await productsController.GetProducts();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = notFoundResult.Value as string;
            message.Should().Be("No products detected in the database");


        }
        // Create product 

        [Fact]

        public async Task CreateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            // Arrange  
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            //Act 
            var result = await productsController.CreateProduct(productDTO);

            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]

        public async Task CreateProduct_WhenCreateIsSucssesfull_ReturnOkResponse()
        {

            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 67.95m);
            var response = new Response(true, "Created");

            //Act 
            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.CreateProduct(productDTO);

            // Assert

            var okResult = result.Result as OkObjectResult;
            okResult!.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult!.Message.Should().Be("Created");
            responseResult!.Flag.Should().BeTrue();


        }
        [Fact]
        public async Task CreatedProduct_WhenCreateFails_ReturnsBadRequestResponse()
        {
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m);
            var response = new Response(false, "Failed");

            //Act
            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.CreateProduct(productDTO);


            // Assert
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
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m);
            var response = new Response(true, "Update");

            // Act 
            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);

            // Assert
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
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m);
            var response = new Response(false, "Update Failed");

            //Act
            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);


            // Assert
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
            //Arrange
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m);
            var response = new Response(true, "Delete succsesfully");

            // Set up to get fake response from DelteAsync()

            A.CallTo(() => productInterface.DeleteAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await productsController.DeleteProduct(productDTO);

            // Assert
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
            var productDTO = new ProductDTO(1, "Product 1", 78, 45.36m);
            var response = new Response(false, "Delete Failed");

            // Set up to get fake response from DelteAsync()
            A.CallTo(() => productInterface.DeleteAsync(A<Product>.Ignored)).Returns(response);

            //Act
            var result = await productsController.DeleteProduct(productDTO);


            // Assert
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