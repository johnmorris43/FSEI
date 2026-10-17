# FSEI V2 — Core Domain Model

## 1. Purpose

The Flight Simulator Ecosystem Index (FSEI) is an evaluation
and information platform designed to assess products within
the flight simulation ecosystem.

FSEI V2 replaces the Excel-based V1 implementation with
a structured application and database architecture.

The domain model defines the entities, relationships,
and business rules used throughout the application.

### Core Objectives

- Maintain consistent, structured product information.
- Separate real-world aircraft specifications from simulator products.
- Support multiple flight simulator platforms.
- Track developers, product variants, and dependencies.
- Support evidence-based evaluations and scoring.
- Represent pricing, storefronts, and regional markets.
- Support future expansion into scenery, plugins, and services.

## 2. Domain Design Principles

### 2.1 Separation of Responsibilities

Each domain entity represents a specific business concept.

Examples:

- Aircraft represents a real-world aircraft model.
- AircraftVariant represents a specific aircraft variant.
- AircraftConfiguration represents a configuration of that variant.
- Product represents a commercial or distributable ecosystem product.
- Simulator represents a flight simulation platform.
- Developer represents an organization or individual developing products.

Real-world aircraft specifications must remain separate from
software product compatibility and commercial information.

### 2.2 Naming Conventions

FSEI follows these naming standards:

| Element | Convention | Example |
|---|---|---|
| Folder | Plural | Products |
| Namespace | Matches domain folder | FSEI.Domain.Products |
| Class | Singular | Product |
| Entity identifier | Class name + Id | ProductId |
| Navigation collection | Plural | ProductVariants |
| Navigation property | Singular | Product |
| Boolean property | Descriptive PascalCase | IsActive |
| Nullable property | Explicit nullable type | string? Notes |

### 2.3 Relationship Modeling

FSEI uses explicit relationship entities when relationships
require additional information or represent important
business concepts.

Examples:

- ProductVariant connects products to aircraft variants.
- ProductDeveloper connects products to developers.
- ConfigurationAvionics connects aircraft configurations
  to avionics systems.

Relationship cardinalities and required navigation properties
will be documented in the entity reference sections.

### 2.4 Validation and Business Rules

Domain validation protects the consistency of FSEI data.

Examples:

- Product requirement groups must contain at least one requirement.
- A product cannot directly require itself.
- Simulator operating system compatibility records validate
  their associated release boundaries.

Validation rules should be covered by automated unit tests.

### 2.5 Deferred Architecture Decisions

Some architectural decisions remain intentionally deferred.

DEV-013 will address product-to-simulator compatibility,
including simulator releases, operating system restrictions,
and official versus compatibility-layer support.

Deferred features should not be represented as implemented
domain relationships until their designs are approved.

## 3. Core Entity Reference

### 3.1 Aircraft Domain

The Aircraft domain represents real-world aircraft, their
manufacturers, variants, configurations, and engines.

These entities describe the aircraft being simulated rather
than the commercial software products used to simulate them.

#### 3.1.1 Aircraft

**Namespace:** `FSEI.Domain.Aircraft`

**Source:** `Aircraft/Aircraft.cs`

**Purpose:**
Represents a real-world aircraft model associated with
a manufacturer.

**Properties:**

| Property | Type | Description |
|---|---|---|
| AircraftId | int | Unique aircraft identifier |
| ManufacturerId | int | Manufacturer reference |
| Name | required string | Aircraft model name |
| Notes | string? | Optional aircraft information |
| Manufacturer | required Manufacturer | Manufacturer navigation |
| Variants | ICollection<AircraftVariant> | Aircraft variants |

**Relationships:**

- Manufacturer (1) → Aircraft (many)
- Aircraft (1) → AircraftVariant (many)

**User-facing display value:**

`Name`

Example: `Boeing 777`

**Business rules:**

- Aircraft must have a name.
- Aircraft must reference a manufacturer.
- Aircraft may contain multiple variants.
- Aircraft specifications remain separate from simulator
  software product information.

**Implementation notes:**

- The Variants collection is initialized to an empty collection.
- Required properties are enforced at C# object initialization.
- Database constraints have not yet been implemented.

#### 3.1.2 AircraftVariant

**Namespace:** `FSEI.Domain.Aircraft`

**Source:** `Aircraft/AircraftVariant.cs`

**Purpose:**

Represents a specific variant of a real-world aircraft model.

Examples include:
- Boeing 777-200ER
- Boeing 777-300ER
- Boeing 767-400ER
- Airbus A320neo

Aircraft variants remain separate from simulator software
products and their commercial offerings.

**Properties:**

| Property | Type | Description |
|---|---|---|
| AircraftVariantId | int | Unique variant identifier |
| AircraftId | int | Parent aircraft reference |
| Name | required string | Aircraft variant designation |
| Notes | string? | Optional variant information |
| Aircraft | required Aircraft | Parent aircraft navigation |
| AircraftConfigurations | ICollection<AircraftConfiguration> | Variant configurations |
| VariantEngines | ICollection<VariantEngine> | Engine relationship records |

**Relationships:**

- Aircraft (1) → AircraftVariant (many)
- AircraftVariant (1) → AircraftConfiguration (many)
- AircraftVariant (1) → VariantEngine (many)

**User-facing display value:**

`Name`

Example: `777-200ER`

The user interface may combine the parent aircraft name
with the variant designation when additional context is needed.

**Business rules:**

- Each aircraft variant must reference an aircraft.
- Each aircraft variant must have a name.
- A variant may have multiple aircraft configurations.
- A variant may have multiple associated engine records.
- Aircraft variants represent real-world aircraft specifications,
  not simulator-specific software editions.

**Implementation notes:**

- AircraftConfigurations is initialized to an empty collection.
- VariantEngines is initialized to an empty collection.
- Required navigation properties are enforced at C# initialization.
- Database relationship constraints remain future implementation work.

#### 3.1.3 AircraftConfiguration

**Namespace:** `FSEI.Domain.Aircraft`

**Source:** `Aircraft/AircraftConfiguration.cs`

**Purpose:**

Represents a named configuration associated with a
specific real-world aircraft variant.

Configurations allow FSEI to distinguish aircraft
equipment arrangements, including avionics systems,
without duplicating the parent aircraft variant.

**Properties:**

| Property | Type | Description |
|---|---|---|
| AircraftConfigurationId | int | Unique configuration identifier |
| AircraftVariantId | int | Parent aircraft variant reference |
| Name | required string | Configuration name |
| Notes | string? | Optional configuration information |
| AircraftVariant | required AircraftVariant | Parent variant navigation |
| ConfigurationAvionics | ICollection<ConfigurationAvionics> | Associated avionics relationship records |

**Relationships:**

- AircraftVariant (1) → AircraftConfiguration (many)
- AircraftConfiguration (1) → ConfigurationAvionics (many)

**User-facing display value:**

`Name`

The interface may display the configuration name
alongside its parent aircraft variant.

**Business rules:**

- Each configuration must reference an aircraft variant.
- Each configuration must have a name.
- A configuration may contain multiple avionics relationships.
- Aircraft configurations remain separate from software
  product compatibility and commercial offerings.

**Implementation notes:**

- ConfigurationAvionics is initialized to an empty collection.
- Required properties are enforced at C# object initialization.
- Database relationship constraints remain future implementation work.


#### 3.1.4 Manufacturer

**Namespace:** `FSEI.Domain.Aircraft`

**Source:** `Aircraft/Manufacturer.cs`

**Purpose:**

Represents an organization responsible for manufacturing
real-world aircraft or aircraft engines.

Examples include:
- Airbus
- Boeing
- General Electric
- Rolls-Royce
- Pratt & Whitney

Manufacturers are distinct from simulator software
developers and commercial product publishers.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ManufacturerId | int | Unique manufacturer identifier |
| Name | required string | Manufacturer name |
| Notes | string? | Optional manufacturer information |
| Aircraft | ICollection<Aircraft> | Associated aircraft models |
| Engines | ICollection<Engine> | Associated aircraft engines |

**Relationships:**

- Manufacturer (1) → Aircraft (many)
- Manufacturer (1) → Engine (many)

**User-facing display value:**

`Name`

Examples:
- Boeing
- Airbus
- General Electric

**Business rules:**

- Each manufacturer must have a name.
- A manufacturer may be associated with multiple aircraft.
- A manufacturer may be associated with multiple engines.
- Aircraft and engine manufacturers are represented using
  the same Manufacturer entity.
- Manufacturer and Developer remain separate domain concepts.

**Implementation notes:**

- Aircraft is initialized to an empty collection.
- Engines is initialized to an empty collection.
- Name is required at C# object initialization.
- Database uniqueness and relationship constraints remain
  future implementation work.
  Manufacturer
  |
  +---- Aircraft
  |        |
  |        +---- AircraftVariant
  |                  |
  |                  +---- AircraftConfiguration
  |
  +---- Engine


#### 3.1.5 Engine

**Namespace:** `FSEI.Domain.Aircraft`

**Source:** `Aircraft/Engine.cs`

**Purpose:**

Represents a real-world aircraft engine model associated
with an engine manufacturer.

Examples include:
- General Electric GE90-115B
- Rolls-Royce Trent 1000
- Pratt & Whitney PW1100G-JM
- CFM International LEAP-1A

Engine records represent real-world equipment rather
than simulator-specific engine implementations.

**Properties:**

| Property | Type | Description |
|---|---|---|
| EngineId | int | Unique engine identifier |
| ManufacturerId | int | Engine manufacturer reference |
| Manufacturer | required Manufacturer | Manufacturer navigation |
| Model | required string | Engine model designation |
| Notes | string? | Optional engine information |
| VariantEngines | ICollection<VariantEngine> | Associated aircraft variant relationships |

**Relationships:**

- Manufacturer (1) → Engine (many)
- Engine (1) → VariantEngine (many)

**User-facing display value:**

`Model`

Example: `GE90-115B`

The interface may combine the manufacturer name
and engine model when additional context is needed.

**Business rules:**

- Each engine must reference a manufacturer.
- Each engine must have a model designation.
- An engine may be associated with multiple aircraft variants.
- Aircraft variants may support multiple engine options.
- Engine records represent real-world equipment specifications,
  not simulator-specific engine implementations.

**Implementation notes:**

- VariantEngines is initialized to an empty collection.
- Manufacturer and Model are required at C# object initialization.
- Database relationship constraints remain future implementation work.

Manufacturer
|
+---- Engine
|
+---- VariantEngine
|
+---- AircraftVariant


#### 3.1.6 VariantEngine

**Namespace:** `FSEI.Domain.Aircraft`

**Source:** `Aircraft/VariantEngine.cs`

**Purpose:**

Represents the relationship between a real-world aircraft
variant and an engine model.

This junction entity allows aircraft variants to support
multiple engine options while allowing an engine model
to be associated with multiple aircraft variants.

**Properties:**

| Property | Type | Description |
|---|---|---|
| VariantEngineId | int | Unique relationship identifier |
| AircraftVariantId | int | Aircraft variant reference |
| EngineId | int | Engine model reference |
| Notes | string? | Optional relationship information |
| AircraftVariant | required AircraftVariant | Aircraft variant navigation |
| Engine | required Engine | Engine navigation |

**Relationships:**

- AircraftVariant (1) → VariantEngine (many)
- Engine (1) → VariantEngine (many)

Together, these relationships represent:

- AircraftVariant (many) ↔ Engine (many)

**User-facing display value:**

VariantEngine does not have its own display name.

The user interface should display the associated
engine model, optionally including its manufacturer.

Example:

`General Electric GE90-115B`

**Business rules:**

- Each VariantEngine record must reference an aircraft variant.
- Each VariantEngine record must reference an engine.
- An aircraft variant may have multiple engine options.
- An engine model may be associated with multiple aircraft variants.
- VariantEngine represents real-world aircraft-engine
  relationships, not simulator-specific implementations.

**Implementation notes:**

- VariantEngineId is the entity's primary identifier.
- AircraftVariantId and EngineId identify the related records.
- AircraftVariant and Engine are required navigation properties.
- No validation method is currently implemented.
- Database constraints, including prevention of duplicate
  aircraft-variant/engine pairs, remain future implementation work.

Manufacturer
├── Aircraft
│     └── AircraftVariant
│           ├── AircraftConfiguration
│           │     └── ConfigurationAvionics
│           └── VariantEngine
│                 └── Engine
│
└── Engine

### 3.2 Avionics Domain

The Avionics domain represents avionics classifications,
individual avionics systems, and their relationships
to aircraft configurations.

#### 3.2.1 AvionicsType

**Namespace:** `FSEI.Domain.Avionics`

**Source:** `Avionics/AvionicsType.cs`

**Purpose:**

Represents a classification of avionics systems.

AvionicsType provides a consistent way to organize
individual avionics systems by their functional category.

**Properties:**

| Property | Type | Description |
|---|---|---|
| AvionicsTypeId | int | Unique avionics type identifier |
| Name | required string | Avionics classification name |
| IsActive | bool | Indicates whether the classification is active |
| AvionicsSystems | ICollection<AvionicsSystem> | Associated avionics systems |

**Relationships:**

- AvionicsType (1) → AvionicsSystem (many)

**User-facing display value:**

`Name`

**Business rules:**

- Each avionics type must have a name.
- An avionics type may contain multiple avionics systems.
- IsActive defaults to true.
- Avionics types classify systems; they do not represent
  individual avionics products.

**Implementation notes:**

- AvionicsSystems is initialized to an empty collection.
- Database constraints remain future implementation work.

#### 3.2.2 AvionicsSystem

**Namespace:** `FSEI.Domain.Avionics`

**Source:** `Avionics/AvionicsSystem.cs`

**Purpose:**

Represents an individual avionics system that may be
associated with one or more aircraft configurations.

