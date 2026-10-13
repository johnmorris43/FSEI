using FSEI.Domain.Developers;
using FSEI.Domain.Products;
using NUnit.Framework;

namespace FSEI.Domain.Tests.Products;

[TestFixture]
public class ProductDeveloperTests
{
    [Test]
    public void ProductDeveloper_ShouldLinkProductAndDeveloper()
    {
        // Arrange
        var productType = new ProductType
        {
            Code = "AIRCRAFT",
            Name = "Aircraft"
        };

        var product = new Product
        {
            Name = "777",
            ProductType = productType
        };

        var developer = new Developer
        {
            Name = "FlightFactor"
        };

        var productDeveloper = new ProductDeveloper
        {
            ProductId = 1,
            DeveloperId = 2,
            Product = product,
            Developer = developer,
            Role = "Lead Developer"
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(productDeveloper.Product.Name,
                Is.EqualTo("777"));

            Assert.That(productDeveloper.Developer.Name,
                Is.EqualTo("FlightFactor"));

            Assert.That(productDeveloper.Role,
                Is.EqualTo("Lead Developer"));

            Assert.That(productDeveloper.ProductId,
                Is.EqualTo(1));

            Assert.That(productDeveloper.DeveloperId,
                Is.EqualTo(2));
        });
    }
}