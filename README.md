# Flight Simulator Ecosystem Index (FSEI)

The Flight Simulator Ecosystem Index (FSEI) is a structured, evidence-based
evaluation project for flight simulation aircraft, systems, developers, and
the broader simulator ecosystem.

FSEI is not intended to be a traditional aircraft review or a measure of
developer reputation. Evaluations are based on observed simulator behavior,
direct testing, official developer documentation, and clearly identified
supporting evidence.

## Project Goals

FSEI is being developed to:

- Evaluate aircraft and avionics using consistent, repeatable criteria.
- Separate systems fidelity from operational immersion.
- Document what has actually been tested rather than assuming functionality.
- Identify functionality that remains untested or requires additional evidence.
- Track aircraft and simulator changes across software versions.
- Allow aircraft to be reevaluated as products evolve.
- Provide flight-simulation users with transparent evidence behind evaluation results.

## Evaluation Philosophy

FSEI follows four core principles:

**Transparency** — Evaluation results should show what evidence supports them.

**Consistency** — Similar systems should be evaluated using repeatable methods while
allowing criteria to account for differences between aircraft and avionics architectures.

**Data Over Hype** — Developer reputation, marketing claims, and popularity do not
determine evaluation results.

**Continuous Evaluation** — Aircraft, simulators, and add-ons change. FSEI evaluations
can therefore change when new versions, functionality, documentation, or test evidence
become available.

## Evidence

FSEI distinguishes between different levels of evidence, including:

1. Documented functionality
2. Directly observed functionality
3. Stress-tested functionality
4. Failure-verified functionality

Official technical documentation and direct simulator testing form the primary
evidence base. Third-party tools and flight logs may provide supporting evidence
but do not replace direct testing.

## Current Status

FSEI Version 1 is currently a prototype.

The Microsoft Excel workbook contained in this repository is being used to develop
and test the database structure, evaluation methodology, scoring architecture,
evidence relationships, and reporting concepts before development of the final
FSEI application.

The prototype should not be considered the final database or scoring architecture.
Lessons learned during aircraft evaluations are being used to refine the production
design.

## Initial Aircraft Evaluation

The ToLiss Airbus A321 for X-Plane 12 is being used as the first complete FSEI
aircraft evaluation.

The evaluation includes direct operational testing and official developer
documentation covering aircraft systems, avionics, flight-management functions,
autoflight, displays, weather radar, terrain awareness, communications, EFB
functionality, navigation, and abnormal/failure behavior.

Some areas may remain marked **Pending** until sufficient evidence has been obtained.
FSEI does not assume untested functionality.