Avionics systems are organized by classification and
retain information about their model, manufacturer or
developer, and optional version or generation.

**Properties:**

| Property | Type | Description |
|---|---|---|
| AvionicsSystemId | int | Unique avionics system identifier |
| AvionicsTypeId | int | Avionics classification reference |
| DeveloperManufacturer | required string | Organization associated with the system |
| SystemModel | required string | Avionics system model |
| VersionGeneration | string? | Optional version or generation |
| Notes | string? | Optional system information |
| AvionicsType | required AvionicsType | Avionics classification navigation |
| ConfigurationAvionics | ICollection<ConfigurationAvionics> | Aircraft configuration relationship records |

**Relationships:**

- AvionicsType (1) → AvionicsSystem (many)
- AvionicsSystem (1) → ConfigurationAvionics (many)

**User-facing display value:**

`SystemModel`

The interface may combine DeveloperManufacturer,
SystemModel, and VersionGeneration when additional
context is needed.

**Business rules:**

- Each avionics system must reference an avionics type.
- Each avionics system must have a DeveloperManufacturer value.
- Each avionics system must have a SystemModel value.
- VersionGeneration and Notes are optional.
- An avionics system may be associated with multiple
  aircraft configurations.
- DeveloperManufacturer is currently stored as text
  and does not establish a relationship to the
  Developer or Manufacturer entities.

**Implementation notes:**

- ConfigurationAvionics is initialized to an empty collection.
- Required properties are enforced at C# object initialization.
- No validation method is currently implemented.
- Database constraints remain future implementation work.
  AvionicsType
  |
  +---- AvionicsSystem
  |
  +---- ConfigurationAvionics
  |
  +---- AircraftConfiguration

#### 3.2.3 ConfigurationAvionics

**Namespace:** `FSEI.Domain.Avionics`

**Source:** `Avionics/ConfigurationAvionics.cs`

**Purpose:**

Represents the association between an aircraft configuration
and an avionics system.

Allows avionics systems to be reused across aircraft
configurations while preserving configuration-specific
information and evaluation relationships.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ConfigurationAvionicsId | int | Unique association identifier |
| AircraftConfigurationId | int | Aircraft configuration reference |
| AvionicsSystemId | int | Avionics system reference |
| Role | string? | Optional role within the aircraft configuration |
| Notes | string? | Optional association information |
| AircraftConfiguration | required AircraftConfiguration | Aircraft configuration navigation |
| AvionicsSystem | required AvionicsSystem | Avionics system navigation |
| AvionicsEvaluations | ICollection<AvionicsEvaluation>? | Related avionics evaluations |

**Relationships:**

- AircraftConfiguration (1) → ConfigurationAvionics (many)
- AvionicsSystem (1) → ConfigurationAvionics (many)
- ConfigurationAvionics (1) → AvionicsEvaluation (many)

**User-facing display value:**

No dedicated display property is defined.

The interface may display the associated avionics
system's SystemModel and the optional Role.

**Business rules:**

- Each association references one aircraft configuration.
- Each association references one avionics system.
- Role and Notes are optional.
- An aircraft configuration may contain multiple avionics systems.
- An avionics system may appear in multiple aircraft configurations.
- An association may have multiple avionics evaluations.

**Implementation notes:**

- ConfigurationAvionics is an explicit junction entity.
- AvionicsEvaluations is declared nullable and initialized
  to an empty collection.
- Required navigation properties are enforced at C#
  object initialization.
- No validation method is currently implemented.
- Database uniqueness constraints are not yet defined.

## 3.3 Developers

### 3.3.1 Developer

**Namespace:** `FSEI.Domain.Developers`

**Source:** `Developers/Developer.cs`

**Purpose:**

Represents a developer or organization responsible for
creating flight simulation products.

Developers are maintained independently of products,
allowing multiple products to reference the same developer.

**Properties:**

| Property | Type | Description |
|---|---|---|
| DeveloperId | int | Unique developer identifier |
| Name | required string | Developer name |
| WebsiteUrl | string? | Optional website address |
| Description | string? | Optional developer description |
| IsActive | bool | Active record indicator; defaults to true |
| ProductDevelopers | ICollection<ProductDeveloper> | Associated product-development relationships |

**Relationships:**

- Developer (1) → ProductDeveloper (many)
- Product relationships are represented through ProductDeveloper.

**User-facing display value:**

Name

**Business rules:**

- Name is required during C# object initialization.
- WebsiteUrl and Description are optional.
- IsActive defaults to true.
- A developer may be associated with multiple products.
- Product associations are maintained through ProductDeveloper.

**Implementation notes:**

- ProductDevelopers is initialized to an empty collection.
- No custom validation method is currently implemented.
- Database uniqueness constraints are not yet defined.

### 3.3.2 ProductDeveloper

**Namespace:** `FSEI.Domain.Products`

**Source:** `Developers/ProductDeveloper.cs`

**Purpose:**

Represents the relationship between a flight simulation
product and a developer.

Supports products developed collaboratively by multiple
organizations and developers associated with multiple products.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ProductDeveloperId | int | Unique relationship identifier |
| ProductId | int | Foreign key to Product |
| DeveloperId | int | Foreign key to Developer |
| Role | string? | Optional developer contribution or role |
| Product | required Product | Product navigation property |
| Developer | required Developer | Developer navigation property |

**Relationships:**

- Product (1) → ProductDeveloper (many)
- Developer (1) → ProductDeveloper (many)
- Product ↔ Developer is a many-to-many relationship
  implemented through ProductDeveloper.

**User-facing display values:**

- Product.Name
- Developer.Name
- Role, when provided

**Business rules:**

- Each ProductDeveloper record associates one product
  with one developer.
- Product and Developer navigation properties are required
  during C# object initialization.
- Role is optional.
- Multiple developers may be associated with one product.
- One developer may be associated with multiple products.

**Implementation notes:**

- ProductDeveloper uses its own primary key.
- No custom validation method is currently implemented.
- Database uniqueness constraints are not yet defined.

## 3.4 Products

### 3.4.1 Product

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/Product.cs`

**Purpose:**

Represents a product within the flight simulation ecosystem.

Products are maintained independently of real-world aircraft,
allowing multiple simulation products to represent the same
aircraft or aircraft variant.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ProductId | int | Unique product identifier |
| ProductTypeId | int | Foreign key to ProductType |
| Name | required string | Product name |
| Description | string? | Optional product description |
| Notes | string? | Optional notes |
| IsActive | bool | Active record indicator; defaults to true |
| ProductType | required ProductType | Product category navigation |
| ProductVariants | ICollection<ProductVariant> | Associated aircraft variants |
| RequirementGroups | ICollection<ProductRequirementGroup> | Product prerequisite groups |
| CommercialOffers | ICollection<CommercialOffer> | Commercial offerings |

**Relationships:**

- ProductType (1) → Product (many)
- Product (1) → ProductVariant (many)
- Product (1) → ProductRequirementGroup (many)
- Product (1) → CommercialOffer (many)
- ProductDeveloper references Product, but Product currently
  has no corresponding navigation collection.

**User-facing display value:**

Name

**Business rules:**

- Name is required during C# object initialization.
- ProductType navigation is required during initialization.
- Description and Notes are optional.
- IsActive defaults to true.
- A product may have multiple associated aircraft variants.
- A product may have multiple prerequisite groups.
- A product may have multiple commercial offers.

**Implementation notes:**

- Navigation collections are initialized empty.
- No custom validation method is currently implemented.
- Database uniqueness constraints are not yet defined.
- Product and real-world Aircraft are separate entities.

### 3.4.2 ProductType

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductType.cs`

**Purpose:**

Defines the classification of products within the
flight simulation ecosystem.

Product types allow products to be organized into
categories independently of their simulator compatibility.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ProductTypeId | int | Unique product type identifier |
| Code | required string | Product classification code |
| Name | required string | Product classification name |
| Description | string? | Optional description |
| IsActive | bool | Active record indicator; defaults to true |
| Products | ICollection<Product> | Products assigned to this type |

**Relationships:**

- ProductType (1) → Product (many)
- Each Product references one ProductType.

**User-facing display value:**

Name

**Business rules:**

- Code and Name are required during C# object initialization.
- Description is optional.
- IsActive defaults to true.
- A product type may classify multiple products.
- Each product references one product type.

**Implementation notes:**

- Products is initialized to an empty collection.
- No custom validation method is currently implemented.
- Database uniqueness constraints are not yet defined.
- Product classification is separate from simulator compatibility.

### 3.4.3 ProductVariant

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductVariant.cs`

**Purpose:**

Associates a flight simulation product with the
real-world aircraft variant represented by that product.

This relationship allows multiple simulation products
to represent the same aircraft variant and allows
one product to represent multiple aircraft variants.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ProductVariantId | int | Unique relationship identifier |
| ProductId | int | References the simulation product |
| AircraftVariantId | int | References the real-world aircraft variant |
| Notes | string? | Optional relationship notes |
| Product | required Product | Product navigation property |
| AircraftVariant | required AircraftVariant | Aircraft variant navigation property |

**Relationships:**

- Product (1) → ProductVariant (many)
- AircraftVariant (1) → ProductVariant (many)
- Product and AircraftVariant have a many-to-many relationship through ProductVariant.

**User-facing display values:**

- Product.Name
- AircraftVariant.Name

**Business rules:**

- Each ProductVariant references one Product.
- Each ProductVariant references one AircraftVariant.
- A Product may represent multiple aircraft variants.
- An AircraftVariant may be represented by multiple products.
- Notes are optional.

**Implementation notes:**

- ProductVariant is an explicit relationship entity.
- Required navigation properties establish the intended relationships.
- No custom validation method is currently implemented.
- Database uniqueness constraints have not yet been defined.

### 3.4.4 ProductRequirementGroup

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductRequirementGroup.cs`

**Purpose:**

Defines a group of product ownership requirements
that must be satisfied before a dependent product
can be used or acquired.

Requirements within a group use OR logic.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ProductRequirementGroupId | int | Unique group identifier |
| ProductId | int | Product requiring another product |
| Description | string? | Optional description |
| Product | required Product | Parent product |
| Requirements | ICollection<ProductRequirement> | Accepted prerequisites |

**Relationships:**

- Product (1) → ProductRequirementGroup (many)
- ProductRequirementGroup (1) → ProductRequirement (many)

**Business rules:**

- Each group belongs to one product.
- A group must contain at least one requirement.
- A group is satisfied when at least one required
  product is owned.
- An empty group is not satisfied.
- Each requirement is validated individually.
- Direct self-dependencies are prohibited by
  ProductRequirement.Validate().

**Methods:**

- IsSatisfiedBy(IEnumerable<int> ownedProductIds)
  Returns true when at least one prerequisite is owned.

- Validate()
  Rejects empty groups and validates each requirement.

**Implementation notes:**

- Requirements within a group use OR logic.
- Validation must be invoked explicitly.
- Cross-group evaluation is not implemented
  by this entity.

### 3.4.5 ProductRequirement

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductRequirement.cs`

**Purpose:**

Represents an individual product prerequisite within
a ProductRequirementGroup.

Identifies a product that may be required before
another product can be used or acquired.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ProductRequirementId | int | Unique requirement identifier |
| ProductRequirementGroupId | int | Parent requirement group identifier |
| RequiredProductId | int | Prerequisite product identifier |
| Notes | string? | Optional requirement notes |
| ProductRequirementGroup | required ProductRequirementGroup | Parent requirement group |
| RequiredProduct | required Product | Required prerequisite product |

**Relationships:**

- ProductRequirementGroup (1) → ProductRequirement (many)
- Product (1) → ProductRequirement (many), as a prerequisite

**Business rules:**

- Each requirement belongs to one requirement group.
- Each requirement identifies one prerequisite product.
- Direct self-dependencies are prohibited.
- Requirements within the same group are evaluated
  using OR logic by ProductRequirementGroup.
- Notes are optional.

**Methods:**

- Validate()
  Throws InvalidOperationException when the parent
  product requires itself.

**Implementation notes:**

- Validation must be invoked explicitly.
- ProductRequirementGroup.Validate() invokes
  validation for each contained requirement.
- Indirect circular dependency detection is
  not currently implemented.
- Database uniqueness constraints have not
  yet been defined.

## 3.5 Simulators

The Simulators domain represents flight simulation
platforms, their product families, software releases,
lifecycle statuses, and operating system compatibility.

### 3.5.1 Simulator

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/Simulator.cs`

**Purpose:**

Represents an individual flight simulator product,
such as X-Plane 12 or Microsoft Flight Simulator 2024.

**Properties:**

| Property | Type | Description |
|---|---|---|
| SimulatorId | int | Unique simulator identifier |
| SimulatorFamilyId | int | Simulator family identifier |
| SimulatorStatusId | int | Lifecycle status identifier |
| Name | required string | User-facing simulator name |
| Version | string? | Optional version information |
| Notes | string? | Optional simulator notes |
| SimulatorStatus | required SimulatorStatus | Lifecycle status |
| SimulatorFamily | required SimulatorFamily | Parent simulator family |
| Releases | ICollection<SimulatorRelease> | Software release history |
| SimulatorOperatingSystemCompatibilities | ICollection<SimulatorOperatingSystemCompatibility> | Operating system compatibility records |

**Relationships:**

- SimulatorFamily (1) → Simulator (many)
- SimulatorStatus (1) → Simulator (many)
- Simulator (1) → SimulatorRelease (many)
- Simulator (1) → SimulatorOperatingSystemCompatibility (many)

**User-facing display value:**

- Simulator.Name

**Business rules:**

- Each simulator belongs to one simulator family.
- Each simulator has one lifecycle status.
- A simulator may have multiple software releases.
- A simulator may have multiple operating system
  compatibility records.
- Version and Notes are optional.

