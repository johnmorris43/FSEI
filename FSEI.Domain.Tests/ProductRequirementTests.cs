using FSEI.Domain.Products;

namespace FSEI.Domain.Tests.Products;

public class ProductRequirementTests
{
    [Test]
    public void Validate_WhenRequiredProductIsDifferent_DoesNotThrow()
    {
        var product = CreateProduct(1, "757 Extended Upgrade");
        var requiredProduct = CreateProduct(2, "757 Base Product");

        var group = CreateRequirementGroup(product);

        var requirement = new ProductRequirement
        {
            ProductRequirementGroupId = group.ProductRequirementGroupId,
            RequiredProductId = requiredProduct.ProductId,
            ProductRequirementGroup = group,
            RequiredProduct = requiredProduct
        };

        Assert.DoesNotThrow(() => requirement.Validate());
    }

    [Test]
    public void Validate_WhenProductRequiresItself_ThrowsInvalidOperationException()
    {
        var product = CreateProduct(1, "757 Extended Upgrade");

        var group = CreateRequirementGroup(product);

        var requirement = new ProductRequirement
        {
            ProductRequirementGroupId = group.ProductRequirementGroupId,
            RequiredProductId = product.ProductId,
            ProductRequirementGroup = group,
            RequiredProduct = product
        };

        var exception = Assert.Throws<InvalidOperationException>(
            () => requirement.Validate());

        Assert.That(
            exception!.Message,
            Is.EqualTo("A product cannot require itself."));
    }

    private static ProductRequirementGroup CreateRequirementGroup(Product product)
    {
        return new ProductRequirementGroup
        {
            ProductRequirementGroupId = 1,
            ProductId = product.ProductId,
            Product = product
        };
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