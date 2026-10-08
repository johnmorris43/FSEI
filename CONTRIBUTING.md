# Contributing to FSEI

Thank you for your interest in contributing to the Flight Simulator Ecosystem Index (FSEI).

FSEI is an open-source project designed to provide structured, evidence-backed analysis of the flight simulation ecosystem. Contributions from flight simulation users, software developers, aircraft developers, scenery developers, researchers, and other members of the community are welcome.

## Project Principles

FSEI is built around several core principles:

- Evidence should support factual claims and evaluations.
- Evaluation methodology should be transparent and reproducible.
- Historical information should be preserved whenever practical.
- Scores should be explainable rather than presented as unexplained numbers.
- Business rules belong in the domain model rather than only in the user interface.
- Developer and community feedback is encouraged.
- FSEI maintains independent control of its evaluation methodology and results.

## Ways to Contribute

Contributions may include:

- Bug reports
- Feature suggestions
- Documentation improvements
- Factual product corrections
- Simulator compatibility information
- Aircraft, avionics, or product information
- Pricing and package information
- Evidence and source material
- Test improvements
- Code contributions
- Domain-model suggestions
- Evaluation challenges supported by evidence

When possible, significant changes should begin with a GitHub Issue so the proposed change can be discussed before implementation.

## Evidence and Sources

FSEI distinguishes between factual product information, supporting evidence, and FSEI evaluation results.

When contributing factual information or requesting a correction, provide the strongest available source.

Preferred sources generally include:

1. Official developer documentation
2. Aircraft or product manuals
3. Official developer changelogs
4. Official product or store information
5. Direct testing
6. Independent technical reviews
7. Community reports

Community information can identify issues worth investigating, but important factual or scoring changes should be verified whenever possible.

Sources should include enough information for another contributor to locate and review the evidence.

## Developer Contributions

Flight simulation developers are encouraged to contribute factual information about their own products.

Examples include:

- Supported simulator versions
- Operating-system compatibility
- Product package relationships
- Upgrade requirements
- Included aircraft variants
- Avionics configurations
- Release information
- Pricing corrections
- Documentation
- Technical corrections

Developer participation does not grant control over FSEI evaluation results, scoring methodology, or comparative conclusions.

Corrections to factual information are welcomed. Requests to change an evaluation or score must be supported by evidence and reviewed using the same FSEI methodology applied to other products.

## Evaluation and Scoring Changes

Evaluation scores should never be changed solely because a contributor disagrees with a result.

A proposed evaluation change should identify:

- The evaluation being challenged
- The component or score in question
- The supporting evidence
- Why the existing evaluation may no longer accurately represent the product
- Relevant product or simulator version information

When an evaluation methodology changes, FSEI should preserve the previous methodology whenever practical rather than silently rewriting historical results.

## Code Contributions

Before submitting code:

- Keep changes focused on the issue being addressed.
- Follow the existing project structure and naming conventions.
- Keep business rules in the appropriate domain/application layer.
- Avoid introducing unnecessary dependencies.
- Avoid premature abstractions.
- Add or update tests when changing business behavior.
- Ensure the solution builds successfully.
- Ensure all automated tests pass.

Do not weaken existing validation merely to make a test pass.

## Building and Testing FSEI

### Prerequisites

- .NET 10 SDK
- JetBrains Rider or another compatible .NET development environment

### Build the Solution

From the repository root:

```bash
dotnet build FSEI.sln
```

### Run Unit Tests

FSEI uses NUnit for domain unit testing.

Run the complete test suite:

```bash
dotnet test FSEI.sln
```

### Running Tests in Rider

1. Open FSEI.sln.
2. Build the solution.
3. Open the Unit Tests tool window.
4. Run all tests in FSEI.Domain.Tests.
5. Verify that every test passes.

### Development Requirements

Before committing changes:

- The solution must build successfully.
- All existing tests must pass.
- New business rules should include appropriate unit tests.
- Existing tests must not be disabled to bypass failures.

The verified development baseline is 78 passing unit tests as of October 8, 2026.

## Pull Requests

Pull requests should clearly describe:

- What changed
- Why the change is necessary
- Which issue it addresses, when applicable
- Any domain or architectural decisions involved
- Evidence supporting data or evaluation changes
- Tests added or modified

Large architectural changes should normally be discussed before implementation.

Maintainers may request changes before a pull request is merged.

## Domain Model Changes

FSEI's domain model represents real relationships within the flight simulation ecosystem.

Before introducing a new entity or relationship, contributors should consider whether the concept:

- Represents a real domain concept
- Already exists elsewhere in the model
- Requires historical tracking
- Requires supporting evidence
- Represents a many-to-many relationship
- Contains business rules that should be validated
- Could affect existing evaluations or historical data

The goal is not to reproduce spreadsheet structure or external store layouts directly. The goal is to represent the underlying domain accurately.

## Historical Data

Historical information is valuable to FSEI.

Whenever practical, updates should preserve previous information rather than overwrite it.

Examples include:

- Simulator releases
- Compatibility changes
- Product pricing
- Product availability
- Evaluation methodology versions
- Evaluation results
- Ecosystem metrics

This allows FSEI to describe not only the current ecosystem but how that ecosystem has changed over time.

## Security

Do not submit:

- Passwords
- API keys
- Access tokens
- Private credentials
- Personally identifiable information
- Proprietary material that you do not have permission to distribute

Security vulnerabilities should not be publicly exploited or demonstrated against production FSEI systems.

## Code of Conduct

Contributors are expected to communicate professionally and respectfully.

Technical disagreement is welcome. Personal attacks, harassment, or attempts to manipulate FSEI evaluations are not.

The goal is to improve the accuracy and usefulness of the project.

## Questions

If you are uncertain whether a proposed contribution fits the project, open a GitHub Issue and describe the idea before investing significant development time.

FSEI is being built in the open, and constructive technical discussion is encouraged.