**Implementation notes:**

- Simulator represents the simulator product.
- SimulatorRelease represents an individual release.
- SimulatorStatus represents lifecycle classification.
- Operating system compatibility is modeled separately.
- The intended distinction between Simulator.Version
  and SimulatorRelease.Version requires clarification
  before database implementation.

### 3.5.2 SimulatorFamily

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/SimulatorFamily.cs`

**Purpose:**

Represents a family of related flight simulation
products across different generations.

Examples include X-Plane and Microsoft Flight Simulator.

**Properties:**

| Property | Type | Description |
|---|---|---|
| SimulatorFamilyId | int | Unique family identifier |
| Name | required string | User-facing family name |
| Notes | string? | Optional family notes |
| Simulators | ICollection<Simulator> | Simulators belonging to the family |

**Relationships:**

- SimulatorFamily (1) → Simulator (many)

**User-facing display value:**

- SimulatorFamily.Name

**Business rules:**

- A simulator family may contain multiple simulators.
- Each Simulator references one SimulatorFamily.
- A simulator family may exist without associated simulators.
- Name is required.
- Notes are optional.

**Implementation notes:**

- SimulatorFamily groups related simulator products.
- Individual simulator releases are represented
  through SimulatorRelease, not SimulatorFamily.
- No custom validation method is currently implemented.
- Database uniqueness constraints have not yet
  been defined.

### 3.5.3 SimulatorRelease

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/SimulatorRelease.cs`

**Purpose:**

Represents an individual software release of a flight
simulator, including its version, release date, and
optional release channel.

**Properties:**

| Property | Type | Description |
|---|---|---|
| SimulatorReleaseId | int | Unique release identifier |
| SimulatorId | int | Parent simulator identifier |
| Version | required string | Software release version |
| ReleaseDate | DateTime? | Optional release date |
| Notes | string? | Optional release notes |
| ReleaseChannel | string? | Optional release channel |
| Simulator | required Simulator | Parent simulator |

**Relationships:**

- Simulator (1) → SimulatorRelease (many)

**User-facing display values:**

- SimulatorRelease.Version
- SimulatorRelease.ReleaseChannel (when available)

**Business rules:**

- Each release belongs to one simulator.
- A simulator may have multiple releases.
- Version is required.
- ReleaseDate, Notes, and ReleaseChannel are optional.
- ReleaseChannel can distinguish stable, beta,
  and release-candidate builds.

**Implementation notes:**

- ReleaseChannel is currently stored as a string.
- No controlled release-channel values are enforced.
- No custom validation method is implemented.
- Database uniqueness constraints have not yet
  been defined.
- The distinction between Simulator.Version and
  SimulatorRelease.Version requires clarification
  before database implementation.

### 3.5.4 SimulatorStatus

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/SimulatorStatus.cs`

**Purpose:**

Defines the lifecycle status of a flight simulator,
allowing FSEI to distinguish currently supported
simulators from legacy or discontinued products.

**Properties:**

| Property | Type | Description |
|---|---|---|
| SimulatorStatusId | int | Unique status identifier |
| Code | required string | Status code |
| Name | required string | Display name |
| Description | string? | Optional status description |
| IsActive | bool | Indicates whether the status record is active |
| Simulators | ICollection<Simulator> | Simulators assigned this status |

**Relationships:**

- SimulatorStatus (1) → Simulator (many)

**Business rules:**

- Each simulator has one status.
- Multiple simulators may share the same status.
- Code and Name are required.
- IsActive defaults to true.
- IsActive controls the availability of the status
  record, not the lifecycle state of the simulator.

**Implementation notes:**

- SimulatorStatus is a reference-data entity.
- Status codes are not currently restricted to
  predefined values.
- No custom validation method is implemented.
- Database uniqueness constraints have not yet
  been defined.

### 3.6.1 CompatibilityStatus

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/CompatibilityStatus.cs`

**Purpose:**

Defines the compatibility status assigned to a simulator
and operating system combination.

This allows FSEI to distinguish supported configurations
from configurations with limited, unsupported, or
unknown compatibility.

**Properties:**

| Property | Type | Description |
|---|---|---|
| CompatibilityStatusId | int | Unique status identifier |
| Code | required string | Compatibility status code |
| Name | required string | Display name |
| Description | string? | Optional description |
| IsActive | bool | Indicates whether the status record is active |
| SimulatorOperatingSystemCompatibilities | ICollection<SimulatorOperatingSystemCompatibility> | Compatibility records assigned this status |

**Relationships:**

- CompatibilityStatus (1) →
  SimulatorOperatingSystemCompatibility (many)

**Business rules:**

- Code and Name are required.
- IsActive defaults to true.
- Multiple compatibility records may share the same status.
- IsActive controls the availability of the status record,
  not whether a simulator is compatible.
- Compatibility status values are not currently restricted
  to predefined codes.

**Implementation notes:**

- CompatibilityStatus is a reference-data entity.
- No custom validation method is implemented.
- Database uniqueness constraints have not yet been defined.

### 3.6.2 OperatingSystem

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/OperatingSystem.cs`

**Purpose:**

Represents an operating system supported or evaluated
within the flight simulation ecosystem.

Operating systems are tracked independently from
simulators so FSEI can document compatibility across
different simulator products and operating environments.

**Properties:**

| Property | Type | Description |
|---|---|---|
| OperatingSystemId | int | Unique operating system identifier |
| Name | required string | Operating system display name |
| Version | string? | Optional operating system version |
| Notes | string? | Additional information |
| SimulatorOperatingSystemCompatibilities | ICollection<SimulatorOperatingSystemCompatibility> | Simulator compatibility records associated with this operating system |

**Relationships:**

- OperatingSystem (1) →
  SimulatorOperatingSystemCompatibility (many)

**Business rules:**

- Name is required.
- Version is optional.
- Notes are optional.
- An operating system may have multiple simulator
  compatibility records.
- An operating system may exist without any
  compatibility records.

**Implementation notes:**

- OperatingSystem is a domain entity.
- No custom validation method is implemented.
- No IsActive property is currently defined.
- Operating system compatibility is represented
  through SimulatorOperatingSystemCompatibility.
- Processor architecture is modeled separately.
- Database uniqueness constraints have not yet
  been defined.

### 3.6.3 ProcessorArchitecture

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/ProcessorArchitecture.cs`

**Purpose:**

Defines the processor architectures used to evaluate
flight simulator compatibility.

Processor architectures are tracked independently from
operating systems to support compatibility assessments
across different hardware environments.

**Properties:**

| Property | Type | Description |
|---|---|---|
| ProcessorArchitectureId | int | Unique architecture identifier |
| Code | required string | Architecture code |
| Name | required string | Display name |
| Description | string? | Optional description |
| IsActive | bool | Indicates whether the architecture record is active |
| SimulatorOperatingSystemCompatibilities | ICollection<SimulatorOperatingSystemCompatibility> | Simulator compatibility records associated with this architecture |

**Relationships:**

- ProcessorArchitecture (1) →
  SimulatorOperatingSystemCompatibility (many)

**Business rules:**

- Code and Name are required.
- IsActive defaults to true.
- Multiple simulator compatibility records may reference
  the same processor architecture.
- A processor architecture may exist without any
  simulator compatibility records.
- IsActive indicates whether the architecture record
  is available for use.

**Implementation notes:**

- ProcessorArchitecture is a reference-data entity.
- No custom validation method is implemented.
- No predefined architecture codes are enforced.
- Database uniqueness constraints have not yet
  been defined.

### 3.6.4 SimulatorOperatingSystemCompatibility

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/SimulatorOperatingSystemCompatibility.cs`

**Purpose:**

Represents the compatibility of a flight simulator with
a particular operating system and, optionally, a
processor architecture.

Compatibility can also be associated with a range of
simulator releases.

**Properties:**

| Property | Type | Description |
|---|---|---|
| SimulatorOperatingSystemCompatibilityId | int | Unique compatibility record identifier |
| SimulatorId | int | Associated simulator identifier |
| OperatingSystemId | int | Associated operating system identifier |
| ProcessorArchitectureId | int? | Optional processor architecture identifier |
| CompatibilityStatusId | int | Assigned compatibility status identifier |
| SupportedFromReleaseId | int? | Optional starting simulator release identifier |
| SupportedThroughReleaseId | int? | Optional ending simulator release identifier |
| Notes | string? | Additional compatibility information |
| Simulator | required Simulator | Associated simulator |
| OperatingSystem | required OperatingSystem | Associated operating system |
| ProcessorArchitecture | ProcessorArchitecture? | Optional processor architecture |
| CompatibilityStatus | required CompatibilityStatus | Assigned compatibility status |
| SupportedFromRelease | SimulatorRelease? | Optional starting release |
| SupportedThroughRelease | SimulatorRelease? | Optional ending release |

**Relationships:**

- Simulator (1) →
  SimulatorOperatingSystemCompatibility (many)
- OperatingSystem (1) →
  SimulatorOperatingSystemCompatibility (many)
- ProcessorArchitecture (1) →
  SimulatorOperatingSystemCompatibility (many, optional)
- CompatibilityStatus (1) →
  SimulatorOperatingSystemCompatibility (many)
- SimulatorRelease (1) →
  SimulatorOperatingSystemCompatibility (many)
  through optional starting and ending release references.

**Business rules:**

- A compatibility record must reference a simulator.
- A compatibility record must reference an operating system.
- A compatibility record must reference a compatibility status.
- Processor architecture is optional.
- Starting and ending simulator releases are optional.
- Any specified starting release must belong to the
  compatibility record's simulator.
- Any specified ending release must belong to the
  compatibility record's simulator.

**Validation:**

`ValidateReleaseBoundaries()`

Throws InvalidOperationException when:

1. SupportedFromRelease belongs to a different simulator.
2. SupportedThroughRelease belongs to a different simulator.

**Implementation notes:**

- Release boundary validation is implemented in the
  domain model.
- Release ordering is not currently validated.
- The model does not currently enforce uniqueness
  of compatibility combinations.
- Database foreign key constraints have not yet
  been configured.
- Processor architecture is modeled independently
  from the operating system.

## 3.7 Stores

The Stores domain represents commercial offerings,
acquisition methods, billing intervals, storefronts,
accepted payment methods, and historical pricing.

These entities separate flight simulation products
from their commercial availability and purchasing options.

The domain supports multiple storefronts, acquisition
arrangements, currencies, and geographic markets.

### 3.7.1 AcquisitionType

**Namespace:** `FSEI.Domain.Stores`

**Purpose:**

Defines the acquisition categories used to classify how products
or commercial offerings are obtained.

**Properties:**

- `AcquisitionTypeId` (int) — Primary key.
- `Code` (required string) — Acquisition category code.
- `Name` (required string) — Display name.
- `Description` (string?, optional) — Additional information.
- `IsActive` (bool) — Indicates whether the category is active.
  Defaults to true.

**Relationships:**

- No navigation properties are currently defined in this class.

**Business rules:**

- Code and Name are required during object initialization.
- New acquisition types are active by default.
- Inactive acquisition types can remain available for historical records.

**Validation:**

- No custom validation method is currently implemented.

**Implementation notes:**

- Code uniqueness is not currently enforced by this class.
- Database constraints have not yet been configured.

### 3.7.2 BillingInterval

**Namespace:** `FSEI.Domain.Stores`

**Purpose:**

Defines the billing intervals used for recurring commercial
offerings and subscription-based products.

Examples include monthly and annual billing.

**Properties:**

- `BillingIntervalId` (int) — Primary key.
- `Code` (required string) — Billing interval code.
- `Name` (required string) — Display name.
- `Description` (string?, optional) — Additional information.
- `IsActive` (bool) — Indicates whether the billing interval
  is active. Defaults to true.

**Relationships:**

- No navigation properties are currently defined in this class.

**Business rules:**

- Code and Name must be initialized.
- New billing intervals are active by default.
- Inactive billing intervals may be retained for historical records.

**Validation:**

- No custom validation method is currently implemented.

**Implementation notes:**

- Code uniqueness is not currently enforced by this class.
- The class does not define billing duration or recurrence calculations.
- Database constraints have not yet been configured.

### 3.7.3 CommercialOffer

**Namespace:** `FSEI.Domain.Stores`

**Purpose:**

Represents a commercial offering of a product through a
specific storefront.

Separates the product itself from where and how it is sold.

**Properties:**

- `CommercialOfferId` (int) — Primary key.
- `ProductId` (int) — References the associated product.
- `StorefrontId` (int) — References the selling storefront.
- `Url` (string?, optional) — Direct URL to the commercial offer.
- `Notes` (string?, optional) — Additional information.
- `IsActive` (bool) — Indicates whether the offer is active.
  Defaults to true.

**Relationships:**

- `Product` (required Product) — The product being offered.
- `Storefront` (required Storefront) — The storefront providing
  the offer.
- `OfferOptions` (ICollection<OfferOption>) — Purchasing options
  associated with the offer.

**Business rules:**

- A commercial offer must reference a product.
- A commercial offer must reference a storefront.
- A commercial offer may contain multiple purchasing options.
- The offer URL is optional.
- New commercial offers are active by default.
- An inactive commercial offer may remain available for
  historical reference.

**Validation:**

- No custom validation method is currently implemented.

**Implementation notes:**

- Pricing is not stored directly on CommercialOffer.
- Purchasing options are represented through OfferOption.
- The model supports multiple storefront offers for the
  same product.
- Database foreign key and uniqueness constraints have
  not yet been configured.

### 3.7.4 OfferOption

**Namespace:** `FSEI.Domain.Stores`

**Purpose:**

Represents a purchasing or acquisition option associated
with a commercial offer.

Supports different acquisition arrangements, including
one-time purchases and recurring subscriptions.

**Properties:**

- `OfferOptionId` (int) — Primary key.
- `CommercialOfferId` (int) — References the commercial offer.
- `AcquisitionTypeId` (int) — References the acquisition category.
- `BillingIntervalId` (int?, optional) — References the billing interval.
- `Name` (required string) — Display name.
- `Description` (string?, optional) — Additional information.
- `IsActive` (bool) — Indicates whether the option is active.
  Defaults to true.

**Relationships:**

- `CommercialOffer` (required CommercialOffer) — Parent offer.
- `AcquisitionType` (required AcquisitionType) — Acquisition category.
- `BillingInterval` (BillingInterval?, optional) — Billing frequency.

**Business rules:**

- An offer option must reference a commercial offer.
- An offer option must reference an acquisition type.
- Billing intervals are optional.
- Billing interval IDs and navigation references must be consistent.
- New offer options are active by default.
- Inactive options may be retained for historical reference.

**Validation:**

`Validate()`

Throws InvalidOperationException when:

1. CommercialOfferId does not match CommercialOffer.CommercialOfferId.
2. AcquisitionTypeId does not match AcquisitionType.AcquisitionTypeId.
3. BillingIntervalId is populated but BillingInterval is null.
4. BillingIntervalId does not match BillingInterval.BillingIntervalId.
5. BillingInterval is populated but BillingIntervalId is null.

**Implementation notes:**

- Acquisition categories are represented by AcquisitionType.
- Billing frequency is represented by BillingInterval.
- Pricing is not stored directly on OfferOption.
- The current validation checks relationship consistency.
- Acquisition-specific billing requirements are not yet enforced.
- Database foreign key constraints have not yet been configured.

### 3.7.5 PriceObservation

**Namespace:** `FSEI.Domain.Stores`

**Purpose:**

Records an observed price for a commercial offer at a
specific date and time.

Supports historical price tracking, multiple currencies,
geographic markets, and optional acquisition options.

**Properties:**

- `PriceObservationId` (int) — Primary key.
- `CommercialOfferId` (int) — Associated commercial offer.
- `CurrencyId` (int) — Currency reference.
- `MarketId` (int?, optional) — Geographic market reference.
- `OfferOptionId` (int?, optional) — Acquisition option reference.
- `Amount` (decimal) — Observed price.
- `ObservedAt` (DateTime) — Observation timestamp.

**Relationships:**

- `CommercialOffer` (required CommercialOffer)
- `Currency` (required Currency)
- `Market` (Market?, optional)
- `OfferOption` (OfferOption?, optional)

**Business rules:**

- Every price observation belongs to a commercial offer.
- Every observation references a currency.
- Observed amounts cannot be negative.
- An observation must have a timestamp.
- Geographic market information is optional.
- Acquisition option information is optional.
- A specified acquisition option must belong to the
  same commercial offer.

**Validation:**

`Validate()`

Throws InvalidOperationException when:

1. CommercialOfferId does not match CommercialOffer.
2. CurrencyId does not match Currency.
3. Amount is negative.
4. ObservedAt contains the default DateTime value.
5. MarketId and Market are inconsistent.
6. OfferOptionId and OfferOption are inconsistent.
7. OfferOption belongs to another commercial offer.

**Implementation notes:**

- Uses decimal for financial precision.
- Supports historical pricing observations.
- Does not represent an individual customer's purchase.
- Currency conversion is not performed by this entity.
- UTC timestamp conventions should be established
  before database implementation.
- Database constraints have not yet been configured.

### 3.7.6 Storefront

**Namespace:** `FSEI.Domain.Stores`

**Purpose:**

Represents a storefront or retailer through which flight
simulation products are commercially offered.

**Properties:**

- `StorefrontId` (int) — Primary key.
- `Name` (required string) — Storefront name.
- `Url` (string?, optional) — Storefront website.
- `Notes` (string?, optional) — Additional information.
- `IsActive` (bool) — Indicates whether the storefront is active.
  Defaults to true.

**Relationships:**

- `CommercialOffers` (ICollection<CommercialOffer>)
  — Commercial offers associated with this storefront.

**Business rules:**

- A storefront must have a name.
- A storefront may have multiple commercial offers.
- Website information is optional.
- Inactive storefronts may be retained for historical purposes.

**Validation:**

No custom Validate() method is currently implemented.

**Implementation notes:**

- Storefronts are modeled independently from products.
- CommercialOffer connects a product to a storefront.
- Storefront records support historical pricing information.
- Database uniqueness constraints have not yet been configured.

### 3.7.7 StorefrontPaymentMethod

**Namespace:** `FSEI.Domain.Stores`

**Purpose:**

Represents a payment method accepted by a storefront.

Provides a many-to-many relationship between storefronts
and payment methods through a dedicated entity.

**Properties:**

- `StorefrontPaymentMethodId` (int) — Primary key.
- `StorefrontId` (int) — Storefront reference.
- `PaymentMethodId` (int) — Payment method reference.
- `IsActive` (bool) — Whether the relationship is active.
  Defaults to true.

**Relationships:**

- `Storefront` (Storefront?, optional navigation)
- `PaymentMethod` (PaymentMethod?, optional navigation)

**Business rules:**

- Each record associates one storefront with one payment method.
- A storefront may accept multiple payment methods.
- A payment method may be accepted by multiple storefronts.
- Individual relationships may be deactivated without
  deactivating the storefront or payment method.

**Validation:**

`Validate()`

Throws InvalidOperationException when:

1. A populated Storefront navigation does not match StorefrontId.
2. A populated PaymentMethod navigation does not match PaymentMethodId.

**Implementation notes:**

- Navigation properties are nullable.
- Validation does not require navigation properties to be populated.
- Database foreign-key constraints have not yet been configured.
- A unique index on StorefrontId and PaymentMethodId is recommended.

## 3.8 Evaluations

The Evaluations domain defines the methodologies,
components, assessments, and scoring mechanisms
used to evaluate flight simulation products.

The domain supports versioned evaluation methodologies,
component-level scoring, aircraft systems-depth
evaluations, and avionics fidelity evaluations.

Evaluation results are calculated from individual
component assessments rather than maintained
as independent, manually entered totals.

Evaluation scopes distinguish between aircraft
families, individual variants, and aircraft
configurations.

The domain also supports evaluation evidence,
scoring guidance, and historical methodology
versions to provide a structured foundation
for consistent and traceable evaluations.


### 3.8.1 EvaluationMethodology

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/EvaluationMethodology.cs`

