using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Apps.Tests
{
    public class ProductServiceTests
    {
        [Fact(DisplayName = "Get all Products")]
        [Trait("ProductService", "Get all Products")]
        public async Task GetAll()
        {
            var repository = new Mock<ProductRepository>();

            var response = await new ProductService(repository.Object).FindAll();

            Assert.True(response.Count == 4);
        }
    }
}
