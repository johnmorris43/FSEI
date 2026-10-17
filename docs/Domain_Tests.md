# FSEI V2 — Domain Unit Test Documentation

## 1. Introduction

### 1.1 Purpose

This document describes the unit-testing architecture,
test organization, and verified test scenarios for
the Flight Simulator Ecosystem Index (FSEI) V2.

The primary purpose of the domain test suite is to
verify the behavior of domain entities, business rules,
validation methods, and relationships.

This documentation serves as a technical reference
for developers maintaining and extending FSEI V2.

### 1.2 Scope

This document covers the FSEI.Domain.Tests project.

The documentation includes:

- Testing framework and configuration.
- Test project dependencies.
- Organization of domain test classes.
- Individual test methods and scenarios.
- Expected test behavior.
- Business rules verified by tests.
- Identified gaps in test coverage.
- Test execution and maintenance procedures.

Test behavior will be documented directly from
the existing C# test implementations.

Potential improvements will be recorded separately
from currently implemented functionality.

### 1.3 Related Documentation

- `Domain_Model.md`
  Documents domain entities, properties,
  relationships, validation, and business rules.

- `Domain_Tests.md`
  Documents the automated tests used to verify
  domain behavior.

These documents provide complementary references
for the FSEI V2 domain layer.


## 2. Testing Framework and Architecture

### 2.1 Test Project

**Project:** FSEI.Domain.Tests

**Project File:** FSEI.Domain.Tests.csproj

**Project Type:** .NET Class Library

**Target Framework:** .NET 10

**Language:** C#

**Language Version:** Latest

**Nullable Reference Types:** Enabled

**Implicit Usings:** Enabled

**NuGet Packaging:** Disabled

The test project references the FSEI.Domain
project directly.

This allows unit tests to access domain entities,
business rules, and validation methods.


### 2.2 Testing Framework

FSEI V2 uses NUnit as its unit-testing framework.

**Framework:** NUnit

**Version:** 4.3.2

NUnit provides the testing infrastructure
for defining and executing automated tests.

The project also includes the NUnit test adapter
for integration with compatible .NET test runners.


### 2.3 Test Dependencies

The following packages are defined in
FSEI.Domain.Tests.csproj.

| Package | Version | Purpose |
|---|---|---|
| NUnit | 4.3.2 | Unit-testing framework |
| NUnit3TestAdapter | 5.0.0 | Test discovery and execution adapter |
| NUnit.Analyzers | 4.7.0 | NUnit code analysis |
| Microsoft.NET.Test.Sdk | 17.14.0 | .NET test infrastructure |
| coverlet.collector | 6.0.4 | Code coverage collection |

These versions represent the dependencies
declared in the test project configuration.


### 2.4 Project Configuration

The test project enables:

- Nullable reference type analysis.
- Implicit global using directives.
- Latest available C# language version.
- Direct references to FSEI.Domain.
- NUnit.Framework as a global using.
- Code coverage collection through Coverlet.

The project is configured with
IsPackable set to false.


### 2.5 Test Architecture

The domain testing architecture consists
of two primary projects.

FSEI.Domain
|
| Referenced by
|
FSEI.Domain.Tests

FSEI.Domain contains the domain entities
and their implemented business logic.

FSEI.Domain.Tests contains automated tests
intended to verify that behavior.

The test project references the domain project
without requiring changes to production code.


## 3. Test Organization

### 3.1 Directory Structure

The test project organizes test classes into
directories corresponding to domain areas.

The following directories and test classes
were identified during the initial review.

**Stores**
- PriceObservationTests
- StorefrontPaymentMethodTests
- OfferOptionTests

**Products**
- ProductRequirementGroupTests
- ProductDeveloperTests
- ProductRequirementTests

**Simulators**
- SimulatorOperatingSystemCompatibilityTests

**GeographicRegions**
- CurrencyTest
- MarketTest

**Evaluations**
- EvaluationComponentAssessmentTests
- SystemsDepthEvaluationTests

**Avionics**
- AvionicsEvaluationTests

**UserCollections**
- UserProductTests

Additional test directories or classes may
be incorporated as the project develops.


### 3.2 Documentation Methodology

Each test class will be documented individually.

Documentation will include:

- Test class name.
- Namespace and source file.
- Purpose of the test class.
- Domain entity or behavior under test.
- Individual test methods.
- Test setup and input conditions.
- Expected results.
- Business rules verified.
- Relevant implementation observations.

Documentation will distinguish between
implemented tests and recommended future tests.


### 3.3 Verification Status

The test project configuration has been reviewed.

Individual test implementations have not yet
been reviewed as part of this documentation phase.

Test execution results and coverage percentages
will be recorded only after verification.

The presence of a test class does not, by itself,
establish that all associated business rules
are covered.


## 4. Domain Test Documentation

### 4.1 Products

The Products test suite verifies business rules
associated with product relationships, dependencies,
and requirement validation.

The documented tests exercise domain behavior
using NUnit assertions.

### 4.1.1 ProductRequirementGroupTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `Products/ProductRequirementGroupTests.cs`

**Framework:** NUnit

**Test Fixture:** ProductRequirementGroupTests

**Domain Class:** ProductRequirementGroup

**Number of Tests:** 4

**Purpose:**

Verifies the behavior of product requirement groups.

Tests determine whether owned products satisfy
defined prerequisites and whether invalid
requirement groups are rejected.

The test class focuses on two domain methods:

- `IsSatisfiedBy()`
- `Validate()`

**Test Methods:**

#### Test 1 — IsSatisfiedBy_WhenOneRequiredProductIsOwned_ReturnsTrue

**Purpose:**

Verifies that a requirement group is satisfied
when at least one qualifying product is owned.

**Arrange:**

- Create a product named "777 Upgrade".
- Create qualifying products A and B.
- Create a requirement group for the upgrade.
- Add both qualifying products as requirements.
- Supply an owned-product collection containing
  only qualifying product B.

**Act:**

Call `IsSatisfiedBy()` with the owned-product IDs.

**Expected Result:**

- The method returns true.
- Ownership of one qualifying product is sufficient.

**Business Rule Verified:**

A requirement group uses OR logic when evaluating
its individual product requirements.


#### Test 2 — IsSatisfiedBy_WhenNoRequiredProductIsOwned_ReturnsFalse

**Purpose:**

Verifies that a requirement group is not satisfied
when none of its qualifying products are owned.

**Arrange:**

- Create a product named "777 Upgrade".
- Create qualifying products A and B.
- Create a requirement group containing both.
- Supply an owned-product collection containing
  product ID 99.

**Act:**

Call `IsSatisfiedBy()` with the owned-product IDs.

**Expected Result:**

- The method returns false.

**Business Rule Verified:**

A requirement group cannot be satisfied unless
at least one qualifying product is owned.


#### Test 3 — Validate_WhenGroupHasNoRequirements_ThrowsInvalidOperationException

**Purpose:**

Verifies that an empty requirement group fails
domain validation.

**Arrange:**

- Create a product named "777 Upgrade".
- Create a requirement group for the product.
- Leave the Requirements collection empty.

**Act:**

Call `Validate()`.

**Expected Result:**

An InvalidOperationException is thrown.

The exception message must equal:

"A product requirement group must contain at least one requirement."

**Business Rule Verified:**

A valid product requirement group must contain
at least one individual product requirement.


#### Test 4 — Validate_WhenProductRequiresItself_ThrowsInvalidOperationException

**Purpose:**

Verifies that requirement group validation rejects
a direct product self-dependency.

**Arrange:**

- Create a product named "777 Upgrade".
- Create a requirement group for the product.
- Add a requirement referencing the same product.

**Act:**

Call `Validate()`.

**Expected Result:**

An InvalidOperationException is thrown.

The test does not verify the exception message.

**Business Rule Verified:**

A product cannot directly require itself.

Requirement group validation invokes validation
on its individual ProductRequirement entities.


**Test Helpers:**

The class contains three private static helper methods.

- `CreateProduct()`
  Creates a Product with the supplied identifier
  and name.

- `CreateRequirementGroup()`
  Creates a ProductRequirementGroup associated
  with the supplied product.

- `CreateRequirement()`
  Creates a ProductRequirement linking a requirement
  group to a required product.

These helpers reduce repeated object initialization
across the test methods.


**Coverage Summary:**

The existing tests verify:

- Successful requirement satisfaction.
- Unsuccessful requirement satisfaction.
- Rejection of empty requirement groups.
- Rejection of direct self-dependencies.

**Coverage Gaps:**

The following scenarios are not directly tested
by this class:

- A valid, nonempty group passing Validate().
- IsSatisfiedBy() with an empty Requirements collection.
- IsSatisfiedBy() with an empty owned-product collection.
- Indirect circular dependencies.
- Duplicate requirements within a group.
- Multiple owned qualifying products.
- Null input to IsSatisfiedBy().
- Validation of foreign-key and navigation-property
  consistency.

These are observations for future review,
not failures of the existing tests.

**Implementation Notes:**

- The class uses the NUnit TestFixture attribute.
- Each test method uses the NUnit Test attribute.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- Tests exercise synchronous domain methods.
- Test execution results have not yet been
  independently verified during this
  documentation review.


### 4.1.2 ProductRequirementTests

**Namespace:** `FSEI.Domain.Tests.Products`

**Source:** `Products/ProductRequirementTests.cs`

**Framework:** NUnit

**Test Class:** ProductRequirementTests

**Domain Class:** ProductRequirement

**Number of Tests:** 2

**Purpose:**

Verifies the validation behavior of individual
product requirements within the FSEI domain model.

Ensures that a product may reference another
product as a prerequisite while preventing
direct self-dependencies.

The tests focus on the ProductRequirement
Validate() method.


**Test Methods:**

#### Test 1 — Validate_WhenRequiredProductIsDifferent_DoesNotThrow

**Purpose:**

Verifies that a product requirement passes
validation when the required product differs
from the product associated with the
requirement group.

**Arrange:**

- Create a product with ID 1 named
  "757 Extended Upgrade".
- Create a required product with ID 2 named
  "757 Base Product".
- Create a requirement group associated
  with the upgrade product.
- Create a ProductRequirement linking
  the group to the base product.
- Assign the required navigation properties.

**Act:**

Call ProductRequirement.Validate().

**Expected Result:**

- Validate() completes without throwing
  an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

A product may require a different product
as a prerequisite.

The validation method permits requirements
where the parent product identifier differs
from the required product identifier.


#### Test 2 — Validate_WhenProductRequiresItself_ThrowsInvalidOperationException

**Purpose:**

Verifies that product requirement validation
rejects direct self-dependencies.

**Arrange:**

- Create a product with ID 1 named
  "757 Extended Upgrade".
- Create a requirement group associated
  with the product.
- Create a ProductRequirement referencing
  the same product as its prerequisite.
- Assign the required navigation properties.

**Act:**

Call ProductRequirement.Validate().