**Purpose:**

Represents a methodology used to evaluate components
within the flight simulation ecosystem.

Provides a common definition for evaluation methodologies
while allowing their criteria to evolve through
separately maintained methodology versions.

**Properties:**

- `EvaluationMethodologyId` (int) — Primary key.
- `Name` (required string) — Methodology name.
- `Description` (string?, optional) — Additional information.
- `IsActive` (bool) — Indicates whether the methodology
  is active. Defaults to true.

**Relationships:**

- `Versions` (ICollection<EvaluationMethodologyVersion>)
  — Versions associated with this methodology.

**Business rules:**

- Each methodology must have a Name initialized.
- A methodology may have multiple versions.
- A methodology may exist without any versions.
- New methodologies are active by default.
- A methodology may be deactivated without deleting
  its associated versions.

**Validation:**

- No custom `Validate()` method is currently implemented.
- The `required` keyword enforces initialization of Name
  but does not prevent empty or whitespace-only values.

**Implementation notes:**

- Versions is initialized to an empty collection.
- Methodology version details are maintained separately
  through EvaluationMethodologyVersion.
- Database uniqueness and foreign-key constraints
  have not yet been configured.

### 3.8.2 EvaluationMethodologyVersion

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/EvaluationMethodologyVersion.cs`

**Purpose:**

Represents a specific version of an evaluation methodology.

Allows evaluation criteria to evolve over time while
maintaining distinct methodology versions.

Each methodology version contains the evaluation
components associated with that version.

**Properties:**

- `EvaluationMethodologyVersionId` (int) — Primary key.
- `EvaluationMethodologyId` (int) — Parent methodology reference.
- `VersionNumber` (int) — Methodology version number.
- `EffectiveDate` (DateTime) — Date the version becomes effective.
- `Notes` (string?, optional) — Additional version information.
- `IsActive` (bool) — Indicates whether the version is active.
  Defaults to true.

**Relationships:**

- `EvaluationMethodology` (required EvaluationMethodology)
  — Parent evaluation methodology.
- `Components` (ICollection<EvaluationComponent>)
  — Evaluation components associated with this version.

**Business rules:**

- Each methodology version belongs to one evaluation methodology.
- A methodology may contain multiple versions.
- Each version has its own version number.
- Each version has an effective date.
- A methodology version may contain multiple evaluation components.
- A methodology version may exist without any components.
- New methodology versions are active by default.
- Previous methodology versions may be retained for historical reference.

**Validation:**

- No custom `Validate()` method is currently implemented.
- VersionNumber is not currently restricted to positive values.
- EffectiveDate is not currently checked for an uninitialized value.
- The class does not enforce unique version numbers
  within an evaluation methodology.

**Implementation notes:**

- Components is initialized to an empty collection.
- EvaluationMethodology is a required navigation property.
- VersionNumber and EffectiveDate are stored directly
  on the methodology version.
- Evaluation components are maintained separately
  through EvaluationComponent.
- Database foreign-key and uniqueness constraints
  have not yet been configured.

### 3.8.3 EvaluationComponent

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/EvaluationComponent.cs`

**Purpose:**

Represents an individual scoring component associated
with a specific evaluation methodology version.

Evaluation components define the criteria used to
assess systems depth or avionics fidelity.

Each component specifies its maximum possible score
and may be referenced by multiple assessments.

**Properties:**

- `EvaluationComponentId` (int) — Primary key.
- `EvaluationMethodologyVersionId` (int) — Parent methodology
  version reference.
- `Name` (required string) — Evaluation component name.
- `MaximumScore` (int) — Maximum score permitted for
  assessments using this component.
- `Description` (string?, optional) — Additional information.
- `IsActive` (bool) — Indicates whether the component is active.
  Defaults to true.

**Relationships:**

- `EvaluationMethodologyVersion` (required EvaluationMethodologyVersion)
  — Parent methodology version.
- `Assessments` (ICollection<EvaluationComponentAssessment>)
  — Assessments associated with this component.

**Business rules:**

- Each evaluation component belongs to one methodology version.
- Each component must have a Name initialized.
- A methodology version may contain multiple components.
- A component may be used by multiple assessments.
- A component may exist without any assessments.
- New evaluation components are active by default.
- Assessment scores must fall between zero and MaximumScore
  when assigned through EvaluationComponentAssessment.SetScore().

**Validation:**

- No custom `Validate()` method is currently implemented.
- MaximumScore is not currently validated by this class.
- The class does not prevent negative MaximumScore values.
- Name initialization is required, but empty or
  whitespace-only names are not explicitly rejected.

**Implementation notes:**

- Assessments is initialized to an empty collection.
- EvaluationMethodologyVersion is a required navigation property.
- MaximumScore is used by EvaluationComponentAssessment
  to validate individual assessment scores.
- MaximumScore also contributes to calculated evaluation
  percentages in SystemsDepthEvaluation and AvionicsEvaluation.
- Database foreign-key and uniqueness constraints
  have not yet been configured.

### 3.8.4 EvaluationComponentAssessment

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/EvaluationComponentAssessment.cs`

**Purpose:**

Represents an individual assessment of an evaluation component.

Stores the score and optional commentary associated with
a component evaluated as part of a systems-depth or
avionics evaluation.

Ensures that each assessment belongs to exactly one
evaluation and references a component associated with
the selected evaluation methodology version.

**Properties:**

- `EvaluationComponentAssessmentId` (int) — Primary key.
- `AvionicsEvaluationId` (int?, optional) — Avionics evaluation reference.
- `SystemsDepthEvaluationId` (int?, optional) — Systems-depth evaluation reference.
- `EvaluationComponentId` (int) — Evaluation component reference.
- `Score` (int?, private setter) — Individual component score.
- `Commentary` (string?, optional) — Assessment commentary.

**Relationships:**

- `EvaluationComponent` (required EvaluationComponent)
  — Component being assessed.
- `AvionicsEvaluation` (AvionicsEvaluation?, optional)
  — Associated avionics evaluation.
- `SystemsDepthEvaluation` (SystemsDepthEvaluation?, optional)
  — Associated systems-depth evaluation.

**Business rules:**

- Each assessment references one evaluation component.
- An assessment must belong to exactly one evaluation.
- The assessment may belong to either a systems-depth
  evaluation or an avionics evaluation, but not both.
- An assessment cannot exist without an evaluation assignment
  when ValidateEvaluationAssignment() is invoked.
- Score is optional and may remain null.
- Assigned scores must fall between zero and the
  associated evaluation component's MaximumScore.
- The evaluation component must belong to the
  specified evaluation methodology version.
- Commentary is optional.

**Methods:**

`SetScore(int? score)`

- Accepts a nullable integer score.
- Allows the score to be cleared by assigning null.
- Rejects scores below zero.
- Rejects scores exceeding EvaluationComponent.MaximumScore.
- Throws ArgumentOutOfRangeException for invalid scores.

`ValidateMethodologyVersion(
    EvaluationMethodologyVersion evaluationMethodologyVersion)`

- Verifies that EvaluationComponent.EvaluationMethodologyVersionId
  matches the supplied methodology version identifier.
- Throws InvalidOperationException when the identifiers differ.

`ValidateEvaluationAssignment()`

- Verifies that exactly one evaluation identifier is populated.
- Rejects assessments assigned to neither evaluation type.
- Rejects assessments assigned to both evaluation types.
- Throws InvalidOperationException for invalid assignments.

`Validate(
    EvaluationMethodologyVersion evaluationMethodologyVersion)`

- Calls ValidateEvaluationAssignment().
- Calls ValidateMethodologyVersion().
- Ensures both validation checks are performed.

**Implementation notes:**

- Score uses a private setter and is assigned through SetScore().
- Score validation uses the associated EvaluationComponent.MaximumScore.
- Validation methods must be invoked explicitly.
- Evaluation assignment validation checks the nullable
  foreign-key identifiers rather than navigation properties.
- Methodology version validation checks the component's
  methodology version identifier.
- Database foreign-key and relationship constraints
  have not yet been configured.

### 3.8.5 EvaluationScope

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/EvaluationScope.cs`

