# OxyPlot .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the OxyPlot solution upgrade from .NET Standard 2.0/.NET 6/.NET 8 to add .NET 10.0 support. All 7 projects will be upgraded simultaneously in a single atomic operation, followed by testing and validation.

**Progress**: 1/3 tasks complete (33%) ![0%](https://progress-bar.xyz/33)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-04-09 12:44)*
**References**: Plan §Phase 1 Pre-Migration Validation

- [✓] (1) Verify .NET 10.0 SDK installed per Plan §Critical Dependencies
- [✓] (2) .NET 10.0 SDK version meets minimum requirements (**Verify**)

---

### [▶] TASK-002: Atomic framework and dependency upgrade with compilation fixes
**References**: Plan §Phase 2 Project File Updates, Plan §Phase 3 API Compatibility Resolution, Plan §Detailed Dependency Analysis, Plan §Package Compatibility Analysis

- [ ] (1) Update TargetFramework/TargetFrameworks in all 7 projects per Plan §Migration Order (add net10.0 or net10.0-windows to existing targets)
- [ ] (2) All project files updated to include .NET 10.0 target (**Verify**)
- [ ] (3) Remove System.Memory package reference from ExampleLibrary per Plan §Package Compatibility Analysis
- [ ] (4) System.Memory package removed (**Verify**)
- [ ] (5) Restore all dependencies across solution
- [ ] (6) All dependencies restored successfully (**Verify**)
- [ ] (7) Build solution and fix all compilation errors per Plan §API Compatibility Resolution (focus: 56 Api.0002 source incompatibilities in OxyPlot and ExampleLibrary; reference Plan §Project-by-Project Plans for specific APIs)
- [ ] (8) Solution builds with 0 errors for all target frameworks (**Verify**)
- [ ] (9) Commit changes with message: "TASK-002: Add .NET 10.0 support - update project files, packages, and fix API incompatibilities"

---

### [ ] TASK-003: Run full test suite and validate upgrade
**References**: Plan §Phase 4 Build & Test Validation, Plan §Testing & Validation Strategy

- [ ] (1) Run tests in OxyPlot.Tests and OxyPlot.SkiaSharp.Tests projects for both net8.0 and net10.0 targets
- [ ] (2) Fix any test failures (reference Plan §Detailed Dependency Analysis for WPF binary incompatibility considerations)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures on both net8.0 and net10.0 (**Verify**)
- [ ] (5) Commit test fixes with message: "TASK-003: Complete .NET 10.0 upgrade testing and validation"

---