**Expected Result:**

- An InvalidOperationException is thrown.
- The exception message matches the
  expected validation message.

**Expected Exception Message:**

"A product cannot require itself."

**Assertions:**

- Assert.Throws<InvalidOperationException>()
- Assert.That(exception.Message, Is.EqualTo(...))

**Business Rule Verified:**

A product cannot directly require itself.

ProductRequirement.Validate() must reject
requirements where the parent product ID
matches the required product ID.


**Test Helpers:**

The class contains two private static
helper methods.

#### CreateRequirementGroup()

**Purpose:**

Creates a ProductRequirementGroup associated
with a supplied Product.

**Behavior:**

- Assigns ProductRequirementGroupId = 1.
- Assigns ProductId from the supplied product.
- Initializes the required Product navigation.

#### CreateProduct()

**Purpose:**

Creates a Product using a supplied identifier
and name.

**Behavior:**

- Assigns ProductId.
- Assigns ProductTypeId = 1.
- Assigns Name.
- Initializes ProductType using null!.

The null-forgiving operator satisfies the
required-member initialization requirement
without constructing a ProductType instance.

These helpers support focused testing of
ProductRequirement validation.


**Coverage Summary:**

The existing tests verify:

- Successful validation when a product
  requires a different product.
- Rejection of direct self-dependencies.
- The exception type generated by
  invalid self-dependencies.
- The exact exception message associated
  with direct self-dependency validation.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Indirect circular product dependencies.
- Foreign-key and navigation-property
  consistency.
- Missing or null navigation properties.
- Duplicate product requirements.
- Requirements referencing nonexistent
  product identifiers.
- Requirement validation involving
  multiple requirement groups.

These observations identify potential
future test scenarios.

They do not indicate failures in the
existing tests.


**Implementation Notes:**

- The class contains two NUnit tests.
- Each test method uses the Test attribute.
- No explicit TestFixture attribute is used.
- NUnit supports test discovery without
  requiring an explicit TestFixture attribute
  for this class.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- Both tests exercise synchronous validation.
- The tests focus on direct product
  dependency validation.
- Test execution results have not been
  independently verified during this
  documentation review.

### 4.1.3 ProductDeveloperTests

**Namespace:** `FSEI.Domain.Tests.Products`

**Source:** `Products/ProductDeveloperTests.cs`

**Framework:** NUnit

**Test Fixture:** ProductDeveloperTests

**Domain Class:** ProductDeveloper

**Number of Tests:** 1

**Purpose:**

Verifies the relationship between a Product
and a Developer within the FSEI domain model.

Ensures that ProductDeveloper correctly
maintains references to the associated
product and developer.

Also verifies that the developer's role
and relationship identifiers retain
their assigned values.


**Test Methods:**

#### Test 1 — ProductDeveloper_ShouldLinkProductAndDeveloper

**Purpose:**

Verifies that a ProductDeveloper entity
correctly associates a product with its
developer and stores the developer's role.

**Arrange:**

Create a ProductType with:

- Code = "AIRCRAFT"
- Name = "Aircraft"

Create a Product with:

- Name = "777"
- ProductType = previously created ProductType

Create a Developer with:

- Name = "FlightFactor"

Create a ProductDeveloper with:

- ProductId = 1
- DeveloperId = 2
- Product = previously created Product
- Developer = previously created Developer
- Role = "Lead Developer"

**Act:**

Access the properties of the
ProductDeveloper entity.

No domain method is invoked.

**Expected Results:**

The ProductDeveloper entity must retain
the following values:

1. Product.Name equals "777".
2. Developer.Name equals "FlightFactor".
3. Role equals "Lead Developer".
4. ProductId equals 1.
5. DeveloperId equals 2.

**Assertions:**

The test uses NUnit Assert.Multiple()
to evaluate five assertions.

- Assert.That(Product.Name, Is.EqualTo("777"))
- Assert.That(Developer.Name, Is.EqualTo("FlightFactor"))
- Assert.That(Role, Is.EqualTo("Lead Developer"))
- Assert.That(ProductId, Is.EqualTo(1))
- Assert.That(DeveloperId, Is.EqualTo(2))

**Business Behavior Verified:**

A ProductDeveloper entity can maintain
references to both a Product and a Developer.

The relationship can also retain a
developer role and the associated
product and developer identifiers.


**Test Helpers:**

No private helper methods are implemented
in this test class.

All required domain entities are
constructed directly within the test.


**Coverage Summary:**

The existing test verifies:

- Product navigation property assignment.
- Developer navigation property assignment.
- Developer role assignment.
- ProductId property assignment.
- DeveloperId property assignment.
- Multiple assertions within one test.

**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Null Product navigation references.
- Null Developer navigation references.
- Empty or whitespace-only developer roles.
- Missing or invalid product identifiers.
- Missing or invalid developer identifiers.
- Foreign-key and navigation-property consistency.
- Duplicate product-developer relationships.
- Multiple developers associated with one product.
- One developer associated with multiple products.
- Database persistence of product-developer
  relationships.

These are potential future test scenarios.

They do not indicate failures in the
existing implementation.


**Implementation Notes:**

- The class uses the NUnit TestFixture attribute.
- The test method uses the NUnit Test attribute.
- The test follows the Arrange/Assert pattern.
- No separate Act operation is implemented.
- Assert.Multiple() groups five assertions.
- Domain entities are constructed directly.
- No database operations are performed.
- No mocking framework is used.
- No asynchronous operations are performed.
- The test verifies assigned property values
  rather than invoking domain validation.
- Test execution results have not been
  independently verified during this
  documentation review.

ProductType
|
└── Aircraft
|
└── Product: 777
|
└── ProductDeveloper
|
├── Developer: FlightFactor
|
└── Role: Lead Developer


### 4.2 Stores

The Stores test suite verifies business rules
associated with commercial offers, pricing,
payment methods, and purchasing options.

These tests help ensure that commercial data
maintains valid relationships across products,
storefronts, currencies, and geographic markets.

### 4.2.1 PriceObservationTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `Stores/PriceObservationTests.cs`

**Framework:** NUnit

**Test Class:** PriceObservationTests

**Domain Class:** PriceObservation

**Number of Tests:** 20

**Purpose:**

Verifies validation behavior for price observations
within the FSEI commercial domain.

Ensures that recorded prices maintain consistent
relationships with commercial offers, currencies,
markets, and purchasing options.

Also verifies price amount and observation
timestamp requirements.

The tests focus on the PriceObservation.Validate()
method.


**Test Group 1 — Relationship Validation**

#### Test 1 — Validate_WhenRelationshipsMatch_DoesNotThrow

Verifies that a price observation with matching
commercial offer and currency relationships
passes validation.

Expected Result:
- No exception is thrown.


#### Test 2 — Validate_WhenCommercialOfferDoesNotMatch_ThrowsInvalidOperationException

Verifies that a mismatched CommercialOfferId
causes validation to fail.

Expected Result:
- InvalidOperationException is thrown.


#### Test 3 — Validate_WhenCurrencyDoesNotMatch_ThrowsInvalidOperationException

Verifies that a mismatched CurrencyId
causes validation to fail.

Expected Result:
- InvalidOperationException is thrown.


**Test Group 2 — Price Amount Validation**

#### Test 4 — Validate_WhenAmountIsZero_DoesNotThrow

Verifies that a price observation may contain
an amount of zero.

Expected Result:
- No exception is thrown.

Business Rule:
- Zero-valued prices are permitted.


#### Test 5 — Validate_WhenAmountIsNegative_ThrowsInvalidOperationException

Verifies that negative price amounts
are rejected.

Expected Result:
- InvalidOperationException is thrown.

Business Rule:
- Price amounts cannot be negative.


**Test Group 3 — Observation Timestamp**

#### Test 6 — Validate_WhenObservedAtIsProvided_DoesNotThrow

Verifies that a price observation containing
a valid observation timestamp passes validation.

Expected Result:
- No exception is thrown.


#### Test 7 — Validate_WhenObservedAtIsNotProvided_ThrowsInvalidOperationException

Verifies that an uninitialized observation
timestamp causes validation to fail.

Expected Result:
- InvalidOperationException is thrown.

Business Rule:
- Price observations require an observation timestamp.


**Test Group 4 — Multiple Currencies**

#### Test 8 — Validate_SameCommercialOfferCanHaveObservationsInDifferentCurrencies

Verifies that the same commercial offer
can have price observations in different currencies.

Test Data:

- EUR observation: 79.99
- JPY observation: 9700

Expected Results:

- Both observations pass validation.
- Both reference the same commercial offer.
- Their currency identifiers differ.

Business Rule:
- A commercial offer may have price observations
  recorded in multiple currencies.


**Test Group 5 — Geographic Market Validation**

#### Test 9 — Validate_WhenMarketIsNotProvided_DoesNotThrow

Verifies that market information is optional.

Expected Result:
- No exception is thrown.


#### Test 10 — Validate_WhenMarketMatches_DoesNotThrow

Verifies that matching MarketId and Market
navigation references pass validation.

Expected Result:
- No exception is thrown.


#### Test 11 — Validate_WhenMarketIdDoesNotMatchMarket_ThrowsInvalidOperationException

Verifies that inconsistent market identifiers
cause validation to fail.

Expected Result:
- InvalidOperationException is thrown.


#### Test 12 — Validate_WhenMarketExistsWithoutMarketId_ThrowsInvalidOperationException

Verifies that a Market navigation property
without its corresponding MarketId is rejected.

Expected Result:
- InvalidOperationException is thrown.


#### Test 13 — Validate_WhenMarketIdExistsWithoutMarket_ThrowsInvalidOperationException

Verifies that a MarketId without its
corresponding Market navigation property
is rejected.

Expected Result:
- InvalidOperationException is thrown.


**Test Group 6 — Offer Option Validation**

#### Test 14 — Validate_WhenOfferOptionIsNotProvided_DoesNotThrow

Verifies that a price observation may exist
without an associated OfferOption.

Expected Result:
- No exception is thrown.


#### Test 15 — Validate_WhenOfferOptionMatches_DoesNotThrow

Verifies that matching OfferOptionId and
OfferOption navigation references pass validation.

Expected Result:
- No exception is thrown.


#### Test 16 — Validate_WhenOfferOptionIdDoesNotMatchOfferOption_ThrowsInvalidOperationException

Verifies that mismatched OfferOption identifiers
cause validation to fail.

Expected Result:
- InvalidOperationException is thrown.


#### Test 17 — Validate_WhenOfferOptionBelongsToDifferentCommercialOffer_ThrowsInvalidOperationException

Verifies that an OfferOption associated with
a different CommercialOffer is rejected.

Expected Result:
- InvalidOperationException is thrown.

Business Rule:
- An OfferOption associated with a price observation
  must belong to the observation's CommercialOffer.


#### Test 18 — Validate_WhenOfferOptionExistsWithoutOfferOptionId_ThrowsInvalidOperationException