**Purpose:**

Defines the scope used to identify the level at which
a systems-depth evaluation is performed.

Allows FSEI to distinguish evaluations performed at
the aircraft family, aircraft variant, or aircraft
configuration level.

**Properties:**

- `EvaluationScopeId` (int) — Primary key.
- `Name` (required string) — Evaluation scope display name.
- `Code` (required string) — Evaluation scope identifier.
- `IsActive` (bool) — Indicates whether the scope is active.
  Defaults to true.

**Relationships:**

- `SystemsDepthEvaluations` (ICollection<SystemsDepthEvaluation>)
  — Systems-depth evaluations associated with this scope.

**Business rules:**

- Each evaluation scope must have a Name initialized.
- Each evaluation scope must have a Code initialized.
- A scope may be associated with multiple systems-depth evaluations.
- A scope may exist without associated evaluations.
- New evaluation scopes are active by default.
- SystemsDepthEvaluation.ValidateEvaluationTarget()
  recognizes the following scope codes:
  - AIRCRAFT_FAMILY
  - VARIANT
  - CONFIGURATION

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name and Code are required during C# object initialization.
- Empty or whitespace-only values are not explicitly rejected.
- Code uniqueness is not currently enforced by this class.
- The class itself does not restrict Code to predefined values.

**Implementation notes:**

- SystemsDepthEvaluations is initialized to an empty collection.
- EvaluationScope is referenced by SystemsDepthEvaluation.
- Scope-to-target consistency is validated by
  SystemsDepthEvaluation.ValidateEvaluationTarget().
- Unknown scope codes are rejected by that method.
- Database foreign-key and uniqueness constraints
  have not yet been configured.

### 3.8.6 SystemsDepthEvaluation

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/SystemDepthEvaluation.cs`

**Purpose:**

Represents a systems-depth evaluation performed against
a real-world aircraft, aircraft variant, or aircraft
configuration.

Associates the evaluation with a specific methodology
version and evaluation scope.

Calculates an overall systems-depth percentage from
individual component assessments.

**Properties:**

- `SystemsDepthEvaluationId` (int) — Primary key.
- `EvaluationMethodologyVersionId` (int) — Methodology version reference.
- `EvaluationScopeId` (int) — Evaluation scope reference.
- `AircraftId` (int?, optional) — Aircraft reference.
- `VariantId` (int?, optional) — Aircraft variant reference.
- `AircraftConfigurationId` (int?, optional) — Aircraft configuration reference.
- `SystemsDepthScore` (int?, calculated) — Overall systems-depth percentage.
- `DateVerified` (DateTime?, optional) — Date the evaluation was verified.
- `Notes` (string?, optional) — Additional evaluation information.

**Relationships:**

- `Aircraft` (Aircraft?, optional) — Aircraft being evaluated.
- `Variant` (AircraftVariant?, optional) — Aircraft variant being evaluated.
- `AircraftConfiguration` (AircraftConfiguration?, optional)
  — Aircraft configuration being evaluated.
- `EvaluationScope` (required EvaluationScope)
  — Scope of the evaluation.
- `EvaluationMethodologyVersion` (required EvaluationMethodologyVersion)
  — Methodology version used for the evaluation.
- `Sources` (ICollection<EvaluationSource>)
  — Supporting evaluation evidence relationships.
- `Assessments` (ICollection<EvaluationComponentAssessment>)
  — Component assessments contributing to the score.

**Business rules:**

- Each systems-depth evaluation references one methodology version.
- Each systems-depth evaluation references one evaluation scope.
- An evaluation must target exactly one of the following:
  - Aircraft
  - AircraftVariant
  - AircraftConfiguration
- The selected evaluation target must match the evaluation scope.
- An evaluation may contain multiple component assessments.
- Only assessments containing a score contribute to SystemsDepthScore.
- The calculated score is expressed as a percentage.
- SystemsDepthScore is null when no assessments have scores.
- SystemsDepthScore is null when the combined maximum
  possible score of evaluated assessments equals zero.
- DateVerified and Notes are optional.

**Calculated properties:**

`SystemsDepthScore`

- Selects assessments where Score contains a value.
- Calculates the sum of the assigned assessment scores.
- Calculates the sum of their associated component maximum scores.
- Divides the total assigned score by the total maximum score.
- Multiplies the result by 100.
- Rounds to the nearest integer using
  MidpointRounding.AwayFromZero.
- Returns null when no scored assessments exist.
- Returns null when the combined maximum score equals zero.

**Validation:**

`ValidateEvaluationTarget()`

Throws InvalidOperationException when:

1. No evaluation target identifier is populated.
2. More than one evaluation target identifier is populated.
3. EvaluationScope.Code contains an unrecognized value.
4. The populated evaluation target does not match
   the selected evaluation scope.

Recognized scope codes:

- `AIRCRAFT_FAMILY` — Requires AircraftId.
- `VARIANT` — Requires VariantId.
- `CONFIGURATION` — Requires AircraftConfigurationId.

**Implementation notes:**

- SystemsDepthScore is calculated dynamically and has no setter.
- The calculation uses only assessments with populated scores.
- Sources and Assessments are initialized to empty collections.
- EvaluationScope and EvaluationMethodologyVersion are
  required navigation properties.
- Aircraft, Variant, and AircraftConfiguration navigations
  are nullable.
- ValidateEvaluationTarget() must be invoked explicitly.
- Target validation checks the populated foreign-key identifiers.
- The current method does not verify that populated target
  navigation properties match their corresponding identifiers.
- The current method does not validate methodology-version
  consistency for individual component assessments.
- Database foreign-key and relationship constraints
  have not yet been configured.

### 3.8.7 SystemsDepthScoreGuidance

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/SystemDepthScoringGuide.cs`

**Purpose:**

Defines descriptive scoring guidance for systems-depth
evaluations.

Associates a score range with a label and description
to provide consistent interpretation of evaluation scores.

**Properties:**

- `SystemsDepthScoreGuidanceId` (int) — Primary key.
- `MinimumScore` (int) — Lower boundary of the score range.
- `MaximumScore` (int) — Upper boundary of the score range.
- `Label` (required string) — Descriptive scoring label.
- `Description` (required string) — Explanation of the scoring range.
- `IsActive` (bool) — Indicates whether the guidance record
  is active. Defaults to true.

**Relationships:**

- No navigation properties are currently defined.
- No direct relationship to SystemsDepthEvaluation
  is implemented in this class.

**Business rules:**

- Each guidance record defines a minimum and maximum score.
- Each guidance record requires a Label and Description
  during C# object initialization.
- New guidance records are active by default.
- Guidance records may be deactivated without deletion.

**Validation:**

- No custom `Validate()` method is currently implemented.
- MinimumScore and MaximumScore are not currently
  validated by this class.
- The class does not enforce MinimumScore <= MaximumScore.
- The class does not prevent overlapping score ranges.
- Label and Description are required during initialization,
  but empty or whitespace-only values are not explicitly rejected.

**Implementation notes:**

- SystemsDepthScoreGuidance is a standalone domain entity.
- Score ranges are represented using integer values.
- The class does not currently contain logic for
  selecting guidance based on a calculated evaluation score.
- The class does not define a scoring scale or enforce
  specific minimum and maximum boundaries.
- Database constraints have not yet been configured.

### 3.8.8 AvionicsEvaluation

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/AvionicsEvaluation.cs`

**Purpose:**

Represents an evaluation of avionics fidelity associated
with a specific aircraft configuration and avionics system.

Associates the evaluation with a methodology version
and calculates an overall fidelity percentage from
individual component assessments.

Supports evidence-based evaluation through associated
evaluation sources.

**Properties:**

- `AvionicsEvaluationId` (int) — Primary key.
- `ConfigurationAvionicsId` (int) — Aircraft configuration
  avionics relationship reference.
- `EvaluationMethodologyVersionId` (int) — Methodology
  version reference.
- `EvaluationDate` (DateTime?, optional) — Evaluation date.
- `FidelityScore` (int?, calculated) — Overall avionics
  fidelity percentage.
- `Notes` (string?, optional) — Additional evaluation information.

**Relationships:**

- `ConfigurationAvionics` (required ConfigurationAvionics)
  — Aircraft configuration and avionics association.
- `EvaluationMethodologyVersion` (required EvaluationMethodologyVersion)
  — Methodology version used for the evaluation.
- `Sources` (ICollection<AvionicsEvaluationSource>)
  — Supporting evaluation evidence relationships.
- `Assessments` (ICollection<EvaluationComponentAssessment>)
  — Component assessments contributing to the fidelity score.

**Business rules:**

- Each avionics evaluation references one ConfigurationAvionics record.
- Each avionics evaluation references one methodology version.
- An evaluation may contain multiple component assessments.
- Only assessments containing a score contribute to FidelityScore.
- The calculated fidelity score is expressed as a percentage.
- FidelityScore is null when no assessments have scores.
- FidelityScore is null when the combined maximum
  possible score of evaluated assessments equals zero.
- EvaluationDate and Notes are optional.
- An avionics evaluation may have multiple supporting sources.

**Calculated properties:**

`FidelityScore`

- Selects assessments where Score contains a value.
- Calculates the sum of the assigned assessment scores.
- Calculates the sum of their associated component maximum scores.
- Divides the total assigned score by the total maximum score.
- Multiplies the result by 100.
- Rounds to the nearest integer using
  MidpointRounding.AwayFromZero.
- Returns null when no scored assessments exist.
- Returns null when the combined maximum score equals zero.

**Validation:**

- No custom `Validate()` method is currently implemented.
- The class does not directly validate methodology-version
  consistency for individual component assessments.
- The class does not directly validate that
  ConfigurationAvionicsId matches the associated
  ConfigurationAvionics navigation property.
- Assessment-level validation is implemented separately
  through EvaluationComponentAssessment.

**Implementation notes:**

- FidelityScore is calculated dynamically and has no setter.
- The calculation uses only assessments with populated scores.
- Sources and Assessments are initialized to empty collections.
- ConfigurationAvionics and EvaluationMethodologyVersion
  are required navigation properties.
- The class does not store a separate calculated score.
- Evidence relationships are maintained through
  AvionicsEvaluationSource.
- Database foreign-key and relationship constraints
  have not yet been configured.

### 3.8.9 AvionicsEvaluationSource

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/AvionicsEvaluationSource.cs`

**Purpose:**

Represents the relationship between an avionics evaluation
and a supporting evidence source.

Allows FSEI to associate evidence with avionics evaluations
while identifying the role that each source plays in
supporting the evaluation.

**Properties:**

- `AvionicsEvaluationSourceId` (int) — Primary key.
- `AvionicsEvaluationId` (int) — Avionics evaluation reference.
- `SourceId` (int) — Supporting evidence source reference.
- `EvidenceRoleId` (int) — Evidence role reference.
- `Notes` (string?, optional) — Additional information
  about the evidence relationship.

**Relationships:**

- `AvionicsEvaluation` (required AvionicsEvaluation)
  — Avionics evaluation associated with the evidence.
- `Source` (required Source)
  — Supporting evidence source.
- `EvidenceRole` (required EvidenceRole)
  — Classification of the evidence relationship.

**Business rules:**

- Each record associates one avionics evaluation
  with one supporting evidence source.
- Each record references an evidence role.
- An avionics evaluation may have multiple
  supporting evidence relationships.
- A source may be associated with multiple
  avionics evaluations through separate records.
- Notes are optional.

**Validation:**

- No custom `Validate()` method is currently implemented.
- The class does not verify that foreign-key identifiers
  match their associated navigation properties.
- Required navigation properties must be initialized,
  but relationship consistency is not validated
  directly by this class.

**Implementation notes:**

- AvionicsEvaluationSource is an explicit relationship entity.
- AvionicsEvaluation, Source, and EvidenceRole
  are required navigation properties.
- Source and EvidenceRole are defined in
  the FSEI.Domain.Evidence namespace.
- Evidence relationships are maintained separately
  from evaluation component assessments.
- No uniqueness constraints are implemented in this class.
- Database foreign-key and relationship constraints
  have not yet been configured.

## 3.9 Evidence

### 3.9.1 EvidenceRole

**Namespace:** `FSEI.Domain.Evidence`

**Source:** `Evidence/EvidenceRole.cs`

**Purpose:**

Defines the role that an evidence source plays in
supporting a systems-depth or avionics evaluation.

Provides a common classification for evidence
relationships across evaluation types.

**Properties:**

- `EvidenceRoleId` (int) — Primary key.
- `Name` (required string) — Evidence role name.
- `IsActive` (bool) — Indicates whether the evidence
  role is active. Defaults to true.

**Relationships:**

- `SystemsDepthEvaluations` (ICollection<EvaluationSource>)
  — Systems-depth evaluation evidence relationships
  associated with this role.
- `AvionicsEvaluationSources` (ICollection<AvionicsEvaluationSource>)
  — Avionics evaluation evidence relationships
  associated with this role.

**Business rules:**

- Each evidence role must have a Name initialized.
- An evidence role may be associated with multiple
  systems-depth evaluation evidence relationships.
- An evidence role may be associated with multiple
  avionics evaluation evidence relationships.
- An evidence role may exist without any associated
  evaluation evidence relationships.
