using FSEI.Domain.Products;

namespace FSEI.Domain.Tests;

[TestFixture]
public class ProductRequirementGroupTests
{
    [Test]
    public void IsSatisfiedBy_WhenOneRequiredProductIsOwned_ReturnsTrue()
    {
        var product = CreateProduct(1, "777 Upgrade");
        var qualifyingProductA = CreateProduct(2, "777 Base Package A");
        var qualifyingProductB = CreateProduct(3, "777 Base Package B");

        var group = CreateRequirementGroup(product);

        group.Requirements.Add(CreateRequirement(group, qualifyingProductA));
        group.Requirements.Add(CreateRequirement(group, qualifyingProductB));

        var ownedProductIds = new[] { qualifyingProductB.ProductId };

        var result = group.IsSatisfiedBy(ownedProductIds);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSatisfiedBy_WhenNoRequiredProductIsOwned_ReturnsFalse()
    {
        var product = CreateProduct(1, "777 Upgrade");
        var qualifyingProductA = CreateProduct(2, "777 Base Package A");
        var qualifyingProductB = CreateProduct(3, "777 Base Package B");

        var group = CreateRequirementGroup(product);

        group.Requirements.Add(CreateRequirement(group, qualifyingProductA));
        group.Requirements.Add(CreateRequirement(group, qualifyingProductB));

        var ownedProductIds = new[] { 99 };

        var result = group.IsSatisfiedBy(ownedProductIds);

        Assert.That(result, Is.False);
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

    private static ProductRequirement CreateRequirement(
        ProductRequirementGroup group,
        Product requiredProduct)
    {
        return new ProductRequirement
        {
            ProductRequirementGroupId = group.ProductRequirementGroupId,
            RequiredProductId = requiredProduct.ProductId,
            ProductRequirementGroup = group,
            RequiredProduct = requiredProduct
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
    
    [Test]
    public void Validate_WhenGroupHasNoRequirements_ThrowsInvalidOperationException()
    {
        var product = CreateProduct(1, "777 Upgrade");

        var group = CreateRequirementGroup(product);

        var exception = Assert.Throws<InvalidOperationException>(
            () => group.Validate());

        Assert.That(
            exception!.Message,
            Is.EqualTo(
                "A product requirement group must contain at least one requirement."));
    }
}