Verifies that an OfferOption navigation property
without its corresponding identifier is rejected.

Expected Result:
- InvalidOperationException is thrown.


#### Test 19 — Validate_SameCommercialOfferCanHaveMonthlyAndAnnualPricedOptions

Verifies that one commercial offer can support
different purchasing options with separate prices.

Test Data:

Monthly Subscription:
- OfferOptionId = 1
- Amount = 9.05

Annual Subscription:
- OfferOptionId = 2
- Amount = 81.64

Expected Results:

- Both observations pass validation.
- Both reference the same commercial offer.
- Their OfferOption identifiers differ.
- Their price amounts differ.

Business Rule:
- A commercial offer may support multiple
  purchasing options with different prices.


**Additional Test Identified:**

#### Test 20 — Validate_WhenOfferOptionIdExistsWithoutOfferOption_ThrowsInvalidOperationException

Verifies that an OfferOptionId without its
corresponding OfferOption navigation property
is rejected.

Expected Result:
- InvalidOperationException is thrown.


**Test Helpers:**

The class contains three private static
helper methods.

#### CreatePriceObservation()

Constructs a valid baseline PriceObservation
with associated:

- ProductType
- Product
- Storefront
- CommercialOffer
- Currency

Default test values include:

- Amount = 69.99
- Currency = USD
- ObservedAt = DateTime.UtcNow


#### CreateMarket()

Constructs a Market associated with a Region.

Used to test geographic market relationships.


#### CreateAcquisitionType()

Constructs an AcquisitionType with:

- AcquisitionTypeId = 1
- Code = PURCHASE
- Name = Purchase

Used when creating OfferOption test entities.


**Coverage Summary:**

The existing tests verify:

- Commercial offer relationship consistency.
- Currency relationship consistency.
- Zero and negative price handling.
- Required observation timestamps.
- Multiple currency observations.
- Optional geographic markets.
- Market identifier consistency.
- Optional purchasing options.
- OfferOption identifier consistency.
- Commercial offer ownership of OfferOptions.
- Multiple purchasing options with different prices.


**Coverage Gaps:**

Potential additional test scenarios include:

- Future-dated observation timestamps.
- Extremely large price amounts.
- Duplicate price observations.
- Currency conversion calculations.
- Market-specific tax calculations.
- Historical price comparison behavior.
- Price observation persistence.
- Validation of additional commercial
  pricing constraints.

These are future review considerations
rather than failures of the existing tests.


**Implementation Notes:**

- Tests use NUnit Test attributes.
- No explicit TestFixture attribute is declared.
- Domain entities are constructed directly.
- No database operations are performed.
- No mocking framework is used.
- Several tests use Assert.Multiple().
- Validation methods are invoked synchronously.
- Test execution results have not been
  independently verified during this review.

### 4.2.2 StorefrontPaymentMethodTests

**Namespace:** `FSEI.Domain.Tests.Stores`

**Source:** `Stores/StorefrontPaymentMethodTests.cs`

**Framework:** NUnit

**Test Fixture:** StorefrontPaymentMethodTests

**Domain Class:** StorefrontPaymentMethod

**Number of Tests:** 5

**Purpose:**

Verifies the relationship between storefronts
and their supported payment methods.

Ensures that StorefrontPaymentMethod correctly
maintains relationship identifiers and validates
their consistency with associated navigation
properties.

Also verifies default activation behavior
and validation when navigation properties
are not populated.

The tests focus on:

- StorefrontPaymentMethod entity initialization.
- StorefrontPaymentMethod.Validate()


**Test Methods:**

#### Test 1 — StorefrontPaymentMethod_WithValidReferences_IsCreated

**Purpose:**

Verifies that a StorefrontPaymentMethod entity
can be initialized with valid storefront
and payment method references.

**Arrange:**

Create a Storefront with:

- StorefrontId = 1
- Name = "X-Plane.org Store"

Create a PaymentMethod with:

- PaymentMethodId = 1
- Code = "PAYPAL"
- Name = "PayPal"

Create a StorefrontPaymentMethod with:

- StorefrontPaymentMethodId = 1
- StorefrontId = 1
- PaymentMethodId = 1
- Storefront = previously created Storefront
- PaymentMethod = previously created PaymentMethod

**Act:**

Access the initialized entity properties.

No validation method is invoked.

**Expected Results:**

- StorefrontId matches Storefront.StorefrontId.
- PaymentMethodId matches PaymentMethod.PaymentMethodId.
- IsActive is true.

**Assertions:**

The test uses Assert.Multiple() to verify
three conditions:

- StorefrontId equals 1.
- PaymentMethodId equals 1.
- IsActive is true.

**Business Behavior Verified:**

A storefront payment method relationship
can retain matching entity identifiers.

New relationships are active by default.


#### Test 2 — Validate_WhenStorefrontIdDoesNotMatch_ThrowsInvalidOperationException

**Purpose:**

Verifies that inconsistent storefront
identifiers cause validation to fail.

**Arrange:**

Create a Storefront with:

- StorefrontId = 10
- Name = "X-Plane.org Store"

Create a PaymentMethod with:

- PaymentMethodId = 1
- Code = "PAYPAL"
- Name = "PayPal"

Create a StorefrontPaymentMethod with:

- StorefrontId = 99
- PaymentMethodId = 1
- Storefront = previously created Storefront
- PaymentMethod = previously created PaymentMethod

**Act:**

Call StorefrontPaymentMethod.Validate().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

When a Storefront navigation property is
provided, StorefrontId must match the
referenced storefront's identifier.


#### Test 3 — Validate_WhenPaymentMethodIdDoesNotMatch_ThrowsInvalidOperationException

**Purpose:**

Verifies that inconsistent payment method
identifiers cause validation to fail.

**Arrange:**

Create a Storefront with:

- StorefrontId = 10
- Name = "X-Plane.org Store"

Create a PaymentMethod with:

- PaymentMethodId = 1
- Code = "PAYPAL"
- Name = "PayPal"

Create a StorefrontPaymentMethod with:

- StorefrontId = 10
- PaymentMethodId = 99
- Storefront = previously created Storefront
- PaymentMethod = previously created PaymentMethod

**Act:**

Call StorefrontPaymentMethod.Validate().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

When a PaymentMethod navigation property
is provided, PaymentMethodId must match
the referenced payment method's identifier.


#### Test 4 — Validate_WhenReferencesMatch_DoesNotThrow

**Purpose:**

Verifies that validation succeeds when
storefront and payment method identifiers
match their navigation properties.

**Arrange:**

Create a Storefront with:

- StorefrontId = 10
- Name = "X-Plane.org Store"

Create a PaymentMethod with:

- PaymentMethodId = 1
- Code = "PAYPAL"
- Name = "PayPal"

Create a StorefrontPaymentMethod with:

- StorefrontId = 10
- PaymentMethodId = 1
- Storefront = previously created Storefront
- PaymentMethod = previously created PaymentMethod

**Act:**

Call StorefrontPaymentMethod.Validate().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

Consistent storefront and payment method
relationships pass domain validation.


#### Test 5 — Validate_WhenNavigationPropertiesAreNull_DoesNotThrow

**Purpose:**

Verifies that StorefrontPaymentMethod.Validate()
does not require populated navigation properties.

**Arrange:**

Create a StorefrontPaymentMethod with:

- StorefrontPaymentMethodId = 1
- StorefrontId = 10
- PaymentMethodId = 1

Do not assign the Storefront or
PaymentMethod navigation properties.

**Act:**

Verify that both navigation properties
are null.

Call StorefrontPaymentMethod.Validate().

**Expected Results:**

- Storefront is null.
- PaymentMethod is null.
- Validate() completes without throwing
  an exception.

**Assertions:**

The test uses Assert.Multiple() to verify:

- Storefront is null.
- PaymentMethod is null.
- Assert.DoesNotThrow() succeeds.

**Business Behavior Verified:**

The current validation implementation
permits unpopulated navigation properties.

Relationship consistency checks do not
require both navigation properties
to be initialized.


**Test Helpers:**

No private helper methods are implemented
in this test class.

Each test constructs its required
domain entities directly.


**Coverage Summary:**

The existing tests verify:

- StorefrontPaymentMethod initialization.
- Storefront identifier consistency.
- Payment method identifier consistency.
- Successful validation with matching references.
- Default IsActive behavior.
- Validation with null navigation properties.
- Multiple assertions within individual tests.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Zero or negative storefront identifiers.
- Zero or negative payment method identifiers.
- Duplicate storefront-payment method relationships.
- Deactivation of existing relationships.
- Reactivation of inactive relationships.
- Validation when only Storefront is null.
- Validation when only PaymentMethod is null.
- Database persistence of relationships.
- Foreign-key enforcement by a database.

These are potential future test scenarios.

They do not indicate failures in the
existing implementation.


**Implementation Notes:**

- The class uses the NUnit TestFixture attribute.
- All five test methods use the NUnit Test attribute.
- Assert.Multiple() is used in two tests.
- Three tests invoke Validate() and assert
  exception or non-exception behavior with
  populated navigation properties.
- One test invokes Validate() with null
  navigation properties.
- The initialization test does not invoke Validate().
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- Test execution results have not been
  independently verified during this
  documentation review.

### 4.2.3 OfferOptionTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `Stores/OfferOptionTests.cs`

**Framework:** NUnit

**Test Class:** OfferOptionTests

**Domain Class:** OfferOption

**Number of Tests:** 8

**Purpose:**

Verifies the validation behavior of purchasing
options associated with commercial offers.

Ensures that OfferOption maintains consistent
relationships with CommercialOffer,
AcquisitionType, and BillingInterval entities.

The tests focus on the OfferOption.Validate()
method.


**Test Group 1 — Commercial Offer Validation**

#### Test 1 — Validate_WhenCommercialOfferMatches_DoesNotThrow

**Purpose:**

Verifies that an OfferOption passes validation
when its CommercialOfferId matches the
associated CommercialOffer entity.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- The helper initializes CommercialOfferId = 1.
- The associated CommercialOffer has
  CommercialOfferId = 1.

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- Validation completes without throwing
  an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

A purchasing option with a matching
commercial offer relationship passes validation.


#### Test 2 — Validate_WhenCommercialOfferDoesNotMatch_ThrowsInvalidOperationException

**Purpose:**

Verifies that inconsistent commercial offer
identifiers cause validation to fail.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- Change CommercialOfferId to 2.
- Leave the associated CommercialOffer
  with CommercialOfferId = 1.

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

OfferOption.CommercialOfferId must match
the associated CommercialOffer identifier.


**Test Group 2 — Acquisition Type Validation**

#### Test 3 — Validate_WhenAcquisitionTypeMatches_DoesNotThrow

**Purpose:**