- New evidence roles are active by default.
- Evidence roles may be deactivated without deletion.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name is required during C# object initialization.
- Empty or whitespace-only names are not explicitly rejected.
- Name uniqueness is not currently enforced by this class.

**Implementation notes:**

- SystemsDepthEvaluations is initialized to an empty collection.
- AvionicsEvaluationSources is initialized to an empty collection.
- Both navigation collections reference relationship entities
  defined in the FSEI.Domain.Evaluations namespace.
- EvidenceRole does not directly reference
  SystemsDepthEvaluation or AvionicsEvaluation.
- Evidence relationships are maintained through
  EvaluationSource and AvionicsEvaluationSource.
- Database foreign-key and uniqueness constraints
  are not defined in this class.

### 3.9.2 EvidenceScope

**Namespace:** `FSEI.Domain.Evidence`

**Source:** `Evidence/EvidenceScope.cs`

**Purpose:**

Represents a classification used to define the
scope associated with an evidence source.

Allows evidence sources to be grouped according
to their assigned evidence scope.

**Properties:**

- `EvidenceScopeId` (int) — Primary key.
- `Name` (required string) — Evidence scope name.
- `IsActive` (bool) — Indicates whether the evidence
  scope is active. Defaults to true.

**Relationships:**

- `Sources` (ICollection<Source>)
  — Evidence sources associated with this scope.

**Business rules:**

- Each evidence scope must have a Name initialized.
- An evidence scope may be associated with multiple sources.
- An evidence scope may exist without associated sources.
- New evidence scopes are active by default.
- Evidence scopes may be deactivated without deletion.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name is required during C# object initialization.
- Empty or whitespace-only names are not explicitly rejected.
- Name uniqueness is not currently enforced by this class.

**Implementation notes:**

- Sources is initialized to an empty collection.
- EvidenceScope is referenced by Source.
- EvidenceScope does not currently define
  predefined scope codes or values.
- The class does not contain logic for determining
  which evidence scope applies to a source.
- Database foreign-key and uniqueness constraints
  are not defined in this class.

### 3.9.3 EvaluationSource

**Namespace:** `FSEI.Domain.Evaluations`

**Source:** `Evaluations/EvidenceSource.cs`

**Purpose:**

Represents the relationship between a systems-depth
evaluation and a supporting evidence source.

Allows FSEI to associate evidence with systems-depth
evaluations while identifying the role that each
source plays in supporting the evaluation.

**Properties:**

- `EvaluationSourceId` (int) — Primary key.
- `SystemsDepthEvaluationId` (int) — Systems-depth
  evaluation reference.
- `SourceId` (int) — Supporting evidence source reference.
- `EvidenceRoleId` (int) — Evidence role reference.
- `Notes` (string?, optional) — Additional information
  about the evidence relationship.

**Relationships:**

- `SystemsDepthEvaluation` (required SystemsDepthEvaluation)
  — Systems-depth evaluation associated with the evidence.
- `Source` (required Source)
  — Supporting evidence source.
- `EvidenceRole` (required EvidenceRole)
  — Classification of the evidence relationship.

**Business rules:**

- Each record associates one systems-depth evaluation
  with one supporting evidence source.
- Each record references an evidence role.
- A systems-depth evaluation may have multiple
  supporting evidence relationships.
- A source may be associated with multiple
  systems-depth evaluations through separate records.
- Notes are optional.

**Validation:**

- No custom `Validate()` method is currently implemented.
- The class does not verify that foreign-key identifiers
  match their associated navigation properties.
- Required navigation properties must be initialized,
  but relationship consistency is not validated
  directly by this class.

**Implementation notes:**

- EvaluationSource is an explicit relationship entity.
- SystemsDepthEvaluation, Source, and EvidenceRole
  are required navigation properties.
- Source and EvidenceRole are defined in
  the FSEI.Domain.Evidence namespace.
- SystemsDepthEvaluation is defined in
  the FSEI.Domain.Evaluations namespace.
- Evidence relationships are maintained separately
  from evaluation component assessments.
- No uniqueness constraints are implemented in this class.
- Database foreign-key and relationship constraints
  are not defined in this class.

### 3.9.4 Source

**Namespace:** `FSEI.Domain.Evidence`

**Source:** `Evidence/Source.cs`

**Purpose:**

Represents a supporting evidence source used within
the flight simulation ecosystem.

Provides information about the source, its type,
its evidence scope, and its optional association
with an aircraft or aircraft variant.

Supports evidence-based systems-depth and avionics
evaluations through separate relationship entities.

**Properties:**

- `SourceId` (int) — Primary key.
- `SourceTypeId` (int) — Source type reference.
- `EvidenceScopeId` (int) — Evidence scope reference.
- `AircraftId` (int?, optional) — Aircraft reference.
- `VariantId` (int?, optional) — Aircraft variant reference.
- `Name` (required string) — Evidence source name.
- `Url` (string?, optional) — Source URL.
- `DateAccessed` (DateTime?, optional) — Date the source
  was accessed.
- `Notes` (string?, optional) — Additional source information.

**Relationships:**

- `SourceType` (required SourceType)
  — Classification of the evidence source.
- `EvidenceScope` (required EvidenceScope)
  — Scope associated with the evidence source.
- `Aircraft` (Aircraft?, optional)
  — Aircraft associated with the source.
- `Variant` (AircraftVariant?, optional)
  — Aircraft variant associated with the source.
- `SystemsDepthEvaluations` (ICollection<EvaluationSource>)
  — Systems-depth evaluation evidence relationships.
- `AvionicsEvaluations` (ICollection<AvionicsEvaluationSource>)
  — Avionics evaluation evidence relationships.

**Business rules:**

- Each source must have a Name initialized.
- Each source references one source type.
- Each source references one evidence scope.
- AircraftId and VariantId are optional.
- A source may be associated with an aircraft.
- A source may be associated with an aircraft variant.
- A source may exist without an aircraft or variant reference.
- Url, DateAccessed, and Notes are optional.
- A source may support multiple systems-depth evaluations.
- A source may support multiple avionics evaluations.
- A source may exist without associated evaluations.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name is required during C# object initialization.
- Empty or whitespace-only names are not explicitly rejected.
- Url is not validated for URL format.
- DateAccessed is not validated by this class.
- The class does not enforce mutual exclusivity
  between AircraftId and VariantId.
- The class does not validate consistency between
  foreign-key identifiers and navigation properties.

**Implementation notes:**

- SourceType and EvidenceScope are required
  navigation properties.
- Aircraft and Variant navigation properties are nullable.
- SystemsDepthEvaluations is initialized
  to an empty collection.
- AvionicsEvaluations is initialized
  to an empty collection.
- SourceType and EvidenceScope are defined in
  the FSEI.Domain.Evidence namespace.
- Aircraft and AircraftVariant are defined in
  the FSEI.Domain.Aircraft namespace.
- Systems-depth evidence relationships are maintained
  through EvaluationSource.
- Avionics evidence relationships are maintained
  through AvionicsEvaluationSource.
- The class does not define uniqueness constraints.
- Database foreign-key and relationship constraints
  are not defined in this class.

### 3.9.5 SourceType

**Namespace:** `FSEI.Domain.Evidence`

**Source:** `Evidence/SourceType.cs`

**Purpose:**

Represents a classification used to identify
the type of an evidence source.

Provides a common source-type definition
that can be associated with multiple
supporting evidence sources.

**Properties:**

- `SourceTypeId` (int) — Primary key.
- `Name` (required string) — Source type name.
- `IsActive` (bool) — Indicates whether the source
  type is active. Defaults to true.

**Relationships:**

- `Sources` (ICollection<Source>)
  — Evidence sources associated with this source type.

**Business rules:**

- Each source type must have a Name initialized.
- A source type may be associated with multiple sources.
- A source type may exist without associated sources.
- New source types are active by default.
- Source types may be deactivated without deletion.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name is required during C# object initialization.
- Empty or whitespace-only names are not explicitly rejected.
- Name uniqueness is not currently enforced by this class.

**Implementation notes:**

- Sources is initialized to an empty collection.
- SourceType is referenced by Source.
- SourceType does not currently define
  predefined source-type codes or values.
- The class does not contain logic for determining
  which source type applies to an evidence source.
- Database foreign-key and uniqueness constraints
  are not defined in this class.

## 3.10 GeographicRegions

### 3.10.1 Currency

**Namespace:** `FSEI.Domain.GeographicRegions`

**Source:** `GeographicRegions/Currency.cs`

**Purpose:**

Represents a currency used within the flight
simulation ecosystem.

Provides a standardized currency identifier
for geographic and commercial information
associated with the ecosystem.

**Properties:**

- `CurrencyId` (int) — Primary key.
- `Code` (required string) — Three-character currency code.
- `Name` (required string) — Currency name.
- `Symbol` (string?, optional) — Currency symbol.
- `Notes` (string?, optional) — Additional currency information.
- `IsActive` (bool) — Indicates whether the currency
  is active. Defaults to true.

**Relationships:**

- No navigation properties are currently defined.
- Currency relationships with other domain entities
  are not established in this class.

**Business rules:**

- Each currency must have a Code initialized.
- Each currency must have a Name initialized.
- Currency codes must contain exactly three characters.
- Currency codes cannot be null, empty, or whitespace-only.
- Symbol and Notes are optional.
- New currencies are active by default.
- Currencies may be deactivated without deletion.

**Validation:**

- A custom `Validate()` method is implemented.
- Validate() rejects null, empty, or whitespace-only codes.
- Validate() rejects codes that do not contain
  exactly three characters.
- Invalid codes produce an InvalidOperationException.
- Validate() does not enforce uppercase currency codes.
- Validate() does not verify ISO 4217 membership.
- Validate() does not validate Name.
- Validate() does not enforce Code uniqueness.

**Implementation notes:**

- Code and Name use the C# required modifier.
- IsActive defaults to true.
- Validation must be invoked explicitly;
  property assignment does not automatically
  execute Validate().
- Code length validation uses the string Length property.
- No navigation collections are defined.
- No database uniqueness constraints are
  defined in this class.

### 3.10.2 Market

**Namespace:** `FSEI.Domain.GeographicRegions`

**Source:** `GeographicRegions/Market.cs`

**Purpose:**

Represents a geographic or commercial market
within the flight simulation ecosystem.

Associates a market with a geographic region
and provides identifying information used
to distinguish individual markets.

**Properties:**

- `MarketId` (int) — Primary key.
- `RegionId` (int) — Geographic region reference.
- `Code` (required string) — Market code.
- `Name` (required string) — Market name.
- `Description` (string?, optional) — Additional
  information describing the market.
- `IsActive` (bool) — Indicates whether the market
  is active. Defaults to true.

**Relationships:**

- `Region` (required Region)
  — Geographic region associated with the market.

**Business rules:**

- Each market must have a Code initialized.
- Each market must have a Name initialized.
- Each market references one geographic region.
- Market.RegionId must match Market.Region.RegionId.
- A geographic region may contain multiple markets.
- Description is optional.
- New markets are active by default.
- Markets may be deactivated without deletion.

**Validation:**

- A custom `Validate()` method is implemented.
- Validate() compares RegionId with Region.RegionId.
- A mismatch produces an InvalidOperationException.
- Validate() does not validate Code or Name.
- Empty or whitespace-only Code and Name values
  are not explicitly rejected.
- Code uniqueness is not enforced by this class.
- Validate() does not explicitly check whether
  Region is null before accessing Region.RegionId.

**Implementation notes:**

- Code, Name, and Region use the C# required modifier.
- Region is a required navigation property.
- RegionId provides the corresponding
  geographic region identifier.
- IsActive defaults to true.
- Validation must be invoked explicitly;
  property assignment does not automatically
  execute Validate().
- Region contains a Markets navigation collection
  representing the inverse relationship.
- No database foreign-key or uniqueness constraints
  are defined in this class.

### 3.10.3 Region

**Namespace:** `FSEI.Domain.GeographicRegions`

**Source:** `GeographicRegions/Region.cs`

**Purpose:**

Represents a geographic region within the
flight simulation ecosystem.

Supports hierarchical geographic organization
by allowing regions to contain other regions.

Provides a geographic structure for organizing
commercial markets associated with the ecosystem.

**Properties:**

- `RegionId` (int) — Primary key.
- `ParentRegionId` (int?, optional) — Parent region reference.
- `Code` (required string) — Geographic region code.
- `Name` (required string) — Geographic region name.
- `Description` (string?, optional) — Additional
  information describing the region.
- `IsActive` (bool) — Indicates whether the region
  is active. Defaults to true.

**Relationships:**

- `ParentRegion` (Region?, optional)
  — Parent geographic region.
- `ChildRegions` (ICollection<Region>)
  — Geographic regions belonging to this region.
- `Markets` (ICollection<Market>)
  — Commercial markets associated with this region.

**Business rules:**

- Each region must have a Code initialized.
- Each region must have a Name initialized.
- A region may reference a parent region.
- A region may exist without a parent region.
- A region may contain multiple child regions.
- A region may exist without child regions.
- A region may contain multiple markets.
- A region may exist without associated markets.
- Description is optional.
- New regions are active by default.
- Regions may be deactivated without deletion.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Code and Name are required during C# object initialization.
- Empty or whitespace-only Code and Name values
  are not explicitly rejected.
- ParentRegionId is optional.
- The class does not verify that ParentRegionId
  matches ParentRegion.RegionId.
- The class does not prevent a region from
  referencing itself as its parent.
- The class does not detect circular
  parent-child relationships.
- Region code uniqueness is not enforced
  by this class.

**Implementation notes:**

- Code and Name use the C# required modifier.
- ParentRegionId is nullable.
- ParentRegion is a nullable navigation property.
- ChildRegions is initialized to an empty collection.
- Markets is initialized to an empty collection.
- ParentRegion and ChildRegions establish a
  self-referencing geographic hierarchy.
