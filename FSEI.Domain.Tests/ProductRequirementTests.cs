
using FSEI.Domain.Products;

namespace FSEI.Domain.Tests.Products;

public class ProductRequirementTests
{
    [Test]
    public void Validate_WhenRequiredProductIsDifferent_DoesNotThrow()
    {
        var product = CreateProduct(1, "757 Extended Upgrade");
        var requiredProduct = CreateProduct(2, "757 Base Product");

        var requirement = new ProductRequirement
        {
            ProductId = product.ProductId,
            RequiredProductId = requiredProduct.ProductId,
            Product = product,
            RequiredProduct = requiredProduct
        };

        Assert.DoesNotThrow(() => requirement.Validate());
    }

    [Test]
    public void Validate_WhenProductRequiresItself_ThrowsInvalidOperationException()
    {
        var product = CreateProduct(1, "757 Extended Upgrade");

        var requirement = new ProductRequirement
        {
            ProductId = product.ProductId,
            RequiredProductId = product.ProductId,
            Product = product,
            RequiredProduct = product
        };

        var exception = Assert.Throws<InvalidOperationException>(
            () => requirement.Validate());

        Assert.That(
            exception!.Message,
            Is.EqualTo("A product cannot require itself."));
    }

    private static Product CreateProduct(int id, string name)
    {
        return new Product
        {
            ProductId = id,
            ProductTypeId = 1,
            Name = name,
            ProductType = null!
        };
    }
}