Verifies that an OfferOption with a matching
AcquisitionType relationship passes validation.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- The helper initializes AcquisitionTypeId = 1.
- The associated AcquisitionType has
  AcquisitionTypeId = 1.

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- No exception is thrown.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

A purchasing option with a matching
acquisition type relationship passes validation.


#### Test 4 — Validate_WhenAcquisitionTypeDoesNotMatch_ThrowsInvalidOperationException

**Purpose:**

Verifies that inconsistent acquisition type
identifiers cause validation to fail.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- Change AcquisitionTypeId to 2.
- Leave the associated AcquisitionType
  with AcquisitionTypeId = 1.

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

OfferOption.AcquisitionTypeId must match
the associated AcquisitionType identifier.


**Test Group 3 — Billing Interval Validation**

#### Test 5 — Validate_WhenBillingIntervalMatches_DoesNotThrow

**Purpose:**

Verifies that an OfferOption passes validation
when its billing interval identifier matches
the associated BillingInterval entity.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- Set BillingIntervalId = 1.
- Create a BillingInterval with:
    - BillingIntervalId = 1
    - Code = "MONTHLY"
    - Name = "Monthly"

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- No exception is thrown.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

Matching billing interval relationships
are accepted by domain validation.


#### Test 6 — Validate_WhenBillingIntervalDoesNotMatch_ThrowsInvalidOperationException

**Purpose:**

Verifies that mismatched billing interval
identifiers cause validation to fail.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- Set BillingIntervalId = 1.
- Create a BillingInterval with:
    - BillingIntervalId = 2
    - Code = "MONTHLY"
    - Name = "Monthly"

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

OfferOption.BillingIntervalId must match
the associated BillingInterval identifier.


#### Test 7 — Validate_WhenBillingIntervalIdExistsWithoutBillingInterval_ThrowsInvalidOperationException

**Purpose:**

Verifies that a billing interval identifier
without a corresponding navigation property
causes validation to fail.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- Set BillingIntervalId = 1.
- Set BillingInterval = null.

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

A billing interval identifier cannot be
provided without its associated
BillingInterval navigation property.


#### Test 8 — Validate_WhenBillingIntervalExistsWithoutBillingIntervalId_ThrowsInvalidOperationException

**Purpose:**

Verifies that a BillingInterval navigation
property without a corresponding identifier
causes validation to fail.

**Arrange:**

- Create an OfferOption using CreateOfferOption().
- Set BillingIntervalId = null.
- Create a BillingInterval with:
    - BillingIntervalId = 1
    - Code = "MONTHLY"
    - Name = "Monthly"

**Act:**

Call OfferOption.Validate().

**Expected Result:**

- InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

A BillingInterval navigation property
cannot be provided without its
corresponding BillingIntervalId.


**Test Helpers:**

The class contains one private static
helper method.

#### CreateOfferOption()

**Purpose:**

Constructs a valid baseline OfferOption
with its required domain relationships.

**Entities Created:**

- ProductType
- Product
- Storefront
- CommercialOffer
- AcquisitionType
- OfferOption

**Default Test Data:**

ProductType:
- ProductTypeId = 1
- Code = "SERVICE"
- Name = "Service"

Product:
- ProductId = 1
- ProductTypeId = 1
- Name = "Test Navigation Product"

Storefront:
- StorefrontId = 1
- Name = "Test Store"

CommercialOffer:
- CommercialOfferId = 1
- ProductId = 1
- StorefrontId = 1

AcquisitionType:
- AcquisitionTypeId = 1
- Code = "SUBSCRIPTION"
- Name = "Subscription"

OfferOption:
- OfferOptionId = 1
- CommercialOfferId = 1
- AcquisitionTypeId = 1
- Name = "Monthly Subscription"

The helper does not initialize a
BillingInterval relationship.

Individual tests configure billing
interval properties when necessary.


**Coverage Summary:**

The existing tests verify:

- Matching commercial offer relationships.
- Rejection of mismatched CommercialOfferId values.
- Matching acquisition type relationships.
- Rejection of mismatched AcquisitionTypeId values.
- Matching billing interval relationships.
- Rejection of mismatched BillingIntervalId values.
- Rejection of BillingIntervalId without BillingInterval.
- Rejection of BillingInterval without BillingIntervalId.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Validation when both BillingIntervalId
  and BillingInterval are absent.
- Null CommercialOffer navigation properties.
- Null AcquisitionType navigation properties.
- Empty or whitespace-only option names.
- Duplicate purchasing options.
- Unsupported acquisition type combinations.
- Billing interval compatibility with
  acquisition types.
- Database persistence of purchasing options.
- Database foreign-key enforcement.

These are potential future test scenarios.

They do not indicate failures in the
existing implementation.


**Implementation Notes:**

- The class contains eight NUnit tests.
- Each test method uses the Test attribute.
- No explicit TestFixture attribute is declared.
- Tests construct domain entities directly.
- One private static helper method is implemented.
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- The tests focus on relationship consistency.
- Exception messages are not asserted.
- Test execution results have not been
  independently verified during this
  documentation review.

OfferOption
|
├── CommercialOffer
|
├── AcquisitionType
|     └── Subscription
|
└── BillingInterval
├── BillingIntervalId = 1
└── Name = Monthly

## 4.3 Simulators

The Simulators test suite verifies domain rules
associated with flight simulator entities and
their compatibility relationships.

These tests help ensure that simulator-specific
compatibility information remains associated
with the correct simulator.

### 4.3.1 SimulatorOperatingSystemCompatibilityTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `Simulators/SimulatorOperatingSystemCompatibilityTests.cs`

**Framework:** NUnit

**Test Class:** SimulatorOperatingSystemCompatibilityTests

**Domain Class:** SimulatorOperatingSystemCompatibility

**Number of Tests:** 4

**Purpose:**

Verifies the validation of simulator release
boundaries associated with operating system
compatibility records.

Ensures that release boundaries reference
the same simulator as the compatibility record.

Also verifies that compatibility records
without release boundaries are accepted.

The tests focus on:

- SimulatorOperatingSystemCompatibility.ValidateReleaseBoundaries()


**Test Methods:**

#### Test 1 — ValidateReleaseBoundaries_WhenReleasesBelongToSimulator_DoesNotThrow

**Purpose:**

Verifies that release-boundary validation
succeeds when both release boundaries
belong to the simulator associated with
the compatibility record.

**Arrange:**

Create a SimulatorOperatingSystemCompatibility
entity with:

- SimulatorId = 1

Create a starting SimulatorRelease with:

- SimulatorId = 1
- Version = "11.50"

Create an ending SimulatorRelease with:

- SimulatorId = 1
- Version = "11.55"

Assign the releases to:

- SupportedFromRelease
- SupportedThroughRelease

**Act:**

Call ValidateReleaseBoundaries().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

Release boundaries associated with a
compatibility record are accepted when
both releases belong to the same simulator
identified by the compatibility record.


#### Test 2 — ValidateReleaseBoundaries_WhenFromReleaseBelongsToDifferentSimulator_ThrowsException

**Purpose:**

Verifies that a starting release belonging
to another simulator causes validation
to fail.

**Arrange:**

Create a SimulatorOperatingSystemCompatibility
entity with:

- SimulatorId = 1

Create a starting SimulatorRelease with:

- SimulatorId = 2
- Version = "12.1.4"

Assign the release to:

- SupportedFromRelease

**Act:**

Call ValidateReleaseBoundaries().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

SupportedFromRelease must belong to the
simulator associated with the compatibility
record.


#### Test 3 — ValidateReleaseBoundaries_WhenThroughReleaseBelongsToDifferentSimulator_ThrowsException

**Purpose:**

Verifies that an ending release belonging
to another simulator causes validation
to fail.

**Arrange:**

Create a SimulatorOperatingSystemCompatibility
entity with:

- SimulatorId = 1

Create an ending SimulatorRelease with:

- SimulatorId = 2
- Version = "12.1.4"

Assign the release to:

- SupportedThroughRelease

**Act:**

Call ValidateReleaseBoundaries().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

SupportedThroughRelease must belong to the
simulator associated with the compatibility
record.


#### Test 4 — ValidateReleaseBoundaries_WhenNoReleaseBoundariesAreSet_DoesNotThrow

**Purpose:**

Verifies that compatibility records without
defined release boundaries pass validation.

**Arrange:**

Create a SimulatorOperatingSystemCompatibility
entity with:

- SimulatorId = 1

Leave both release-boundary navigation
properties unassigned:

- SupportedFromRelease
- SupportedThroughRelease

**Act:**

Call ValidateReleaseBoundaries().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

Release boundaries are optional for
simulator operating system compatibility
records.


**Test Helpers:**

The class contains two private static
helper methods.

#### CreateCompatibility()

**Purpose:**

Creates a baseline
SimulatorOperatingSystemCompatibility
entity for testing.

**Behavior:**

- Assigns SimulatorId = 1.
- Initializes Simulator using null!.
- Initializes OperatingSystem using null!.
- Initializes CompatibilityStatus using null!.

The null-forgiving operator is used
to initialize required navigation properties
without constructing their related entities.

The helper does not populate release boundaries.


#### CreateRelease()

**Purpose:**

Creates a SimulatorRelease associated
with a specified simulator identifier.

**Parameters:**

- simulatorId: int
- version: string

**Behavior:**

- Assigns SimulatorId.
- Assigns Version.
- Initializes Simulator using null!.

This helper supports testing release
relationships involving matching
and mismatched simulator identifiers.


**Coverage Summary:**

The existing tests verify:

- Successful validation when both release
  boundaries belong to the correct simulator.
- Rejection of a starting release belonging
  to another simulator.
- Rejection of an ending release belonging
  to another simulator.
- Successful validation when no release
  boundaries are provided.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Starting release later than ending release.
- Release version ordering.
- Starting release provided without
  an ending release when the starting
  release belongs to the correct simulator.
- Ending release provided without
  a starting release when the ending
  release belongs to the correct simulator.
- CompatibilityStatus validation.
- OperatingSystem relationship validation.
- Windows compatibility behavior.
- macOS compatibility behavior.
- Linux compatibility behavior.
- Duplicate compatibility records.
- Database persistence of compatibility
  relationships.
- Database foreign-key enforcement.

These are potential future test scenarios.

They do not indicate failures in the
existing implementation.


**Implementation Notes:**

- The class contains four NUnit tests.
- Each test method uses the Test attribute.
- No explicit TestFixture attribute is declared.
- Two private static helper methods are used.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- Tests invoke ValidateReleaseBoundaries()
  directly.
- Exception messages are not asserted.
- Test execution results have not been
  independently verified during this
  documentation review.

## 4.4 Geographic Regions

The Geographic Regions test suite verifies
domain rules associated with geographic markets,
regions, and currencies.

These tests help protect the integrity of
geographic and financial reference data
used throughout FSEI.

The suite includes:

- CurrencyTests
- MarketTests


### 4.4.1 CurrencyTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `GeographicRegions/CurrencyTest.cs`