- Markets represents the inverse relationship
  of Market.Region.
- IsActive defaults to true.
- The class does not impose a maximum
  geographic hierarchy depth.
- No database foreign-key or uniqueness
  constraints are defined in this class.

North America
└── United States
├── Southeast
└── Northeast

Europe
├── Germany
├── France
└── United Kingdom

## 3.11 Payment

### 3.11.1 PaymentMethod

**Namespace:** `FSEI.Domain.Payment`

**Source:** `Payment/PaymentMethod.cs`

**Purpose:**

Represents a payment method available within
the flight simulation ecosystem.

Provides a common classification for identifying
payment methods associated with commercial
transactions or purchasing options.

**Properties:**

- `PaymentMethodId` (int) — Primary key.
- `Code` (required string) — Payment method code.
- `Name` (required string) — Payment method name.
- `Description` (string?, optional) — Additional
  information describing the payment method.
- `IsActive` (bool) — Indicates whether the payment
  method is active. Defaults to true.

**Relationships:**

- No navigation properties are currently defined.
- Relationships with other domain entities
  are not established in this class.

**Business rules:**

- Each payment method must have a Code initialized.
- Each payment method must have a Name initialized.
- Description is optional.
- New payment methods are active by default.
- Payment methods may be deactivated without deletion.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Code and Name are required during C# object initialization.
- Empty or whitespace-only Code and Name values
  are not explicitly rejected.
- Code uniqueness is not enforced by this class.

**Implementation notes:**

- Code and Name use the C# required modifier.
- IsActive defaults to true.
- The class contains no navigation collections.
- No predefined payment method codes are
  implemented in this class.
- No database uniqueness constraints are
  defined in this class.

## 3.12 Platforms

### 3.12.1 ProcessorArchitecture

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/ProcessorArchitecture.cs`

**Purpose:**

Represents a processor architecture classification
used within the flight simulation ecosystem.

Allows FSEI to distinguish simulator compatibility
based on processor architecture.

Provides a common architecture definition that
can be associated with multiple simulator
operating-system compatibility records.

**Properties:**

- `ProcessorArchitectureId` (int) — Primary key.
- `Code` (required string) — Processor architecture code.
- `Name` (required string) — Processor architecture name.
- `Description` (string?, optional) — Additional
  information describing the processor architecture.
- `IsActive` (bool) — Indicates whether the processor
  architecture is active. Defaults to true.

**Relationships:**

- `SimulatorOperatingSystemCompatibilities`
  (ICollection<SimulatorOperatingSystemCompatibility>)
  — Simulator operating-system compatibility records
  associated with this processor architecture.

**Business rules:**

- Each processor architecture must have a Code initialized.
- Each processor architecture must have a Name initialized.
- Description is optional.
- New processor architectures are active by default.
- Processor architectures may be deactivated without deletion.
- A processor architecture may be associated with multiple
  simulator operating-system compatibility records.
- A processor architecture may exist without any associated
  compatibility records.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Code and Name are required during C# object initialization.
- Empty or whitespace-only Code and Name values
  are not explicitly rejected.
- Code uniqueness is not enforced by this class.
- No validation is implemented for Description.

**Implementation notes:**

- Code and Name use the C# required modifier.
- IsActive defaults to true.
- SimulatorOperatingSystemCompatibilities is initialized
  to an empty collection.
- The navigation collection represents the inverse
  relationship from SimulatorOperatingSystemCompatibility.
- ProcessorArchitecture is an optional reference
  within SimulatorOperatingSystemCompatibility.
- No predefined processor architecture codes
  are implemented in this class.
- No database uniqueness constraints are
  defined in this class.


### 3.12.2 CompatibilityStatus

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/CompatibilityStatus.cs`

**Purpose:**

Represents a compatibility status classification
used within the flight simulation ecosystem.

Provides a common status definition for describing
simulator compatibility with operating systems
and processor architectures.

Allows multiple simulator operating-system
compatibility records to reference the same
compatibility status.

**Properties:**

- `CompatibilityStatusId` (int) — Primary key.
- `Code` (required string) — Compatibility status code.
- `Name` (required string) — Compatibility status name.
- `Description` (string?, optional) — Additional
  information describing the compatibility status.
- `IsActive` (bool) — Indicates whether the compatibility
  status is active. Defaults to true.

**Relationships:**

- `SimulatorOperatingSystemCompatibilities`
  (ICollection<SimulatorOperatingSystemCompatibility>)
  — Simulator operating-system compatibility records
  associated with this compatibility status.

**Business rules:**

- Each compatibility status must have a Code initialized.
- Each compatibility status must have a Name initialized.
- Description is optional.
- New compatibility statuses are active by default.
- Compatibility statuses may be deactivated without deletion.
- A compatibility status may be associated with multiple
  simulator operating-system compatibility records.
- A compatibility status may exist without any associated
  compatibility records.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Code and Name are required during C# object initialization.
- Empty or whitespace-only Code and Name values
  are not explicitly rejected.
- Code uniqueness is not enforced by this class.
- No validation is implemented for Description.

**Implementation notes:**

- Code and Name use the C# required modifier.
- IsActive defaults to true.
- SimulatorOperatingSystemCompatibilities is initialized
  to an empty collection.
- The navigation collection represents the inverse
  relationship from SimulatorOperatingSystemCompatibility.
- CompatibilityStatus is a required navigation property
  within SimulatorOperatingSystemCompatibility.
- No predefined compatibility status codes
  are implemented in this class.
- No database uniqueness constraints are
  defined in this class.

### 3.12.3 SimulatorOperatingSystemCompatibility

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/SimulatorOperatingSystemCompatibility.cs`

**Purpose:**

Represents the compatibility relationship between
a flight simulator and an operating system.

Identifies the compatibility status and optionally
specifies the processor architecture associated
with the compatibility record.

Supports simulator release boundaries to identify
the releases associated with the compatibility
relationship.

**Properties:**

- `SimulatorOperatingSystemCompatibilityId` (int)
  — Primary key.
- `SimulatorId` (int) — Flight simulator reference.
- `ProcessorArchitectureId` (int?, optional)
  — Processor architecture reference.
- `SupportedFromReleaseId` (int?, optional)
  — Beginning simulator release reference.
- `SupportedThroughReleaseId` (int?, optional)
  — Ending simulator release reference.
- `OperatingSystemId` (int)
  — Operating system reference.
- `CompatibilityStatusId` (int)
  — Compatibility status reference.
- `Notes` (string?, optional)
  — Additional compatibility information.

**Relationships:**

- `SupportedFromRelease` (SimulatorRelease?, optional)
  — Simulator release associated with the beginning
  of the compatibility range.
- `SupportedThroughRelease` (SimulatorRelease?, optional)
  — Simulator release associated with the end
  of the compatibility range.
- `ProcessorArchitecture` (ProcessorArchitecture?, optional)
  — Processor architecture associated with
  the compatibility record.
- `CompatibilityStatus` (required CompatibilityStatus)
  — Classification of the compatibility relationship.
- `Simulator` (required Simulator)
  — Flight simulator associated with the record.
- `OperatingSystem` (required OperatingSystem)
  — Operating system associated with the record.

**Business rules:**

- Each compatibility record references one simulator.
- Each compatibility record references one operating system.
- Each compatibility record references one
  compatibility status.
- Processor architecture is optional.
- Supported-from and supported-through
  simulator releases are optional.
- When a supported-from release is provided,
  it must belong to the referenced simulator.
- When a supported-through release is provided,
  it must belong to the referenced simulator.
- Notes are optional.

**Validation:**

- A custom `ValidateReleaseBoundaries()` method
  is implemented.
- The method verifies that SupportedFromRelease,
  when present, belongs to the simulator identified
  by SimulatorId.
- The method verifies that SupportedThroughRelease,
  when present, belongs to the simulator identified
  by SimulatorId.
- A release belonging to another simulator produces
  an InvalidOperationException.
- The method does not verify chronological ordering
  between the beginning and ending releases.
- The method does not verify that release identifier
  properties match their navigation properties.
- The method does not validate compatibility status,
  processor architecture, or operating system references.
- No general `Validate()` method is implemented.

**Implementation notes:**

- Simulator, OperatingSystem, and CompatibilityStatus
  are required navigation properties.
- ProcessorArchitecture, SupportedFromRelease,
  and SupportedThroughRelease are nullable
  navigation properties.
- Simulator and SimulatorRelease are defined in
  the FSEI.Domain.Simulators namespace.
- ProcessorArchitecture, CompatibilityStatus,
  and OperatingSystem are defined in
  the FSEI.Domain.Platforms namespace.
- ValidateReleaseBoundaries() must be invoked explicitly.
- The class does not automatically execute validation
  when properties are assigned.
- Release boundaries are represented through
  optional foreign-key identifiers and navigation properties.
- No uniqueness constraints are implemented in this class.
- Database foreign-key and relationship constraints
  are not defined in this class.

### 3.12.4 OperatingSystem

**Namespace:** `FSEI.Domain.Platforms`

**Source:** `Platforms/OperatingSystem.cs`

**Purpose:**

Represents an operating system within the
flight simulation ecosystem.

Provides identifying information about an
operating system and its optional version.

Supports the association of operating systems
with simulator compatibility records.

**Properties:**

- `OperatingSystemId` (int) — Primary key.
- `Name` (required string) — Operating system name.
- `Version` (string?, optional) — Operating system version.
- `Notes` (string?, optional) — Additional information
  about the operating system.

**Relationships:**

- `SimulatorOperatingSystemCompatibilities`
  (ICollection<SimulatorOperatingSystemCompatibility>)
  — Simulator compatibility records associated
  with this operating system.

**Business rules:**

- Each operating system must have a Name initialized.
- Version is optional.
- Notes are optional.
- An operating system may be associated with multiple
  simulator operating-system compatibility records.
- An operating system may exist without any associated
  compatibility records.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name is required during C# object initialization.
- Empty or whitespace-only Name values
  are not explicitly rejected.
- Version format is not validated.
- Name uniqueness is not enforced by this class.

**Implementation notes:**

- Name uses the C# required modifier.
- Version and Notes are nullable strings.
- SimulatorOperatingSystemCompatibilities is initialized
  to an empty collection.
- The navigation collection represents the inverse
  relationship from SimulatorOperatingSystemCompatibility.
- OperatingSystem is a required navigation property
  within SimulatorOperatingSystemCompatibility.
- No predefined operating system names or versions
  are implemented in this class.
- No database uniqueness constraints are
  defined in this class.

## 3.13 Products

### 3.13.1 ProductType

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductType.cs`

**Purpose:**

Represents a classification used to identify
different types of products within the
flight simulation ecosystem.

Provides a common product-type definition
that can be associated with multiple products.

**Properties:**

- `ProductTypeId` (int) — Primary key.
- `Code` (required string) — Product type code.
- `Name` (required string) — Product type name.
- `Description` (string?, optional) — Additional
  information describing the product type.
- `IsActive` (bool) — Indicates whether the product
  type is active. Defaults to true.

**Relationships:**

- `Products` (ICollection<Product>)
  — Products associated with this product type.

**Business rules:**

- Each product type must have a Code initialized.
- Each product type must have a Name initialized.
- Description is optional.
- New product types are active by default.
- Product types may be deactivated without deletion.
- A product type may be associated with multiple products.
- A product type may exist without associated products.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Code and Name are required during C# object initialization.
- Empty or whitespace-only Code and Name values
  are not explicitly rejected.
- Code uniqueness is not enforced by this class.

**Implementation notes:**

- Code and Name use the C# required modifier.
- IsActive defaults to true.
- Products is initialized to an empty collection.
- The Products collection represents the inverse
  relationship from Product.ProductType.
- ProductType is a required navigation property
  within the Product class.
- No predefined product-type codes are
  implemented in this class.
- No database uniqueness constraints are
  defined in this class.

### 3.13.2 Product

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/Product.cs`

**Purpose:**

Represents an individual product within the
flight simulation ecosystem.

Associates a product with its product type,
aircraft variants, requirement groups,
and commercial offers.

**Properties:**

- `ProductId` (int) — Primary key.
- `ProductTypeId` (int) — Product type reference.
- `Name` (required string) — Product name.
- `Description` (string?, optional) — Product description.
- `Notes` (string?, optional) — Additional product information.
- `IsActive` (bool) — Indicates whether the product
  is active. Defaults to true.

**Relationships:**

- `ProductType` (required ProductType)
  — Classification associated with the product.
- `ProductVariants` (ICollection<ProductVariant>)
  — Aircraft variant associations for the product.
- `RequirementGroups` (ICollection<ProductRequirementGroup>)
  — Requirement groups associated with the product.
- `CommercialOffers` (ICollection<CommercialOffer>)
  — Commercial offers associated with the product.

**Business rules:**

- Each product must have a Name initialized.
- Each product references one product type.
- Description and Notes are optional.
- New products are active by default.
- Products may be deactivated without deletion.
- A product may be associated with multiple
  aircraft variants.
- A product may contain multiple requirement groups.
- A product may have multiple commercial offers.
- A product may exist without associated variants,
  requirement groups, or commercial offers.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name is required during C# object initialization.
- Empty or whitespace-only names are not explicitly rejected.
- The class does not verify that ProductTypeId
  matches ProductType.ProductTypeId.
- Product name uniqueness is not enforced by this class.

**Implementation notes:**

- Name and ProductType use the C# required modifier.
- IsActive defaults to true.
- ProductVariants is initialized to an empty collection.
- RequirementGroups is initialized to an empty collection.
- CommercialOffers is initialized to an empty collection.
- ProductType is defined in the Products namespace.
- CommercialOffer is defined in the Stores namespace.
- No database foreign-key or uniqueness constraints
  are defined in this class.

### 3.13.3 ProductVariant

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductVariant.cs`

