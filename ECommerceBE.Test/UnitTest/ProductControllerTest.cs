using NUnit.Framework;
using ECommerceBE.Controllers;
using Moq;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ECommerceBE.Controllers.Utilities;
using Newtonsoft.Json;
using System;

namespace ECommerceBE.Test.UnitTest
{
    internal class ProductControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestPag()
        {
            //Arrange
            var @params = new PaginationParams();
            @params.PageSize = 1;
            @params.Page = 1;

            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(r => r.SearchItemsAsync(It.IsAny<string>())).Returns(elmetodo());

            ProductController controller = new ProductController(mockRepository.Object);

            //Act
            var searchProductsResult = await controller.SearchProductsByName("p", @params);

            var okObjectResult = searchProductsResult as OkObjectResult;

            string jsonString = JsonConvert.SerializeObject(okObjectResult.Value);
            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

            jsonResult.TryGetValue("matchedProducts", out dynamic matchedProducts);
            jsonResult.TryGetValue("countMatchedProducts", out dynamic countMatchedProducts);
            jsonResult.TryGetValue("productsPaginated", out dynamic productsPaginated);
            jsonResult.TryGetValue("numberOfPages", out dynamic numberOfPages);

            //Assert
            Assert.NotNull(okObjectResult);

            Assert.That(countMatchedProducts, Is.EqualTo(2));
            Assert.That(Math.Abs(numberOfPages - (countMatchedProducts/@params.PageSize)) < 1);
        }

        public async Task<ICollection<Product>> elmetodo()
        {
            Product product = new Product();
            product.Id = 1;
            product.Name = "p1";
            product.Price = 0;
            product.Description = "string";
            product.Stock = 0;
            product.Enabled = true;
            product.SupplierId = 1;

            var list = new List<Product>();
            list.Add(product);

            Product product2 = new Product();
            product.Id = 2;
            product.Name = "p2";
            product.Price = 0;
            product.Description = "string";
            product.Stock = 0;
            product.Enabled = true;
            product.SupplierId = 1;

            list.Add(product2);

            return list;
        }
    }
}