**Framework:** NUnit

**Test Class:** CurrencyTests

**Domain Class:** Currency

**Number of Test Methods:** 3

**Number of Test Cases:** 5

**Purpose:**

Verifies currency-code validation within
the FSEI Geographic Regions domain.

Ensures that currency codes contain
exactly three characters and rejects
empty, whitespace-only, or incorrectly
sized values.

The tests focus on:

- Currency.Validate()


**Test Methods:**

#### Test 1 — Validate_WhenCodeHasThreeCharacters_DoesNotThrow

**Purpose:**

Verifies that a currency containing a
valid three-character code passes
domain validation.

**Arrange:**

Create a Currency with:

- CurrencyId = 1
- Code = "USD"
- Name = "Test Currency"

**Act:**

Call Currency.Validate().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

A three-character currency code is
accepted by domain validation.


#### Test 2 — Validate_WhenCodeIsEmptyOrWhitespace_ThrowsInvalidOperationException

**Purpose:**

Verifies that empty and whitespace-only
currency codes are rejected.

**Test Type:**

Parameterized NUnit test.

**Test Cases:**

| Input | Description | Expected Result |
|---|---|---|
| "" | Empty string | InvalidOperationException |
| "  " | Two spaces | InvalidOperationException |

**Arrange:**

Create a Currency using the supplied
test-case value.

**Act:**

Call Currency.Validate().

**Expected Result:**

An InvalidOperationException is thrown
for each supplied input.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

Currency codes cannot be empty or
consist entirely of whitespace.


#### Test 3 — Validate_WhenCodeDoesNotHaveThreeCharacters_ThrowsInvalidOperationException

**Purpose:**

Verifies that currency codes with
incorrect character counts are rejected.

**Test Type:**

Parameterized NUnit test.

**Test Cases:**

| Input | Character Count | Expected Result |
|---|---:|---|
| "US" | 2 | InvalidOperationException |
| "USDD" | 4 | InvalidOperationException |

**Arrange:**

Create a Currency using the supplied
test-case value.

**Act:**

Call Currency.Validate().

**Expected Result:**

An InvalidOperationException is thrown
for each supplied input.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

Currency codes must contain exactly
three characters.


**Test Helpers:**

The class contains one private static
helper method.

#### CreateCurrency()

**Purpose:**

Constructs a Currency entity using
a supplied currency code.

**Parameter:**

- code: string

**Behavior:**

Creates a Currency with:

- CurrencyId = 1
- Code = supplied parameter
- Name = "Test Currency"

This helper reduces repeated entity
initialization across the test methods.


**Coverage Summary:**

The existing tests verify:

- Acceptance of a three-character code.
- Rejection of an empty code.
- Rejection of a whitespace-only code.
- Rejection of a two-character code.
- Rejection of a four-character code.

The class contains three test methods
representing five NUnit test cases.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Null currency codes.
- Lowercase currency codes.
- Mixed-case currency codes.
- Numeric currency codes.
- Special characters in currency codes.
- Three-character whitespace-containing codes.
- Leading or trailing whitespace.
- Currency code uniqueness.
- Validation against recognized ISO
  currency codes.
- Empty or missing currency names.
- Currency symbol validation.
- Database persistence.
- Database uniqueness constraints.

These are potential future test scenarios.

They do not indicate failures in the
existing implementation.


**Implementation Notes:**

- The class contains three NUnit test methods.
- Five test cases are defined.
- One method uses the Test attribute.
- Two methods use TestCase attributes.
- No explicit TestFixture attribute is declared.
- One private static helper method is used.
- Tests construct Currency entities directly.
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- Exception messages are not asserted.
- Test execution results have not been
  independently verified during this
  documentation review.

### 4.4.2 MarketTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `GeographicRegions/MarketTest.cs`

**Framework:** NUnit

**Test Class:** MarketTests

**Domain Class:** Market

**Number of Test Methods:** 2

**Number of Test Cases:** 2

**Purpose:**

Verifies relationship consistency between
Market and Region entities within the
FSEI Geographic Regions domain.

Ensures that a Market references the
correct geographic Region.

The tests focus on:

- Market.Validate()


**Test Methods:**

#### Test 1 — Validate_WhenRegionMatches_DoesNotThrow

**Purpose:**

Verifies that a Market passes validation
when its RegionId matches the identifier
of its associated Region entity.

**Arrange:**

Create a Region with:

- RegionId = 1
- Code = "DE"
- Name = "Germany"

Create a Market with:

- MarketId = 1
- RegionId = 1
- Code = "DE"
- Name = "German Market"
- Region = previously created Region

**Act:**

Call Market.Validate().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

A Market with a RegionId matching its
associated Region passes domain validation.


#### Test 2 — Validate_WhenRegionDoesNotMatch_ThrowsInvalidOperationException

**Purpose:**

Verifies that a Market fails validation
when its RegionId does not match the
identifier of its associated Region.

**Arrange:**

Create a Region with:

- RegionId = 1
- Code = "DE"
- Name = "Germany"

Create a Market with:

- MarketId = 1
- RegionId = 2
- Code = "DE"
- Name = "German Market"
- Region = previously created Region

**Act:**

Call Market.Validate().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

Market.RegionId must match the identifier
of the associated Region entity.


**Test Helpers:**

The class contains one private static
helper method.

#### CreateRegion()

**Purpose:**

Constructs a Region entity using a
supplied region identifier.

**Parameter:**

- regionId: int

**Behavior:**

Creates a Region with:

- RegionId = supplied parameter
- Code = "DE"
- Name = "Germany"

This helper reduces repeated Region
initialization across the test methods.


**Coverage Summary:**

The existing tests verify:

- Successful validation when RegionId
  matches Region.RegionId.
- Rejection of mismatched region identifiers.
- The expected InvalidOperationException
  for an inconsistent relationship.

Both tests exercise Market.Validate()
directly.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Null Region navigation properties.
- Zero or negative RegionId values.
- Zero or negative MarketId values.
- Empty market codes.
- Empty market names.
- Duplicate market codes.
- Multiple markets associated with one region.
- Market and Region code consistency.
- Database persistence of Market entities.
- Database foreign-key enforcement.

These are potential future test scenarios.

They do not indicate failures in the
existing implementation.


**Implementation Notes:**

- The class contains two NUnit tests.
- Both methods use the Test attribute.
- No explicit TestFixture attribute is declared.
- One private static helper method is used.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- Both tests execute synchronously.
- Exception messages are not asserted.
- Test execution results have not been
  independently verified during this
  documentation review.

## 4.5 Evaluations

The Evaluations test suite verifies domain
rules associated with FSEI's aircraft
evaluation and scoring methodology.

These tests help protect the consistency
of component scores, evaluation assignments,
and methodology versions.

The suite includes:

- EvaluationComponentAssessmentTests
- SystemsDepthEvaluationTests


### 4.5.1 EvaluationComponentAssessmentTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `Evaluations/EvaluationComponentAssessmentTests.cs`

**Framework:** NUnit

**Test Class:** EvaluationComponentAssessmentTests

**Domain Class:** EvaluationComponentAssessment

**Number of Test Methods:** 9

**Purpose:**

Verifies the behavior of individual evaluation
component assessments within the FSEI domain.

Ensures that component scores remain within
their permitted boundaries.

Also verifies that assessments are assigned
to exactly one evaluation and reference
the correct evaluation methodology version.

The tests focus on:

- EvaluationComponentAssessment.SetScore()
- EvaluationComponentAssessment.ValidateEvaluationAssignment()
- EvaluationComponentAssessment.ValidateMethodologyVersion()


**Test Group 1 — Component Score Validation**

#### Test 1 — SetScore_WhenScoreIsWithinMaximum_SetsScore

**Purpose:**

Verifies that a component assessment accepts
a score within its permitted range.

**Arrange:**

Create an EvaluationComponent with:

- Name = "Electrical Systems"
- MaximumScore = 20

Create an EvaluationComponentAssessment
associated with the component.

**Act:**

Call SetScore(19).

**Expected Result:**

The assessment Score property equals 19.

**Assertion:**

Assert.That(assessment.Score, Is.EqualTo(19))

**Business Rule Verified:**

A score within the component's maximum
permitted value can be assigned successfully.


#### Test 2 — SetScore_WhenScoreExceedsMaximum_ThrowsException

**Purpose:**

Verifies that a component assessment rejects
a score exceeding its maximum permitted value.

**Arrange:**

Create an EvaluationComponent with:

- MaximumScore = 20

Create an associated assessment.

**Act:**

Call SetScore(21).

**Expected Result:**

An ArgumentOutOfRangeException is thrown.

**Assertion:**

Assert.Throws<ArgumentOutOfRangeException>()

**Business Rule Verified:**

Component scores cannot exceed the
configured maximum score.


#### Test 3 — SetScore_WhenScoreIsNull_SetsScoreToNull

**Purpose:**

Verifies that a previously assigned
component score can be cleared.

**Arrange:**

Create an EvaluationComponent with:

- MaximumScore = 20

Create an associated assessment.

**Act:**

1. Call SetScore(19).
2. Call SetScore(null).

**Expected Result:**

The assessment Score property is null.

**Assertion:**

Assert.That(assessment.Score, Is.Null)

**Business Behavior Verified:**

A component assessment can represent
an unevaluated state using a null score.


#### Test 4 — SetScore_WhenScoreIsBelowZero_ThrowsException

**Purpose:**

Verifies that negative component scores
are rejected.

**Arrange:**

Create an EvaluationComponent with:

- MaximumScore = 20

Create an associated assessment.

**Act:**

Call SetScore(-1).

**Expected Result:**

An ArgumentOutOfRangeException is thrown.

**Assertion:**

Assert.Throws<ArgumentOutOfRangeException>()

**Business Rule Verified:**

Component scores cannot be negative.


**Test Group 2 — Evaluation Assignment Validation**

#### Test 5 — ValidateEvaluationAssignment_WhenNoEvaluationAssigned_ThrowsException

**Purpose:**

Verifies that an assessment without an
associated evaluation fails validation.

**Arrange:**

Create an EvaluationComponentAssessment
without assigning either:

- SystemsDepthEvaluationId
- AvionicsEvaluationId

**Act:**

Call ValidateEvaluationAssignment().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

An assessment must be assigned to an evaluation.


#### Test 6 — ValidateEvaluationAssignment_WhenBothEvaluationsAssigned_ThrowsException

**Purpose:**

Verifies that an assessment cannot be
assigned to both evaluation types
simultaneously.

**Arrange:**

Create an EvaluationComponentAssessment
with:

- SystemsDepthEvaluationId = 1
- AvionicsEvaluationId = 1

**Act:**

Call ValidateEvaluationAssignment().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

An assessment cannot simultaneously
belong to both Systems Depth and
Avionics evaluations.


#### Test 7 — ValidateEvaluationAssignment_WhenOneEvaluationAssigned_DoesNotThrow