**Purpose:**

Represents the relationship between a product
and an aircraft variant within the flight
simulation ecosystem.

Allows FSEI to associate products with specific
aircraft variants while maintaining the aircraft
variant information separately from the product.

**Properties:**

- `ProductVariantId` (int) — Primary key.
- `ProductId` (int) — Product reference.
- `AircraftVariantId` (int) — Aircraft variant reference.
- `Notes` (string?, optional) — Additional information
  about the product-variant relationship.

**Relationships:**

- `Product` (required Product)
  — Product associated with the aircraft variant.
- `AircraftVariant` (required AircraftVariant)
  — Aircraft variant associated with the product.

**Business rules:**

- Each ProductVariant record references one product.
- Each ProductVariant record references one aircraft variant.
- A product may be associated with multiple aircraft
  variants through separate ProductVariant records.
- Notes are optional.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Product and AircraftVariant are required during
  C# object initialization.
- The class does not verify that ProductId matches
  Product.ProductId.
- The class does not verify that AircraftVariantId
  matches AircraftVariant.AircraftVariantId.
- Duplicate product and aircraft variant combinations
  are not explicitly prevented by this class.

**Implementation notes:**

- ProductVariant is an explicit relationship entity.
- Product and AircraftVariant are required
  navigation properties.
- Product is defined in the FSEI.Domain.Products namespace.
- AircraftVariant is defined in the
  FSEI.Domain.Aircraft namespace.
- Product contains a ProductVariants navigation collection.
- No uniqueness constraints are implemented in this class.
- Database foreign-key and relationship constraints
  are not defined in this class.

### 3.13.4 ProductRequirement

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductRequirement.cs`

**Purpose:**

Represents an individual product dependency within
the flight simulation ecosystem.

Associates a required product with a product
requirement group.

Supports the definition of product prerequisites
while preventing direct self-dependencies.

**Properties:**

- `ProductRequirementId` (int) — Primary key.
- `ProductRequirementGroupId` (int)
  — Product requirement group reference.
- `RequiredProductId` (int)
  — Required product reference.
- `Notes` (string?, optional)
  — Additional information about the requirement.

**Relationships:**

- `ProductRequirementGroup` (required ProductRequirementGroup)
  — Requirement group containing this requirement.
- `RequiredProduct` (required Product)
  — Product identified as a prerequisite.

**Business rules:**

- Each product requirement belongs to one
  product requirement group.
- Each requirement identifies one required product.
- A product cannot directly require itself.
- Notes are optional.
- A requirement group may contain multiple
  product requirements.
- Multiple requirements can be associated
  with the same required product.

**Validation:**

- A custom `Validate()` method is implemented.
- Validate() compares ProductRequirementGroup.ProductId
  with RequiredProductId.
- If both identifiers match, an
  InvalidOperationException is thrown.
- Validate() prevents direct self-dependencies.
- Validate() does not detect indirect or
  circular dependencies between products.
- Validate() does not verify that foreign-key
  identifiers match their navigation properties.
- Validate() does not explicitly check whether
  ProductRequirementGroup is null.

**Implementation notes:**

- ProductRequirementGroup and RequiredProduct
  use the C# required modifier.
- ProductRequirementGroup is defined in
  the FSEI.Domain.Products namespace.
- RequiredProduct is defined in
  the FSEI.Domain.Products namespace.
- ProductRequirementGroup contains a Requirements
  navigation collection.
- Validate() must be invoked explicitly.
- ProductRequirementGroup.Validate() invokes
  Validate() on each contained requirement.
- No uniqueness constraints are implemented
  in this class.
- Database foreign-key and relationship
  constraints are not defined in this class.

### 3.13.5 ProductRequirementGroup

**Namespace:** `FSEI.Domain.Products`

**Source:** `Products/ProductRequirementGroup.cs`

**Purpose:**

Represents a group of product prerequisites within
the flight simulation ecosystem.

Allows a product to define a collection of
acceptable prerequisite products.

Provides functionality for determining whether
an owned product satisfies the requirement group.

**Properties:**

- `ProductRequirementGroupId` (int) — Primary key.
- `ProductId` (int) — Product reference.
- `Description` (string?, optional) — Additional
  information describing the requirement group.

**Relationships:**

- `Product` (required Product)
  — Product associated with the requirement group.
- `Requirements` (ICollection<ProductRequirement>)
  — Individual product requirements belonging
  to this requirement group.

**Business rules:**

- Each requirement group belongs to one product.
- A requirement group must contain at least
  one product requirement to pass validation.
- A requirement group may contain multiple
  product requirements.
- A requirement group is satisfied when at least
  one required product is present in the supplied
  collection of owned product identifiers.
- A requirement group is not satisfied when none
  of its required products are owned.
- Description is optional.
- Each contained requirement must pass
  its own validation rules.

**Methods:**

- `IsSatisfiedBy(IEnumerable<int> ownedProductIds)`
  — Returns true when at least one requirement
  references a product contained in ownedProductIds.
  Returns false otherwise.

- `Validate()`
  — Verifies that the requirement group contains
  at least one requirement and invokes Validate()
  on each contained ProductRequirement.

**Validation:**

- A custom `Validate()` method is implemented.
- Validate() rejects requirement groups
  containing zero requirements.
- An empty requirement group produces an
  InvalidOperationException.
- Validate() invokes ProductRequirement.Validate()
  for each requirement.
- Individual requirement validation prevents
  direct product self-dependencies.
- Validate() does not detect indirect
  circular dependencies between products.
- Validate() does not verify that ProductId
  matches Product.ProductId.
- Validate() must be invoked explicitly.

**Implementation notes:**

- Product uses the C# required modifier.
- Requirements is initialized to an empty collection.
- IsSatisfiedBy() uses LINQ Any().
- Requirement satisfaction uses OR logic:
  ownership of any one listed required product
  satisfies the group.
- IsSatisfiedBy() returns false when Requirements
  is empty.
- IsSatisfiedBy() does not invoke Validate().
- The class does not automatically execute validation
  when requirements are added or removed.
- No database foreign-key or uniqueness constraints
  are defined in this class.

## 3.14 Simulators

### 3.14.1 Simulator

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/Simulator.cs`

**Purpose:**

Represents an individual flight simulator within
the flight simulation ecosystem.

Associates a simulator with its simulator family
and current status.

Maintains relationships with simulator releases
and operating-system compatibility records.

**Properties:**

- `SimulatorId` (int) — Primary key.
- `SimulatorFamilyId` (int) — Simulator family reference.
- `SimulatorStatusId` (int) — Simulator status reference.
- `Name` (required string) — Simulator name.
- `Version` (string?, optional) — Simulator version.
- `Notes` (string?, optional) — Additional
  information about the simulator.

**Relationships:**

- `SimulatorStatus` (required SimulatorStatus)
  — Status associated with the simulator.
- `SimulatorFamily` (required SimulatorFamily)
  — Family associated with the simulator.
- `Releases` (ICollection<SimulatorRelease>)
  — Individual releases associated with the simulator.
- `SimulatorOperatingSystemCompatibilities`
  (ICollection<SimulatorOperatingSystemCompatibility>)
  — Operating-system compatibility records
  associated with the simulator.

**Business rules:**

- Each simulator must have a Name initialized.
- Each simulator references one simulator family.
- Each simulator references one simulator status.
- Version and Notes are optional.
- A simulator may have multiple releases.
- A simulator may exist without associated releases.
- A simulator may have multiple operating-system
  compatibility records.
- A simulator may exist without associated
  compatibility records.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name, SimulatorStatus, and SimulatorFamily
  are required during C# object initialization.
- Empty or whitespace-only names are not
  explicitly rejected.
- The class does not verify that SimulatorFamilyId
  matches SimulatorFamily.SimulatorFamilyId.
- The class does not verify that SimulatorStatusId
  matches SimulatorStatus.SimulatorStatusId.
- Simulator name uniqueness is not enforced
  by this class.

**Implementation notes:**

- Name, SimulatorStatus, and SimulatorFamily
  use the C# required modifier.
- Releases is initialized to an empty collection.
- SimulatorOperatingSystemCompatibilities
  is initialized to an empty collection.
- SimulatorStatus and SimulatorFamily are defined
  in the FSEI.Domain.Simulators namespace.
- SimulatorOperatingSystemCompatibility is defined
  in the FSEI.Domain.Platforms namespace.
- SimulatorRelease contains a required Simulator
  navigation property.
- SimulatorOperatingSystemCompatibility contains
  a required Simulator navigation property.
- No database foreign-key or uniqueness constraints
  are defined in this class.

### 3.14.2 SimulatorFamily

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/SimulatorFamily.cs`

**Purpose:**

Represents a family of flight simulators within
the flight simulation ecosystem.

Provides a common grouping for related simulators
while allowing individual simulators and their
releases to be maintained separately.

**Properties:**

- `SimulatorFamilyId` (int) — Primary key.
- `Name` (required string) — Simulator family name.
- `Notes` (string?, optional) — Additional information
  about the simulator family.

**Relationships:**

- `Simulators` (ICollection<Simulator>)
  — Simulators associated with this simulator family.

**Business rules:**

- Each simulator family must have a Name initialized.
- Notes are optional.
- A simulator family may contain multiple simulators.
- A simulator family may exist without associated simulators.
- Each Simulator references one SimulatorFamily.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Name is required during C# object initialization.
- Empty or whitespace-only names are not explicitly rejected.
- Simulator family name uniqueness is not enforced
  by this class.
- No validation is implemented for Notes.

**Implementation notes:**

- Name uses the C# required modifier.
- Notes is a nullable string.
- Simulators is initialized to an empty collection.
- The Simulators collection represents the inverse
  relationship from Simulator.SimulatorFamily.
- Simulator contains a required SimulatorFamily
  navigation property.
- SimulatorFamily does not define an IsActive property.
- No predefined simulator families are implemented
  in this class.
- No database uniqueness constraints are
  defined in this class.

### 3.14.3 SimulatorStatus

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/SimulatorStatus.cs`

**Purpose:**

Represents a status classification for flight
simulators within the flight simulation ecosystem.

Provides a common status definition that can
be associated with multiple simulators.

Allows simulator status information to be
maintained separately from individual simulators.

**Properties:**

- `SimulatorStatusId` (int) — Primary key.
- `Code` (required string) — Simulator status code.
- `Name` (required string) — Simulator status name.
- `Description` (string?, optional) — Additional
  information describing the simulator status.
- `IsActive` (bool) — Indicates whether the simulator
  status is active. Defaults to true.

**Relationships:**

- `Simulators` (ICollection<Simulator>)
  — Simulators associated with this status.

**Business rules:**

- Each simulator status must have a Code initialized.
- Each simulator status must have a Name initialized.
- Description is optional.
- New simulator statuses are active by default.
- Simulator statuses may be deactivated without deletion.
- A simulator status may be associated with multiple
  simulators.
- A simulator status may exist without associated
  simulators.
- Each Simulator references one SimulatorStatus.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Code and Name are required during C# object initialization.
- Empty or whitespace-only Code and Name values
  are not explicitly rejected.
- Code uniqueness is not enforced by this class.
- No validation is implemented for Description.

**Implementation notes:**

- Code and Name use the C# required modifier.
- IsActive defaults to true.
- Simulators is initialized to an empty collection.
- The Simulators collection represents the inverse
  relationship from Simulator.SimulatorStatus.
- Simulator contains a required SimulatorStatus
  navigation property.
- No predefined simulator status codes are
  implemented in this class.
- No database uniqueness constraints are
  defined in this class.

### 3.14.4 SimulatorRelease

**Namespace:** `FSEI.Domain.Simulators`

**Source:** `Simulators/SimulatorRelease.cs`

**Purpose:**

Represents an individual release of a flight
simulator within the flight simulation ecosystem.

Allows FSEI to maintain release-specific information
independently from the parent simulator.

Supports tracking simulator versions, release dates,
release channels, and additional release notes.

**Properties:**

- `SimulatorReleaseId` (int) — Primary key.
- `SimulatorId` (int) — Parent simulator reference.
- `Version` (required string) — Simulator release version.
- `ReleaseDate` (DateTime?, optional) — Date of release.
- `Notes` (string?, optional) — Additional release information.
- `ReleaseChannel` (string?, optional) — Release channel
  associated with the simulator release.

**Relationships:**

- `Simulator` (required Simulator)
  — Parent simulator associated with this release.

**Business rules:**

- Each simulator release must have a Version initialized.
- Each simulator release references one parent simulator.
- ReleaseDate is optional.
- Notes are optional.
- ReleaseChannel is optional.
- A simulator may have multiple releases.
- Multiple releases may share the same release channel.
- A simulator may exist without associated releases.

**Validation:**

- No custom `Validate()` method is currently implemented.
- Version is required during C# object initialization.
- Empty or whitespace-only version strings are not
  explicitly rejected.
- ReleaseDate is not validated against the current date.
- ReleaseChannel does not have predefined allowed values.
- The class does not verify that SimulatorId matches
  Simulator.SimulatorId.
- Release version uniqueness is not enforced
  by this class.

**Implementation notes:**

- Version and Simulator use the C# required modifier.
- ReleaseDate uses nullable DateTime.
- Notes and ReleaseChannel use nullable strings.
- Simulator contains a Releases navigation collection.
- The Releases collection is initialized to empty
  in the Simulator class.
- No release-channel enumeration is defined
  in this class.
- No predefined simulator releases are implemented.
- No database foreign-key or uniqueness constraints
  are defined in this class.

X-Plane 12
│
├── Release 12.1
│     └── Release Channel: Stable
│
├── Release 12.2
│     └── Release Channel: Stable
│
└── Release 12.3 Beta
└── Release Channel: Beta


