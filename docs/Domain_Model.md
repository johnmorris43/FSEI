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