**Purpose:**

Verifies that an assessment assigned to
one evaluation type passes validation.

**Arrange:**

Create an EvaluationComponentAssessment
with:

- SystemsDepthEvaluationId = 1
- AvionicsEvaluationId unassigned

**Act:**

Call ValidateEvaluationAssignment().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

An assessment assigned to exactly one
evaluation type is accepted.

The test specifically verifies a
Systems Depth evaluation assignment.


**Test Group 3 — Methodology Version Validation**

#### Test 8 — ValidateMethodologyVersion_WhenComponentBelongsToDifferentVersion_ThrowsException

**Purpose:**

Verifies that an assessment rejects
a methodology version different from
the version associated with its component.

**Arrange:**

Create an EvaluationComponent associated
with a methodology version.

Configure:

- Component methodology version ID = 1
- Comparison methodology version ID = 2

The comparison version uses:

- EvaluationMethodologyId = 1
- VersionNumber = 2
- Name = "Systems Depth"

**Act:**

Call ValidateMethodologyVersion() using
the different methodology version.

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

An evaluation component assessment must
use a compatible methodology version.


#### Test 9 — ValidateMethodologyVersion_WhenComponentBelongsToSameVersion_DoesNotThrow

**Purpose:**

Verifies that an assessment accepts
the methodology version associated
with its component.

**Arrange:**

Create an EvaluationComponent.

Configure:

- Component methodology version ID = 1
- Associated methodology version ID = 1

Create an EvaluationComponentAssessment
associated with the component.

**Act:**

Call ValidateMethodologyVersion() using
the component's methodology version.

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

An assessment using its component's
methodology version passes validation.


**Test Helpers:**

The class contains one private static
helper method.

#### CreateComponent()

**Purpose:**

Constructs an EvaluationComponent with
an associated evaluation methodology
and methodology version.

**Parameter:**

- maximumScore: int
- Default value: 20

**Entities Created:**

- EvaluationMethodology
- EvaluationMethodologyVersion
- EvaluationComponent

**Default Test Data:**

EvaluationMethodology:
- Name = "Systems Depth"

EvaluationMethodologyVersion:
- EvaluationMethodologyId = 1
- VersionNumber = 1
- EffectiveDate = DateTime.Today

EvaluationComponent:
- Name = "Electrical Systems"
- MaximumScore = 20 by default

This helper provides consistent test
data for score and validation scenarios.


**Coverage Summary:**

The existing tests verify:

- Assignment of a valid component score.
- Rejection of scores exceeding the maximum.
- Clearing an existing score using null.
- Rejection of negative scores.
- Rejection of assessments without an evaluation.
- Rejection of assessments assigned to
  both evaluation types.
- Acceptance of a Systems Depth assignment.
- Rejection of mismatched methodology versions.
- Acceptance of matching methodology versions.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- A score exactly equal to MaximumScore.
- A score of zero.
- Assignment exclusively to an Avionics evaluation.
- Null EvaluationComponent references.
- Zero or negative MaximumScore values.
- Methodology version references with
  missing navigation properties.
- Assessment persistence.
- Database foreign-key enforcement.
- Interaction with aggregate evaluation
  scoring calculations.

These are potential future test scenarios.

They do not indicate failures in the
existing implementation.


**Implementation Notes:**

- The class contains nine NUnit tests.
- Each test method uses the Test attribute.
- No explicit TestFixture attribute is declared.
- One private static helper method is used.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- Exception messages are not asserted.
- Test execution results have not been
  independently verified during this
  documentation review.

### 4.5.2 SystemsDepthEvaluationTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `Evaluations/SystemsDepthEvaluationTests.cs`

**Framework:** NUnit

**Test Class:** SystemsDepthEvaluationTests

**Domain Class:** SystemsDepthEvaluation

**Number of Test Methods:** 12

**Tests With Assertions:** 8

**Incomplete Tests:** 4

**Purpose:**

Verifies domain behavior associated with
Systems Depth evaluations.

The test suite focuses on two major areas:

1. Evaluation target validation.
2. Normalized Systems Depth scoring.

Evaluation target validation is intended
to ensure that an evaluation references
the correct type of aircraft entity.

Normalized scoring verifies how individual
component assessments contribute to the
overall Systems Depth Score.

The tests focus on:

- SystemsDepthEvaluation.ValidateEvaluationTarget()
- SystemsDepthEvaluation.SystemsDepthScore


**Test Group 1 — Evaluation Target Validation**

#### Test 1 — ValidateEvaluationTarget_WhenNoTargetAssigned_ThrowsException

**Purpose:**

Verifies that an evaluation without an
assigned target fails validation.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "VARIANT"
- EvaluationScope.Name = "Variant"

Do not assign an evaluation target.

**Act:**

Call ValidateEvaluationTarget().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

A Systems Depth evaluation cannot exist
without a valid evaluation target.


#### Test 2 — ValidateEvaluationTarget_WhenMultipleTargetsAssigned_ThrowsException

**Purpose:**

Verifies that an evaluation assigned to
multiple targets fails validation.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "VARIANT"
- EvaluationScope.Name = "Variant"
- AircraftId = 1
- VariantId = 1

**Act:**

Call ValidateEvaluationTarget().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>()

**Business Rule Verified:**

A Systems Depth evaluation cannot
simultaneously reference multiple
evaluation targets.


#### Test 3 — ValidateEvaluationTarget_WhenVariantScopeMatchesVariantTarget_DoesNotThrow

**Purpose:**

Verifies that an evaluation using
the VARIANT scope accepts a matching
variant target.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "VARIANT"
- EvaluationScope.Name = "Variant"
- VariantId = 1

**Act:**

Call ValidateEvaluationTarget().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow()

**Business Rule Verified:**

A VARIANT evaluation scope accepts
a VariantId target.


#### Test 4 — ValidateEvaluationTarget_WhenTargetDoesNotMatchScope_ThrowsException

**Purpose:**

Intended to verify that an evaluation
rejects a target that does not match
its configured evaluation scope.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "VARIANT"
- EvaluationScope.Name = "Variant"
- AircraftId = 1

**Current Implementation:**

The test creates the evaluation and
assigns AircraftId.

However, it does not call
ValidateEvaluationTarget().

It also contains no assertion.

**Expected Business Rule:**

A VARIANT evaluation scope should
reject an AircraftId target.

**Verification Status:**

INCOMPLETE — NOT VERIFIED

**Recommended Future Test Assertion:**

Assert.Throws<InvalidOperationException>(
() => evaluation.ValidateEvaluationTarget());


#### Test 5 — ValidateEvaluationTarget_WhenConfigurationScopeMatchesConfigurationTarget_DoesNotThrow

**Purpose:**

Intended to verify that a CONFIGURATION
scope accepts an aircraft configuration
as its evaluation target.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "CONFIGURATION"
- EvaluationScope.Name = "Configuration"
- AircraftConfigurationId = 1

**Current Implementation:**

The test creates the evaluation and
assigns AircraftConfigurationId.

However, it does not call
ValidateEvaluationTarget().

It also contains no assertion.

**Expected Business Rule:**

A CONFIGURATION scope should accept
an AircraftConfigurationId target.

**Verification Status:**

INCOMPLETE — NOT VERIFIED

**Recommended Future Test Assertion:**

Assert.DoesNotThrow(
() => evaluation.ValidateEvaluationTarget());


#### Test 6 — ValidateEvaluationTarget_WhenAircraftFamilyScopeMatchesAircraftTarget_DoesNotThrow

**Purpose:**

Intended to verify that an
AIRCRAFT_FAMILY scope accepts
an aircraft target.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "AIRCRAFT_FAMILY"
- EvaluationScope.Name = "Aircraft Family"
- AircraftId = 1

**Current Implementation:**

The test creates the evaluation and
assigns AircraftId.

However, it does not call
ValidateEvaluationTarget().

It also contains no assertion.

**Expected Business Rule:**

An AIRCRAFT_FAMILY scope should
accept an AircraftId target.

**Verification Status:**

INCOMPLETE — NOT VERIFIED

**Recommended Future Test Assertion:**

Assert.DoesNotThrow(
() => evaluation.ValidateEvaluationTarget());


#### Test 7 — ValidateEvaluationTarget_WhenScopeCodeIsUnknown_ThrowsException

**Purpose:**

Intended to verify that an evaluation
using an unknown scope code fails
validation.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "UNKNOWN"
- EvaluationScope.Name = "Unknown"
- VariantId = 1

**Current Implementation:**

The test creates the evaluation and
assigns VariantId.

However, it does not call
ValidateEvaluationTarget().

It also contains no assertion.

**Expected Business Rule:**

Unknown evaluation scope codes
should be rejected.

**Verification Status:**

INCOMPLETE — NOT VERIFIED

**Recommended Future Test Assertion:**

Assert.Throws<InvalidOperationException>(
() => evaluation.ValidateEvaluationTarget());


**Test Group 2 — Normalized Systems Depth Scoring**

#### Test 8 — SystemsDepthScore_WhenAllComponentsAreEvaluated_CalculatesNormalizedScore

**Purpose:**

Verifies normalized scoring when
all assessment components contain
assigned scores.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "VARIANT"
- VariantId = 1

Create five component assessments.

Each component has:

- MaximumScore = 20

Assigned scores:

| Component | Score | Maximum |
|---|---:|---:|
| 1 | 19 | 20 |
| 2 | 19 | 20 |
| 3 | 20 | 20 |
| 4 | 19 | 20 |
| 5 | 20 | 20 |

Total awarded points:

19 + 19 + 20 + 19 + 20 = 97

Total available points:

20 × 5 = 100

**Act:**

Access evaluation.SystemsDepthScore.

**Expected Result:**

SystemsDepthScore = 97

**Assertion:**

Assert.That(
evaluation.SystemsDepthScore,
Is.EqualTo(97))

**Business Rule Verified:**

The normalized Systems Depth Score
reflects the percentage of available
points awarded across evaluated components.


#### Test 9 — SystemsDepthScore_WhenComponentIsNotEvaluated_ExcludesItFromDenominator

**Purpose:**

Verifies that an unevaluated component
does not reduce the overall score.

**Arrange:**

Create five component assessments.

Each component has:

- MaximumScore = 20

Assigned scores:

| Component | Score | Maximum |
|---|---:|---:|
| 1 | 20 | 20 |
| 2 | 20 | 20 |
| 3 | 20 | 20 |
| 4 | 20 | 20 |
| 5 | null | 20 |

Total awarded points:

20 + 20 + 20 + 20 = 80

Total available points for evaluated
components:

20 × 4 = 80

**Act:**

Access evaluation.SystemsDepthScore.

**Expected Result:**

SystemsDepthScore = 100

**Assertion:**

Assert.That(
evaluation.SystemsDepthScore,
Is.EqualTo(100))

**Business Rule Verified:**

