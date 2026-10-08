using FSEI.Domain.Products;
using FSEI.Domain.UserCollections;
using NUnit.Framework;

namespace FSEI.Domain.Tests.UserCollections;

[TestFixture]
public class UserProductTests
{
    [Test]
    public void UserProduct_WithMatchingCatalogProduct_IsValid()
    {
        var productType = new ProductType
        {
            ProductTypeId = 1,
            Code = "AIRCRAFT",
            Name = "Aircraft"
        };

        var product = new Product
        {
            ProductId = 10,
            ProductTypeId = productType.ProductTypeId,
            Name = "FlightFactor 777",
            ProductType = productType
        };

        var userProduct = new UserProduct
        {
            UserProductId = 1,
            ProductId = product.ProductId,
            Name = "My FlightFactor 777",
            Product = product
        };

        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(() => userProduct.Validate());
            Assert.That(userProduct.ProductId, Is.EqualTo(product.ProductId));
            Assert.That(userProduct.Product, Is.SameAs(product));
        });
    }
    [Test]
    public void UserProduct_WithoutCatalogProduct_IsValid()
    {
        var userProduct = new UserProduct
        {
            UserProductId = 2,
            Name = "Legacy Aircraft Add-on"
        };

        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(() => userProduct.Validate());
            Assert.That(userProduct.Name,
                Is.EqualTo("Legacy Aircraft Add-on"));

            Assert.That(userProduct.ProductId,
                Is.Null);

            Assert.That(userProduct.Product,
                Is.Null);
        });
    }
    [Test]
    public void Validate_WhenCatalogProductDoesNotMatch_ThrowsInvalidOperationException()
    {
        var productType = new ProductType
        {
            ProductTypeId = 1,
            Code = "AIRCRAFT",
            Name = "Aircraft"
        };

        var product = new Product
        {
            ProductId = 10,
            ProductTypeId = productType.ProductTypeId,
            Name = "FlightFactor 777",
            ProductType = productType
        };

        var userProduct = new UserProduct
        {
            UserProductId = 1,
            ProductId = 99,
            Name = "My FlightFactor 777",
            Product = product
        };

        Assert.Throws<InvalidOperationException>(
            () => userProduct.Validate());
    }
    
    [Test]
    public void Validate_WhenProductIdExistsButProductIsNull_DoesNotThrow()
    {
        var userProduct = new UserProduct
        {
            UserProductId = 3, 
            ProductId = 10,
            Name = "My Boeing 777"
        };

        Assert.DoesNotThrow(() => userProduct.Validate());
    }
    [Test]
    public void Validate_WhenProductExistsWithoutProductId_ThrowsInvalidOperationException()
    {
        var userProduct = new UserProduct
        {
            UserProductId = 4,
            Name = "My Boeing 777",
            Product = new Product
            {
                ProductId = 10,
                ProductTypeId = 1,
                Name = "FlightFactor 777",
                ProductType = new ProductType
                {
                    ProductTypeId = 1,
                    Code = "AIRCRAFT",
                    Name = "Aircraft"
                }
            }
        };

        Assert.Throws<InvalidOperationException>(
            () => userProduct.Validate());
    }
}