Components without assigned scores
are excluded from the normalized
scoring denominator.


#### Test 10 — SystemsDepthScore_WhenComponentScoreIsZero_IncludesItInDenominator

**Purpose:**

Verifies that a component receiving
zero points is still considered evaluated.

**Arrange:**

Create five component assessments.

Each component has:

- MaximumScore = 20

Assigned scores:

| Component | Score | Maximum |
|---|---:|---:|
| 1 | 20 | 20 |
| 2 | 20 | 20 |
| 3 | 20 | 20 |
| 4 | 20 | 20 |
| 5 | 0 | 20 |

Total awarded points:

20 + 20 + 20 + 20 + 0 = 80

Total available points:

20 × 5 = 100

**Act:**

Access evaluation.SystemsDepthScore.

**Expected Result:**

SystemsDepthScore = 80

**Assertion:**

Assert.That(
evaluation.SystemsDepthScore,
Is.EqualTo(80))

**Business Rule Verified:**

A zero score represents an evaluated
component and must remain included
in the normalized scoring denominator.


#### Test 11 — SystemsDepthScore_WhenNoComponentsAreEvaluated_ReturnsNull

**Purpose:**

Verifies that the Systems Depth Score
remains undefined when no component
assessments have been evaluated.

**Arrange:**

Create a SystemsDepthEvaluation with:

- EvaluationScope.Code = "VARIANT"
- VariantId = 1

Do not add any assessments.

**Act:**

Access evaluation.SystemsDepthScore.

**Expected Result:**

SystemsDepthScore = null

**Assertion:**

Assert.That(
evaluation.SystemsDepthScore,
Is.Null)

**Business Rule Verified:**

An evaluation without scored components
does not receive a calculated
Systems Depth Score.


#### Test 12 — SystemsDepthScore_WhenResultIsMidpoint_RoundsAwayFromZero

**Purpose:**

Verifies rounding behavior when
the normalized score falls exactly
between two whole numbers.

**Arrange:**

Create one component assessment with:

- MaximumScore = 8
- Score = 5

Normalized score:

(5 / 8) × 100 = 62.5

**Act:**

Access evaluation.SystemsDepthScore.

**Expected Result:**

SystemsDepthScore = 63

**Assertion:**

Assert.That(
evaluation.SystemsDepthScore,
Is.EqualTo(63))

**Business Rule Verified:**

Positive midpoint values are rounded
away from zero rather than using
banker's rounding.


**Test Helpers:**

The class contains two private static
helper methods.

#### CreateAssessment()

**Purpose:**

Creates an EvaluationComponentAssessment
associated with a Systems Depth evaluation.

**Parameters:**

- evaluation: SystemsDepthEvaluation
- maximumScore: int
- score: int?

**Behavior:**

Creates an EvaluationComponent with:

- Name = "Test Component"
- MaximumScore = supplied maximumScore
- EvaluationMethodologyVersion =
  evaluation.EvaluationMethodologyVersion

Creates an EvaluationComponentAssessment with:

- EvaluationComponent = created component
- SystemsDepthEvaluationId = 1

Calls SetScore(score).

Returns the assessment.


#### CreateEvaluation()

**Purpose:**

Creates a SystemsDepthEvaluation
with an evaluation scope and
methodology version.

**Parameters:**

- scopeCode: string
- scopeName: string

**Behavior:**

Creates an EvaluationScope with:

- Code = supplied scopeCode
- Name = supplied scopeName

Creates an EvaluationMethodologyVersion with:

- VersionNumber = 1
- EffectiveDate = DateTime.Today

Creates an EvaluationMethodology with:

- Name = "Systems Depth"

Returns the SystemsDepthEvaluation.


**Coverage Summary:**

The existing tests with assertions verify:

Evaluation Target Validation:

- Rejection of evaluations without targets.
- Rejection of evaluations with multiple targets.
- Acceptance of a matching VARIANT target.

Normalized Scoring:

- Calculation when all components are evaluated.
- Exclusion of null scores from the denominator.
- Inclusion of zero scores in the denominator.
- Null result when no assessments exist.
- Midpoint rounding away from zero.


**Incomplete Test Coverage:**

Four methods contain test setup but
do not execute validation or assertions.

These methods are:

1. ValidateEvaluationTarget_WhenTargetDoesNotMatchScope_ThrowsException

2. ValidateEvaluationTarget_WhenConfigurationScopeMatchesConfigurationTarget_DoesNotThrow

3. ValidateEvaluationTarget_WhenAircraftFamilyScopeMatchesAircraftTarget_DoesNotThrow

4. ValidateEvaluationTarget_WhenScopeCodeIsUnknown_ThrowsException

These methods require additional assertions
before they can verify their intended
business rules.

Their presence in the test suite should
not be interpreted as completed coverage.


**Additional Coverage Gaps:**

The following scenarios are not directly
verified by the existing assertions:

- CONFIGURATION target validation.
- AIRCRAFT_FAMILY target validation.
- Rejection of mismatched evaluation scopes.
- Rejection of unknown scope codes.
- Evaluation scope navigation properties
  being null.
- Components with different maximum scores.
- Assessments containing invalid score values.
- All assessments having null scores.
- Score rounding at other fractional values.
- Assessment methodology version mismatches
  during aggregate scoring.
- Database persistence of evaluations.
- Database relationship enforcement.

These are potential future test scenarios.

They do not automatically indicate defects
in the production implementation.


**Implementation Notes:**

- The class contains twelve NUnit methods.
- All methods use the Test attribute.
- Eight methods contain assertions.
- Four methods contain no assertions.
- No explicit TestFixture attribute is declared.
- Two private static helper methods are used.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- The scoring tests access a calculated
  SystemsDepthScore property.
- Exception messages are not asserted.
- Test execution results have not been
  independently verified during this
  documentation review.


**Documentation Review Findings:**

Finding EVAL-001:

Four evaluation target validation tests
are incomplete.

Severity: Test Coverage Gap

Affected Methods:

- ValidateEvaluationTarget_WhenTargetDoesNotMatchScope_ThrowsException
- ValidateEvaluationTarget_WhenConfigurationScopeMatchesConfigurationTarget_DoesNotThrow
- ValidateEvaluationTarget_WhenAircraftFamilyScopeMatchesAircraftTarget_DoesNotThrow
- ValidateEvaluationTarget_WhenScopeCodeIsUnknown_ThrowsException

Recommended Action:

Complete the four tests by invoking
ValidateEvaluationTarget() and asserting
the expected result.

No production code changes are implied
by this documentation finding.

Status: Open

## 4.6 Avionics

The Avionics test suite verifies domain rules
associated with avionics fidelity evaluations.

These tests ensure that avionics component
assessments contribute correctly to the
overall normalized Fidelity Score.

The tests distinguish between:

- Fully evaluated components.
- Components that have not been evaluated.
- Components receiving a score of zero.
- Evaluations containing no assessments.


### 4.6.1 AvionicsEvaluationTests

**Namespace:** `FSEI.Domain.Tests`

**Source:** `AvionicsEvaluationTests.cs`

**Framework:** NUnit

**Test Class:** AvionicsEvaluationTests

**Domain Class:** AvionicsEvaluation

**Number of Test Methods:** 4

**Number of Test Cases:** 4

**Purpose:**

Verifies the calculation of normalized
avionics fidelity scores.

Ensures that the FidelityScore property
correctly handles evaluated and
unevaluated components.

The tests focus on:

- AvionicsEvaluation.FidelityScore


**Test Group 1 — Normalized Fidelity Scoring**

#### Test 1 — FidelityScore_WhenAllComponentsAreEvaluated_CalculatesNormalizedScore

**Purpose:**

Verifies that FidelityScore calculates
a normalized score when all avionics
components have been evaluated.

**Arrange:**

Create an AvionicsEvaluation using
the CreateEvaluation() helper.

Create seven component assessments:

| Component | Maximum Score | Awarded Score |
|---|---:|---:|
| 1 | 25 | 25 |
| 2 | 20 | 19 |
| 3 | 15 | 14 |
| 4 | 20 | 19 |
| 5 | 8 | 8 |
| 6 | 7 | 6 |
| 7 | 5 | 5 |
| **Total** | **100** | **96** |

**Act:**

Access evaluation.FidelityScore.

**Calculation:**

Total awarded points = 96

Total available points = 100

Normalized score:

(96 / 100) × 100 = 96

**Expected Result:**

FidelityScore = 96

**Assertion:**

Assert.That(
evaluation.FidelityScore,
Is.EqualTo(96))

**Business Rule Verified:**

The normalized Fidelity Score reflects
the percentage of available points
awarded across evaluated components.


#### Test 2 — FidelityScore_WhenComponentIsNotEvaluated_ExcludesItFromDenominator

**Purpose:**

Verifies that an unevaluated component
is excluded from the normalized scoring
denominator.

**Arrange:**

Create an AvionicsEvaluation.

Create three component assessments:

| Component | Maximum Score | Awarded Score |
|---|---:|---:|
| 1 | 25 | 25 |
| 2 | 20 | 20 |
| 3 | 15 | null |

**Act:**

Access evaluation.FidelityScore.

**Calculation:**

Total awarded points = 45

Total available points for evaluated
components = 45

The component with a null score
is excluded from the denominator.

Normalized score:

(45 / 45) × 100 = 100

**Expected Result:**

FidelityScore = 100

**Assertion:**

Assert.That(
evaluation.FidelityScore,
Is.EqualTo(100))

**Business Rule Verified:**

Unevaluated avionics components do not
reduce the normalized Fidelity Score.

A null score represents a component
that has not been evaluated.


#### Test 3 — FidelityScore_WhenComponentScoreIsZero_IncludesItInDenominator

**Purpose:**

Verifies that a component receiving
zero points remains included in
the normalized scoring denominator.

**Arrange:**

Create an AvionicsEvaluation.

Create three component assessments:

| Component | Maximum Score | Awarded Score |
|---|---:|---:|
| 1 | 25 | 25 |
| 2 | 20 | 20 |
| 3 | 15 | 0 |

**Act:**

Access evaluation.FidelityScore.

**Calculation:**

Total awarded points = 45

Total available points = 60

Normalized score:

(45 / 60) × 100 = 75

**Expected Result:**

FidelityScore = 75

**Assertion:**

Assert.That(
evaluation.FidelityScore,
Is.EqualTo(75))

**Business Rule Verified:**

A zero score represents an evaluated
component and must remain included
in the normalized scoring denominator.


#### Test 4 — FidelityScore_WhenNoComponentsAreEvaluated_ReturnsNull

**Purpose:**

Verifies that FidelityScore remains
undefined when an evaluation contains
no component assessments.

**Arrange:**

Create an AvionicsEvaluation.

Do not add any assessments.

**Act:**

Access evaluation.FidelityScore.

**Expected Result:**

FidelityScore = null

**Assertion:**

Assert.That(
evaluation.FidelityScore,
Is.Null)

**Business Rule Verified:**

An avionics evaluation without
evaluated components does not
receive a calculated Fidelity Score.


**Test Helpers:**

The class contains two private static
helper methods.

#### CreateEvaluation()

**Purpose:**

Creates a baseline AvionicsEvaluation
with an associated evaluation methodology
and methodology version.

**Behavior:**

Creates an EvaluationMethodology with:

- Name = "Avionics Fidelity"

Creates an EvaluationMethodologyVersion with:

- EvaluationMethodologyId = 1
- VersionNumber = 1
- EffectiveDate = DateTime.Today

Creates an AvionicsEvaluation with:

- EvaluationMethodologyVersion =
  created methodology version
- ConfigurationAvionics = null!

The null-forgiving operator is used
without constructing a related
ConfigurationAvionics entity.


#### CreateAssessment()

**Purpose:**

Creates an EvaluationComponentAssessment
associated with an AvionicsEvaluation.

**Parameters:**

- evaluation: AvionicsEvaluation
- maximumScore: int
- score: int?

**Behavior:**

Creates an EvaluationComponent with:

- Name = "Test Component"
- MaximumScore = supplied maximumScore
- EvaluationMethodologyVersion =
  evaluation.EvaluationMethodologyVersion

Creates an EvaluationComponentAssessment with:

- EvaluationComponent = created component
- AvionicsEvaluationId = 1

Calls SetScore(score).

Returns the assessment.


**Coverage Summary:**

The existing tests verify:

- Normalized scoring with all components
  evaluated.
- Exclusion of null scores from the
  normalized scoring denominator.
- Inclusion of zero scores in the
  normalized scoring denominator.
- Null FidelityScore when no
  assessments exist.

All four test methods contain assertions.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- All assessments having null scores.
- Midpoint rounding behavior.
- Components with invalid maximum scores.
- Assessment methodology version mismatches.
- Evaluation assignment validation.
- Null methodology version references.
- Duplicate component assessments.
- Database persistence.
- Database relationship enforcement.

These are potential future test scenarios.

They do not automatically indicate defects
in the production implementation.


**Implementation Notes:**

- The class contains four NUnit tests.
- Each method uses the Test attribute.
- All four methods contain assertions.
- No explicit TestFixture attribute is declared.
- Two private static helper methods are used.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- FidelityScore is accessed as a
  calculated property.
- Test execution results have not been
  independently verified during this
  documentation review.

## 4.7 User Collections

The User Collections test suite verifies domain
rules associated with products maintained in
individual user collections.

These tests ensure that users can maintain
personal product records while preserving
the integrity of relationships with products
in the central FSEI catalog.

The tests distinguish between:

- User products linked to catalog products.
- User products without catalog associations.
- User products containing inconsistent
  catalog identifiers.
- User products with catalog identifiers
  but unloaded navigation properties.
- User products containing catalog references
  without corresponding identifiers.


### 4.7.1 UserProductTests

**Namespace:** `FSEI.Domain.Tests.UserCollections`

**Source:** `UserCollections/UserProductTests.cs`

**Framework:** NUnit

**Test Class:** UserProductTests

**Domain Class:** UserProduct

**Number of Test Methods:** 5

**Number of Test Cases:** 5

**Purpose:**

Verifies validation rules governing
relationships between UserProduct
entities and catalog Product entities.

Ensures that user collections support
both catalog-linked and independently
maintained products.

Also verifies that inconsistent
catalog product relationships are rejected.

The tests focus on:

- UserProduct.Validate()


**Test Group 1 — Valid User Product Relationships**

#### Test 1 — UserProduct_WithMatchingCatalogProduct_IsValid

**Purpose:**

Verifies that a user product linked to
a matching catalog product passes
domain validation.

**Arrange:**

Create a ProductType with:

- ProductTypeId = 1
- Code = "AIRCRAFT"
- Name = "Aircraft"

Create a Product with:

- ProductId = 10
- ProductTypeId = 1
- Name = "FlightFactor 777"
- ProductType = created ProductType

Create a UserProduct with:

- UserProductId = 1
- ProductId = 10
- Name = "My FlightFactor 777"
- Product = created Product

**Act:**

Call UserProduct.Validate().

**Expected Results:**

- Validation completes without an exception.
- UserProduct.ProductId equals Product.ProductId.
- UserProduct.Product references the same
  Product instance.

**Assertions:**

Assert.Multiple(() =>
{
Assert.DoesNotThrow(() => userProduct.Validate());

    Assert.That(
        userProduct.ProductId,
        Is.EqualTo(product.ProductId));

    Assert.That(
        userProduct.Product,
        Is.SameAs(product));
});

**Business Rule Verified:**

A user product can reference a catalog
product when its ProductId matches
the associated Product entity.


#### Test 2 — UserProduct_WithoutCatalogProduct_IsValid

**Purpose:**

Verifies that a user product can exist
without an associated catalog product.

**Arrange:**

Create a UserProduct with:

- UserProductId = 2
- Name = "Legacy Aircraft Add-on"

Do not assign:

- ProductId
- Product

**Act:**

Call UserProduct.Validate().

**Expected Results:**

- Validation completes without an exception.
- Name remains "Legacy Aircraft Add-on".
- ProductId is null.
- Product is null.

**Assertions:**

Assert.Multiple(() =>
{
Assert.DoesNotThrow(() => userProduct.Validate());

    Assert.That(
        userProduct.Name,
        Is.EqualTo("Legacy Aircraft Add-on"));

    Assert.That(
        userProduct.ProductId,
        Is.Null);

    Assert.That(
        userProduct.Product,
        Is.Null);
});

**Business Rule Verified:**

A user product does not require
an associated catalog product.

This allows users to maintain products
that are not represented in the
central FSEI catalog.


**Test Group 2 — Invalid Catalog Relationships**

#### Test 3 — Validate_WhenCatalogProductDoesNotMatch_ThrowsInvalidOperationException

**Purpose:**

Verifies that a user product fails
validation when its ProductId differs
from the identifier of its associated
catalog Product.

**Arrange:**

Create a Product with:

- ProductId = 10
- Name = "FlightFactor 777"

Create a UserProduct with:

- UserProductId = 1
- ProductId = 99
- Name = "My FlightFactor 777"
- Product = created Product

**Act:**

Call UserProduct.Validate().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>(
() => userProduct.Validate());

**Business Rule Verified:**

When a catalog Product is associated
with a UserProduct, their identifiers
must match.


#### Test 4 — Validate_WhenProductIdExistsButProductIsNull_DoesNotThrow

**Purpose:**

Verifies that a user product containing
a catalog ProductId can pass validation
without its Product navigation property
being populated.

**Arrange:**

Create a UserProduct with:

- UserProductId = 3
- ProductId = 10
- Name = "My Boeing 777"

Leave Product unassigned.

**Act:**

Call UserProduct.Validate().

**Expected Result:**

Validation completes without throwing
an exception.

**Assertion:**

Assert.DoesNotThrow(
() => userProduct.Validate());

**Business Rule Verified:**

A populated ProductId does not require
the Product navigation property to
be loaded during domain validation.

This supports entity relationships
where navigation properties have
not been populated.


#### Test 5 — Validate_WhenProductExistsWithoutProductId_ThrowsInvalidOperationException

**Purpose:**

Verifies that a user product fails
validation when a catalog Product
navigation property is populated
but ProductId is missing.

**Arrange:**

Create a UserProduct with:

- UserProductId = 4
- Name = "My Boeing 777"
- ProductId unassigned

Assign a Product with:

- ProductId = 10
- ProductTypeId = 1
- Name = "FlightFactor 777"

Assign a ProductType with:

- ProductTypeId = 1
- Code = "AIRCRAFT"
- Name = "Aircraft"

**Act:**

Call UserProduct.Validate().

**Expected Result:**

An InvalidOperationException is thrown.

**Assertion:**

Assert.Throws<InvalidOperationException>(
() => userProduct.Validate());

**Business Rule Verified:**

A UserProduct referencing a catalog
Product entity must also contain
the corresponding ProductId.


**Test Helpers:**

This class does not contain private
test helper methods.

Test entities are constructed directly
inside the individual test methods.

The tests use the following domain entities:

- UserProduct
- Product
- ProductType


**Coverage Summary:**

The existing tests verify:

- Acceptance of matching catalog
  product relationships.
- Acceptance of user products without
  catalog associations.
- Rejection of mismatched catalog
  product identifiers.
- Acceptance of ProductId values when
  Product navigation properties are null.
- Rejection of populated Product
  navigation properties without ProductId.

All five test methods contain assertions.


**Coverage Gaps:**

The following scenarios are not directly
tested by this class:

- Empty user product names.
- Whitespace-only user product names.
- Duplicate user products in a collection.
- Multiple user products referencing
  the same catalog product.
- Zero or negative ProductId values.
- Zero or negative UserProductId values.
- Catalog product deletion behavior.
- User ownership validation.
- Collection membership validation.
- Database persistence.
- Database foreign-key enforcement.

These are potential future test scenarios.

They do not automatically indicate defects
in the production implementation.


**Implementation Notes:**

- The class contains five NUnit tests.
- All methods use the Test attribute.
- The class explicitly uses TestFixture.
- Two tests use Assert.Multiple().
- All five tests contain assertions.
- No private helper methods are defined.
- Tests construct domain entities directly.
- No database operations are performed.
- No mocking framework is used.
- All tests execute synchronously.
- Exception messages are not asserted.
- Test execution results have not been
  independently verified during this
  documentation review.

## 5. Test Suite Summary and Maintenance

### 5.1 Test Suite Overview

The FSEI V2 domain test documentation covers
the following areas:

| Domain | Test Methods |
|---|---:|
| Products | 7 |
| Stores | 33 |
| Simulators | 4 |
| Geographic Regions | 5 |
| Evaluations | 21 |
| Avionics | 4 |
| User Collections | 5 |
| **Total** | **79** |

Total documented test classes: 13

Total documented test methods: 79

Total NUnit test cases: 81

### 5.2 Known Testing Gaps

Four methods in SystemsDepthEvaluationTests
currently contain setup logic without
validation calls or assertions.

These methods require future review.

Additional potential coverage gaps are
documented within their respective
test class sections.

### 5.3 Documentation Maintenance

When adding or modifying domain tests:

1. Update the corresponding test class section.
2. Document the business rule being tested.
3. Record the expected behavior and assertions.
4. Update test method and test case totals.
5. Identify any new coverage gaps.
6. Keep documentation synchronized with
   the FSEI.Domain.Tests project.

### 5.4 Documentation Status

Document Version: 1.0

Status: Initial Documentation Complete

Scope: FSEI V2 Domain Unit Tests

Framework: NUnit

Target Framework: .NET 10

The documentation reflects the reviewed
test source files.

Successful execution of the complete
test suite must be verified separately.

