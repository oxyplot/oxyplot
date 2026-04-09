# OxyPlot Solution - .NET 10.0 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Overview
This plan outlines the migration of the OxyPlot solution from multi-targeting **.NET Standard 2.0, .NET Framework 4.6.2, .NET 6, and .NET 8** to add **.NET 10.0 (LTS)** support while maintaining backward compatibility with existing targets.

### Scope
- **7 projects** in the OxyPlot.SkiaSharp solution
- **Target Framework**: Add `net10.0` to existing multi-target configurations
- **Migration Type**: Multi-target expansion (non-breaking)
- **Source Branch**: `develop`
- **Target Branch**: `upgrade-to-NET10`

### Key Metrics
- **Total Issues Identified**: 1,411
  - **Project.0002** (TFM changes): 7 instances
  - **Api.0001** (Binary incompatible): 1,347 instances (WPF-related)
  - **Api.0002** (Source incompatible): 56 instances
  - **NuGet.0003** (Package consolidation): 1 instance
- **Estimated Complexity**: Medium
- **Estimated Effort**: 4-8 hours (depending on API remediation complexity)

### Strategy Overview
**All-at-Once Coordinated Update**: All 7 projects will be updated in dependency order in a single comprehensive change. This approach is optimal because:
- All projects are SDK-style with clean project files
- The solution has a clear dependency structure
- Most issues are related to WPF binary incompatibilities (auto-handled)
- No circular dependencies detected

### Critical Dependencies
- **.NET 10.0 SDK** must be installed
- **SkiaSharp 3.116.1** is compatible with .NET 10
- **NUnit packages** are compatible with .NET 10
- **WPF APIs**: Some binary incompatibilities require runtime testing

---

## Migration Strategy

### Selected Approach: All-at-Once Coordinated Update

#### Rationale
1. **Project Maturity**: All projects use SDK-style format, simplifying TFM updates
2. **Dependency Clarity**: Well-defined dependency graph with no circular dependencies
3. **Package Compatibility**: All NuGet packages are compatible with .NET 10.0
4. **Limited Code Changes**: Most issues are WPF binary incompatibilities that resolve at runtime

#### Migration Phases

**Phase 1: Pre-Migration Validation** (15 minutes)
- Verify .NET 10.0 SDK installation
- Create and switch to `upgrade-to-NET10` branch
- Backup current state
- Run baseline tests on existing frameworks

**Phase 2: Project File Updates** (30 minutes)
- Update all 7 projects in dependency order:
  1. OxyPlot (root library)
  2. ExampleLibrary
  3. OxyPlot.SkiaSharp
  4. OxyPlot.SkiaSharp.Wpf
  5. OxyPlot.Wpf.Shared
  6. OxyPlot.Tests
  7. OxyPlot.SkiaSharp.Tests
- Add `net10.0` to `<TargetFrameworks>` for each project
- Remove redundant NuGet package (System.Memory) from ExampleLibrary

**Phase 3: API Compatibility Resolution** (2-4 hours)
- Address 56 source incompatibilities in OxyPlot and ExampleLibrary
- Test WPF binary incompatibilities (1,347 instances) for runtime behavior
- Apply conditional compilation if needed (#if NET10_0_OR_GREATER)

**Phase 4: Build & Test Validation** (1-2 hours)
- Build solution for all target frameworks
- Run test suites (OxyPlot.Tests, OxyPlot.SkiaSharp.Tests)
- Validate WPF projects load and render correctly
- Performance benchmarking (if applicable)

**Phase 5: Final Review & Commit** (30 minutes)
- Review all changes
- Commit to `upgrade-to-NET10` branch
- Prepare PR for review

### Rollback Strategy
If critical issues arise:
1. **Immediate**: `git checkout develop` to return to original state
2. **Partial**: Remove `net10.0` from `<TargetFrameworks>` while keeping code fixes
3. **Incremental**: Use conditional compilation to disable .NET 10-specific code paths

### Multi-Targeting Considerations
Since this is additive multi-targeting:
- **Existing targets remain unchanged** (netstandard2.0, net462, net6.0, net8.0)
- **Consumer compatibility**: Projects consuming OxyPlot can choose their preferred target
- **Testing burden**: All target frameworks should be validated

---

## Detailed Dependency Analysis

### Dependency Graph (Topological Order)

```
Level 0 (Root):
  └─ OxyPlot (netstandard2.0) 
      └─ No dependencies

Level 1 (Direct Consumers):
  ├─ ExampleLibrary (netstandard2.0)
  │   └─ Depends on: OxyPlot
  │   └─ NuGet: System.Memory 4.5.5 [TO BE REMOVED - included in .NET 10]
  │
  ├─ OxyPlot.SkiaSharp (netstandard2.0)
  │   └─ Depends on: OxyPlot
  │   └─ NuGet: SkiaSharp 3.116.1 [Compatible with .NET 10]
  │
  └─ OxyPlot.Wpf.Shared (net462, net6.0-windows, net8.0-windows)
      └─ Depends on: OxyPlot

Level 2 (Second-Order Consumers):
  ├─ OxyPlot.SkiaSharp.Wpf (net462, net6.0-windows, net8.0-windows)
  │   └─ Depends on: OxyPlot, OxyPlot.SkiaSharp
  │   └─ NuGet: SkiaSharp 3.116.1, SkiaSharp.Views.Desktop.Common 3.116.1
  │
  ├─ OxyPlot.Tests (net8.0)
  │   └─ Depends on: OxyPlot, ExampleLibrary
  │   └─ NuGet: NUnit 4.2.2, NUnit3TestAdapter 4.6.0, Microsoft.NET.Test.Sdk 17.11.1
  │
  └─ OxyPlot.SkiaSharp.Tests (net8.0)
      └─ Depends on: OxyPlot, OxyPlot.SkiaSharp, ExampleLibrary
      └─ NuGet: SkiaSharp 3.116.1, NUnit 4.2.2, NUnit3TestAdapter 4.6.0, Microsoft.NET.Test.Sdk 17.11.1
```

### Migration Order (Bottom-Up)

**Order 1: Core Library**
- `OxyPlot.csproj`
  - Current: `<TargetFramework>netstandard2.0</TargetFramework>`
  - Proposed: `<TargetFrameworks>netstandard2.0;net10.0</TargetFrameworks>`
  - Rationale: Root dependency, no external dependencies

**Order 2: First-Level Dependencies**
- `ExampleLibrary.csproj`
  - Current: `<TargetFramework>netstandard2.0</TargetFramework>`
  - Proposed: `<TargetFrameworks>netstandard2.0;net10.0</TargetFrameworks>`
  - Package removal: System.Memory (NuGet.0003)

- `OxyPlot.SkiaSharp.csproj`
  - Current: `<TargetFramework>netstandard2.0</TargetFramework>`
  - Proposed: `<TargetFrameworks>netstandard2.0;net10.0</TargetFrameworks>`

- `OxyPlot.Wpf.Shared.csproj`
  - Current: `<TargetFrameworks>net462;net6.0-windows;net8.0-windows</TargetFrameworks>`
  - Proposed: `<TargetFrameworks>net462;net6.0-windows;net8.0-windows;net10.0-windows</TargetFrameworks>`
  - Note: Binary incompatibilities (Api.0001) require testing

**Order 3: Second-Level Dependencies**
- `OxyPlot.SkiaSharp.Wpf.csproj`
  - Current: `<TargetFrameworks>net462;net6.0-windows;net8.0-windows</TargetFrameworks>`
  - Proposed: `<TargetFrameworks>net462;net6.0-windows;net8.0-windows;net10.0-windows</TargetFrameworks>`
  - Note: Binary incompatibilities (Api.0001) require testing

- `OxyPlot.Tests.csproj`
  - Current: `<TargetFramework>net8.0</TargetFramework>`
  - Proposed: `<TargetFrameworks>net8.0;net10.0</TargetFrameworks>`

- `OxyPlot.SkiaSharp.Tests.csproj`
  - Current: `<TargetFramework>net8.0</TargetFramework>`
  - Proposed: `<TargetFrameworks>net8.0;net10.0</TargetFrameworks>`

### Package Compatibility Analysis

| Package | Current Version | .NET 10 Compatible | Action Required |
|---------|----------------|-------------------|-----------------|
| SkiaSharp | 3.116.1 | ✅ Yes | None |
| SkiaSharp.Views.Desktop.Common | 3.116.1 | ✅ Yes | None |
| NUnit | 4.2.2 | ✅ Yes | None |
| NUnit3TestAdapter | 4.6.0 | ✅ Yes | None |
| Microsoft.NET.Test.Sdk | 17.11.1 | ✅ Yes | None |
| System.Memory | 4.5.5 | ⚠️ Redundant | **Remove** (included in .NET 10) |

---

## Project-by-Project Plans

### 1. OxyPlot (Core Library)

**Current State:**
- Target Framework: `netstandard2.0`
- Dependencies: None
- Issues: 49 instances of Api.0002 (source incompatibilities)

**Migration Plan:**
1. **Update Target Framework**
   ```xml
   <!-- Before -->
   <TargetFramework>netstandard2.0</TargetFramework>

   <!-- After -->
   <TargetFrameworks>netstandard2.0;net10.0</TargetFrameworks>
   ```

2. **Address API Incompatibilities**
   - **Issue**: Api.0002 - 49 source incompatibilities detected
   - **Location**: Various files (requires detailed analysis per file)
   - **Strategy**: 
     - Review assessment.md for specific file/line details
     - Apply conditional compilation where needed
     - Use modern .NET 10 APIs when targeting net10.0
     - Maintain netstandard2.0 compatibility

3. **Validation**
   - Build for both `netstandard2.0` and `net10.0`
   - Verify NuGet package generation includes both targets
   - Run OxyPlot.Tests against both frameworks

**Complexity**: Medium (API incompatibilities require careful review)
**Estimated Time**: 2-3 hours

---

### 2. ExampleLibrary

**Current State:**
- Target Framework: `netstandard2.0`
- Dependencies: OxyPlot
- NuGet Packages: System.Memory 4.5.5
- Issues: 7 instances of Api.0002, 1 instance of NuGet.0003

**Migration Plan:**
1. **Update Target Framework**
   ```xml
   <!-- Before -->
   <TargetFramework>netstandard2.0</TargetFramework>

   <!-- After -->
   <TargetFrameworks>netstandard2.0;net10.0</TargetFrameworks>
   ```

2. **Remove Redundant Package**
   ```xml
   <!-- Remove this package reference -->
   <PackageReference Include="System.Memory" Version="4.5.5" />
   ```
   - **Reason**: System.Memory is part of .NET 10.0 runtime
   - **Impact**: No code changes needed; package functionality is built-in

3. **Address API Incompatibilities**
   - **Issue**: Api.0002 - 7 source incompatibilities detected
   - **Strategy**: Similar to OxyPlot, use conditional compilation if needed

4. **Validation**
   - Build for both targets
   - Verify Memory<T>, Span<T> APIs work without System.Memory package
   - Ensure examples compile and run

**Complexity**: Low-Medium
**Estimated Time**: 30-45 minutes

---

### 3. OxyPlot.SkiaSharp

**Current State:**
- Target Framework: `netstandard2.0`
- Dependencies: OxyPlot, SkiaSharp 3.116.1
- Issues: Project.0002 only

**Migration Plan:**
1. **Update Target Framework**
   ```xml
   <!-- Before -->
   <TargetFramework>netstandard2.0</TargetFramework>

   <!-- After -->
   <TargetFrameworks>netstandard2.0;net10.0</TargetFrameworks>
   ```

2. **Verify SkiaSharp Compatibility**
   - SkiaSharp 3.116.1 is compatible with .NET 10
   - No version change required

3. **Validation**
   - Build for both targets
   - Run OxyPlot.SkiaSharp.Tests
   - Verify rendering functionality

**Complexity**: Low (clean migration, no API issues)
**Estimated Time**: 15 minutes

---

### 4. OxyPlot.Wpf.Shared

**Current State:**
- Target Frameworks: `net462;net6.0-windows;net8.0-windows`
- Dependencies: OxyPlot
- Issues: 674 instances of Api.0001 (binary incompatibilities)

**Migration Plan:**
1. **Update Target Framework**
   ```xml
   <!-- Before -->
   <TargetFrameworks>net462;net6.0-windows;net8.0-windows</TargetFrameworks>

   <!-- After -->
   <TargetFrameworks>net462;net6.0-windows;net8.0-windows;net10.0-windows</TargetFrameworks>
   ```

2. **Address Binary Incompatibilities**
   - **Issue**: Api.0001 - 674 WPF API binary incompatibilities
   - **Nature**: These are typically method signature changes or obsolete overloads
   - **Strategy**: 
     - Most binary incompatibilities auto-resolve at runtime
     - Focus on compilation errors first
     - Runtime test WPF controls and rendering
     - Monitor for behavioral differences

3. **Testing Focus Areas**
   - WPF control rendering
   - Event handling
   - Data binding
   - Visual tree traversal

4. **Validation**
   - Build for all target frameworks
   - Manual WPF UI testing
   - Verify no runtime exceptions

**Complexity**: Medium-High (significant binary incompatibilities require testing)
**Estimated Time**: 1-2 hours

---

### 5. OxyPlot.SkiaSharp.Wpf

**Current State:**
- Target Frameworks: `net462;net6.0-windows;net8.0-windows`
- Dependencies: OxyPlot, OxyPlot.SkiaSharp, SkiaSharp packages
- Issues: 673 instances of Api.0001 (binary incompatibilities)

**Migration Plan:**
1. **Update Target Framework**
   ```xml
   <!-- Before -->
   <TargetFrameworks>net462;net6.0-windows;net8.0-windows</TargetFrameworks>

   <!-- After -->
   <TargetFrameworks>net462;net6.0-windows;net8.0-windows;net10.0-windows</TargetFrameworks>
   ```

2. **Address Binary Incompatibilities**
   - Similar strategy to OxyPlot.Wpf.Shared
   - 673 WPF API issues to validate at runtime

3. **Validation**
   - Build for all targets
   - Test SkiaSharp rendering in WPF context
   - Verify interop between WPF and SkiaSharp

**Complexity**: Medium-High
**Estimated Time**: 1-2 hours

---

### 6. OxyPlot.Tests

**Current State:**
- Target Framework: `net8.0`
- Dependencies: OxyPlot, ExampleLibrary, NUnit packages
- Issues: Project.0002 only

**Migration Plan:**
1. **Update Target Framework**
   ```xml
   <!-- Before -->
   <TargetFramework>net8.0</TargetFramework>

   <!-- After -->
   <TargetFrameworks>net8.0;net10.0</TargetFrameworks>
   ```

2. **Validation**
   - Run full test suite on both net8.0 and net10.0
   - Compare results to ensure no regressions
   - Monitor for test infrastructure compatibility

**Complexity**: Low
**Estimated Time**: 15 minutes (plus test execution time)

---

### 7. OxyPlot.SkiaSharp.Tests

**Current State:**
- Target Framework: `net8.0`
- Dependencies: OxyPlot, OxyPlot.SkiaSharp, ExampleLibrary, NUnit packages
- Issues: Project.0002 only

**Migration Plan:**
1. **Update Target Framework**
   ```xml
   <!-- Before -->
   <TargetFramework>net8.0</TargetFramework>

   <!-- After -->
   <TargetFrameworks>net8.0;net10.0</TargetFrameworks>
   ```

2. **Validation**
   - Run full test suite on both frameworks
   - Verify SkiaSharp rendering tests pass
   - Check for image comparison differences

**Complexity**: Low
**Estimated Time**: 15 minutes (plus test execution time)

---

## Risk Management

### Risk Assessment Matrix

| Risk | Probability | Impact | Mitigation Strategy |
|------|------------|--------|---------------------|
| **WPF Binary Incompatibilities** (1,347 instances) | Medium | High | Comprehensive runtime testing; maintain backward compatibility with conditional compilation |
| **API Source Incompatibilities** (56 instances) | Medium | Medium | Code review and refactoring; use modern .NET 10 APIs where appropriate |
| **.NET 10 SDK Not Installed** | Low | High | Validate SDK installation before migration; provide installation instructions |
| **Test Failures on .NET 10** | Medium | Medium | Run tests early and often; compare results with .NET 8 baseline |
| **Package Compatibility Issues** | Low | Medium | All packages verified as .NET 10 compatible in assessment |
| **Build System Compatibility** | Low | Low | SDK-style projects are fully compatible with .NET 10 |
| **Performance Regressions** | Low | Medium | Benchmark critical paths; compare with .NET 8 performance |

### Detailed Risk Mitigation

#### 1. WPF Binary Incompatibilities (High Priority)

**Risk Description:**
- 1,347 binary incompatibility warnings in WPF projects
- Most are in `OxyPlot.Wpf.Shared` (674) and `OxyPlot.SkiaSharp.Wpf` (673)
- Could cause runtime exceptions or behavioral changes

**Mitigation:**
- **Pre-Migration**: Document current WPF behavior with screenshots/recordings
- **During Migration**: 
  - Compile first, address build errors
  - Run application and test all WPF controls
  - Test rendering, events, data binding, and animations
- **Post-Migration**: 
  - Regression testing with visual validation
  - Performance profiling of rendering operations
- **Fallback**: Keep existing targets (net6.0-windows, net8.0-windows) functional

#### 2. API Source Incompatibilities

**Risk Description:**
- 56 source incompatibilities across OxyPlot and ExampleLibrary
- May require code changes or conditional compilation

**Mitigation:**
- **Analysis**: Use assessment.md to identify specific APIs and locations
- **Strategy Options**:
  1. **Modernize**: Replace deprecated APIs with .NET 10 equivalents (preferred)
  2. **Conditional**: Use `#if NET10_0_OR_GREATER` for platform-specific code
  3. **Abstraction**: Create compatibility shims for complex cases
- **Testing**: Unit tests verify behavior across all target frameworks
- **Documentation**: Record all API changes for future reference

#### 3. Testing Infrastructure

**Risk Description:**
- Tests must run on both .NET 8 and .NET 10
- Test results must be comparable

**Mitigation:**
- **Baseline**: Capture .NET 8 test results before migration
- **Parallel Execution**: Run tests on both frameworks simultaneously
- **Failure Analysis**: Investigate any .NET 10-specific failures immediately
- **CI/CD**: Update build pipelines to test all target frameworks

#### 4. Package Dependencies

**Risk Description:**
- System.Memory removal could cause issues if code directly depends on it

**Mitigation:**
- **Analysis**: Review ExampleLibrary code for direct System.Memory usage
- **Testing**: Build without package to catch compilation errors early
- **Documentation**: Note that consumers may need to remove System.Memory as well

### Rollback Procedures

#### Immediate Rollback (if critical issue found)
```bash
git checkout develop
git branch -D upgrade-to-NET10
```

#### Partial Rollback (keep code fixes, remove .NET 10 target)
1. Remove `net10.0` and `net10.0-windows` from all `<TargetFrameworks>`
2. Re-add System.Memory to ExampleLibrary if needed
3. Commit as intermediate state

#### Incremental Rollback (disable .NET 10 code paths)
```csharp
#if !NET10_0_OR_GREATER
// Use this to temporarily disable .NET 10-specific code
#endif
```

### Success Indicators
✅ All projects build successfully for all target frameworks  
✅ All unit tests pass on .NET 8 and .NET 10  
✅ WPF applications run without visual or functional regressions  
✅ No new warnings introduced (or documented if unavoidable)  
✅ Performance metrics comparable or improved on .NET 10

---

## Testing & Validation Strategy

### Testing Phases

#### Phase 1: Pre-Migration Baseline
**Goal**: Establish known-good state for comparison

1. **Build Verification**
   ```bash
   dotnet build Source/OxyPlot.SkiaSharp.sln --configuration Release
   ```
   - Capture build output
   - Document any existing warnings

2. **Test Execution**
   ```bash
   dotnet test Source/OxyPlot.SkiaSharp.sln --configuration Release
   ```
   - Record test results (pass/fail counts)
   - Note test execution times
   - Save as baseline for .NET 10 comparison

3. **WPF Visual Testing** (Manual)
   - Launch WPF example applications
   - Capture screenshots of key UI states
   - Document current rendering behavior

---

#### Phase 2: Build Validation
**Goal**: Ensure all projects compile for all target frameworks

1. **Clean Build**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build --configuration Release
   ```

2. **Per-Framework Builds**
   ```bash
   # Verify each project builds for net10.0 specifically
   dotnet build OxyPlot/OxyPlot.csproj -f net10.0
   dotnet build ExampleLibrary/ExampleLibrary.csproj -f net10.0
   # ... repeat for each project
   ```

3. **Build Output Analysis**
   - Zero errors required
   - Document any new warnings
   - Verify output directories contain net10.0 assemblies

---

#### Phase 3: Unit Testing
**Goal**: Verify functional correctness across frameworks

1. **Run All Tests - .NET 8 (Baseline)**
   ```bash
   dotnet test --framework net8.0 --logger "console;verbosity=detailed"
   ```

2. **Run All Tests - .NET 10 (Target)**
   ```bash
   dotnet test --framework net10.0 --logger "console;verbosity=detailed"
   ```

3. **Comparison Criteria**
   - Same number of tests discovered
   - Same pass/fail status for each test
   - Execution time within 20% variance
   - No new test failures on .NET 10

4. **Failure Investigation**
   - Any .NET 10-specific failure is a blocker
   - Analyze stack traces for API differences
   - Fix or document workarounds

---

#### Phase 4: Integration Testing
**Goal**: Validate project interactions and runtime behavior

1. **WPF Application Testing**
   - **OxyPlot.Wpf.Shared** validation
     - Launch WPF examples targeting net10.0-windows
     - Verify plot rendering
     - Test mouse interactions (zoom, pan)
     - Validate tooltips and legends

   - **OxyPlot.SkiaSharp.Wpf** validation
     - Verify SkiaSharp rendering in WPF
     - Compare visual output with net8.0-windows
     - Test export functionality

2. **Library Consumption Test**
   - Create simple console app targeting net10.0
   - Reference OxyPlot NuGet package (local build)
   - Verify basic plotting functionality

3. **Multi-Targeting Verification**
   - Build test projects for all frameworks simultaneously
   - Ensure consistent behavior across targets

---

#### Phase 5: Performance Testing (Optional but Recommended)
**Goal**: Ensure no performance regressions

1. **Benchmark Critical Paths**
   - Plot rendering time (various sizes)
   - Data series processing
   - Export operations

2. **Memory Profiling**
   - Compare memory usage .NET 8 vs .NET 10
   - Check for memory leaks in long-running scenarios

3. **Success Criteria**
   - .NET 10 performance within 5% of .NET 8
   - Or show measurable improvements

---

### Test Matrix

| Project | Build (net10.0) | Unit Tests | Integration | Visual |
|---------|----------------|------------|-------------|---------|
| OxyPlot | ✓ | ✓ | - | - |
| ExampleLibrary | ✓ | ✓ | - | - |
| OxyPlot.SkiaSharp | ✓ | ✓ | - | - |
| OxyPlot.Wpf.Shared | ✓ | - | ✓ | ✓ |
| OxyPlot.SkiaSharp.Wpf | ✓ | - | ✓ | ✓ |
| OxyPlot.Tests | ✓ | ✓ | - | - |
| OxyPlot.SkiaSharp.Tests | ✓ | ✓ | - | - |

---

### Validation Checklist

**Build Phase**
- [ ] .NET 10.0 SDK installed and accessible
- [ ] All 7 projects build without errors for net10.0
- [ ] No new build warnings introduced (or documented)
- [ ] NuGet packages generated with net10.0 target

**Testing Phase**
- [ ] All unit tests pass on .NET 8 (baseline)
- [ ] All unit tests pass on .NET 10
- [ ] Test counts match between frameworks
- [ ] No .NET 10-specific test failures

**WPF Validation**
- [ ] OxyPlot.Wpf.Shared runs on net10.0-windows
- [ ] Plot rendering matches expected output
- [ ] Interactive features work (zoom, pan, tooltips)
- [ ] OxyPlot.SkiaSharp.Wpf renders correctly
- [ ] No visual regressions compared to net8.0-windows

**Code Quality**
- [ ] All API incompatibilities addressed or documented
- [ ] System.Memory removed from ExampleLibrary
- [ ] Conditional compilation used appropriately
- [ ] Code compiles for all target frameworks

**Documentation**
- [ ] CHANGELOG updated with .NET 10 support
- [ ] README updated with .NET 10 requirements
- [ ] Known issues documented (if any)

---

### Automated Testing Integration

If CI/CD pipeline exists:
```yaml
# Example GitHub Actions / Azure DevOps step
- name: Test .NET 10
  run: |
    dotnet test --framework net10.0 --logger trx --results-directory ./TestResults

- name: Publish Test Results
  uses: actions/upload-artifact@v3
  with:
    name: test-results-net10
    path: ./TestResults
```

---

## Complexity & Effort Assessment

### Overall Complexity: **MEDIUM**

#### Complexity Factors

**Low Complexity Elements** ✅
- All projects use SDK-style format (easy to modify)
- Clear dependency graph with no circular dependencies
- All NuGet packages are .NET 10 compatible
- Well-structured solution with logical organization
- Active test suites for validation

**Medium Complexity Elements** ⚠️
- 56 API source incompatibilities requiring code changes
- Multi-targeting across 5 different framework versions
- WPF-specific considerations for binary compatibility

**High Complexity Elements** 🔴
- 1,347 WPF binary incompatibilities requiring runtime validation
- Limited automation for WPF visual regression testing
- Requires thorough manual testing of WPF components

---

### Time Estimates (Per Phase)

| Phase | Estimated Time | Breakdown |
|-------|---------------|-----------|
| **Pre-Migration Validation** | 30 minutes | SDK check (5m) + Baseline tests (15m) + Branch setup (10m) |
| **Project File Updates** | 30 minutes | 7 projects @ ~4 minutes each |
| **API Compatibility Fixes** | 2-4 hours | OxyPlot (2-3h) + ExampleLibrary (30m) + Review (30m) |
| **Build & Fix Errors** | 1 hour | Iterative build/fix cycles |
| **Unit Testing** | 1 hour | Run tests + investigate failures |
| **WPF Integration Testing** | 2-3 hours | Manual UI testing for 2 WPF projects |
| **Final Review & Commit** | 30 minutes | Code review + documentation |
| **Total** | **7.5-10.5 hours** | **Best case: 7.5h / Worst case: 10.5h** |

---

### Effort Distribution by Project

| Project | Complexity | Time Estimate | Primary Effort |
|---------|-----------|---------------|----------------|
| **OxyPlot** | Medium | 2-3 hours | API incompatibility fixes (49 instances) |
| **ExampleLibrary** | Low-Medium | 45 minutes | API fixes (7) + package removal |
| **OxyPlot.SkiaSharp** | Low | 15 minutes | TFM update only |
| **OxyPlot.Wpf.Shared** | High | 1.5-2 hours | Binary incompatibility testing (674) |
| **OxyPlot.SkiaSharp.Wpf** | High | 1.5-2 hours | Binary incompatibility testing (673) |
| **OxyPlot.Tests** | Low | 30 minutes | TFM update + test execution |
| **OxyPlot.SkiaSharp.Tests** | Low | 30 minutes | TFM update + test execution |

---

### Skill Requirements

| Skill | Required Level | Usage |
|-------|---------------|-------|
| **.NET Framework Knowledge** | Intermediate | Understanding multi-targeting |
| **C# Language** | Intermediate | API compatibility fixes |
| **WPF** | Intermediate-Advanced | Binary incompatibility troubleshooting |
| **NuGet Package Management** | Basic | Package removal and verification |
| **Git/Source Control** | Basic | Branch management, commits |
| **Testing** | Intermediate | Running and analyzing test results |

---

### Complexity Breakdown by Issue Type

#### Project.0002 (TFM Changes) - **LOW COMPLEXITY**
- **Count**: 7 instances (one per project)
- **Effort**: Straightforward XML edits
- **Risk**: Very low

#### Api.0002 (Source Incompatibilities) - **MEDIUM COMPLEXITY**
- **Count**: 56 instances
- **Effort**: Requires code analysis and refactoring
- **Risk**: Medium (compilation failures, behavioral changes)
- **Mitigation**: Incremental fixes with continuous build validation

#### Api.0001 (Binary Incompatibilities) - **HIGH COMPLEXITY**
- **Count**: 1,347 instances (primarily WPF)
- **Effort**: Runtime testing required; automated detection difficult
- **Risk**: High (runtime exceptions, subtle behavioral changes)
- **Mitigation**: Comprehensive manual testing, phased rollout

#### NuGet.0003 (Package Consolidation) - **LOW COMPLEXITY**
- **Count**: 1 instance
- **Effort**: Simple package reference removal
- **Risk**: Very low (functionality built into framework)

---

### Risk-Effort Matrix

```
High Effort  │                    │ • WPF Binary Compat
             │                    │   (High Risk, High Effort)
             │                    │
             │                    │
Medium Effort│ • API Source Fixes │
             │   (Medium Risk,    │
             │    Medium Effort)  │
             │                    │
Low Effort   │ • TFM Updates      │ • Package Removal
             │ • Test Projects    │
             └────────────────────┴──────────────────
              Low Risk            High Risk
```

---

### Recommendations for Efficiency

1. **Start with Low-Complexity Projects**
   - Begin with OxyPlot.SkiaSharp (15 min, no issues)
   - Build confidence before tackling API fixes

2. **Tackle API Fixes Early**
   - Address OxyPlot API incompatibilities first (blocks dependent projects)
   - Use automated refactoring tools where possible

3. **Parallelize Where Possible**
   - Build validation can run while reviewing code changes
   - Unit tests can run while preparing for WPF testing

4. **Allocate Extra Time for WPF**
   - Binary incompatibilities are unpredictable
   - Budget 50% contingency for WPF projects

5. **Use Incremental Commits**
   - Commit after each project successfully builds
   - Easier rollback if issues discovered later

---

### Success Metrics

**Quantitative**
- ✅ 100% of projects building for net10.0
- ✅ 100% of unit tests passing on net10.0
- ✅ Zero critical bugs introduced

**Qualitative**
- ✅ WPF applications function identically to net8.0-windows
- ✅ Code quality maintained or improved
- ✅ Documentation updated and clear

**Time-Based**
- ✅ Completed within 10.5 hours (worst case)
- ✅ No prolonged build breaks or blocking issues

---

## Source Control Strategy

### Branch Management

**Source Branch**: `develop`  
**Target Branch**: `upgrade-to-NET10`  
**Repository Root**: `C:\Users\andre.lundin\source\repos\test-stuff\oxyplot`

---

### Branching Strategy

```
develop (protected)
   │
   └─── upgrade-to-NET10 (feature branch)
           │
           ├─ commit: Initial TFM updates
           ├─ commit: Remove System.Memory package
           ├─ commit: Fix API incompatibilities - OxyPlot
           ├─ commit: Fix API incompatibilities - ExampleLibrary
           ├─ commit: Build validation - all projects
           ├─ commit: WPF compatibility fixes (if needed)
           └─ commit: Final documentation updates
```

---

### Commit Guidelines

#### Granular Commits
Each major change should be a separate commit for easy rollback:

1. **Project File Updates**
   ```
   git commit -m "chore: Add net10.0 target to all projects"
   ```

2. **Package Cleanup**
   ```
   git commit -m "chore: Remove System.Memory package from ExampleLibrary"
   ```

3. **API Fixes (Per Project)**
   ```
   git commit -m "fix: Resolve .NET 10 API incompatibilities in OxyPlot"
   git commit -m "fix: Resolve .NET 10 API incompatibilities in ExampleLibrary"
   ```

4. **Build Fixes**
   ```
   git commit -m "fix: Address build errors for net10.0 target"
   ```

5. **Test Updates**
   ```
   git commit -m "test: Verify all tests pass on .NET 10"
   ```

6. **Documentation**
   ```
   git commit -m "docs: Update README and CHANGELOG for .NET 10 support"
   ```

---

### Merge Strategy

**Option 1: Pull Request (Recommended)**
- Create PR from `upgrade-to-NET10` → `develop`
- Request code review from team
- Run CI/CD pipeline validation
- Squash or preserve commits based on team preference
- Merge after approval

**Option 2: Direct Merge**
- Ensure all tests pass locally
- Merge `upgrade-to-NET10` into `develop`
- Push to remote
- Monitor CI/CD for any issues

---

### Backup & Safety

**Before Starting**
```bash
# Verify clean working directory
git status

# Create backup tag
git tag backup-pre-net10-upgrade

# Create and switch to upgrade branch
git checkout -b upgrade-to-NET10
```

**During Migration**
```bash
# Frequent commits for easy rollback
git add .
git commit -m "checkpoint: [description]"

# Push to remote for backup
git push origin upgrade-to-NET10
```

**Rollback Scenarios**
```bash
# Scenario 1: Discard all changes
git checkout develop
git branch -D upgrade-to-NET10

# Scenario 2: Rollback to specific commit
git reset --hard <commit-hash>

# Scenario 3: Restore from backup tag
git checkout backup-pre-net10-upgrade
```

---

### .gitignore Considerations

Ensure these directories/files are ignored:
```
bin/
obj/
*.user
.vs/
TestResults/
```

No changes to .gitignore expected for this migration.

---

## Success Criteria

### Critical Success Criteria (Must-Have)

✅ **Build Success**
- All 7 projects build without errors for `net10.0` (or `net10.0-windows`)
- All existing target frameworks continue to build successfully
- NuGet package restore completes without warnings

✅ **Test Pass Rate**
- 100% of existing tests pass on .NET 8 (baseline)
- 100% of existing tests pass on .NET 10
- No new test failures introduced

✅ **Functional Correctness**
- WPF applications run without runtime exceptions
- Plot rendering produces correct visual output
- Interactive features work as expected

✅ **Source Control**
- All changes committed to `upgrade-to-NET10` branch
- Commit history is clean and meaningful
- No merge conflicts with `develop`

---

### Important Success Criteria (Should-Have)

⚠️ **Code Quality**
- No new compiler warnings (or documented if unavoidable)
- Code follows existing style and conventions
- API fixes use modern .NET 10 idioms where appropriate

⚠️ **Performance**
- .NET 10 performance within 10% of .NET 8
- No obvious memory leaks or inefficiencies

⚠️ **Documentation**
- README updated with .NET 10 support information
- CHANGELOG documents the migration
- Known issues documented (if any)

---

### Nice-to-Have Criteria (Optional)

💡 **Performance Improvements**
- Demonstrate measurable performance gains on .NET 10
- Optimize code to leverage .NET 10 features

💡 **Code Modernization**
- Adopt new C# language features where beneficial
- Refactor obsolete patterns to modern equivalents

💡 **CI/CD Integration**
- Update build pipelines to include .NET 10
- Add .NET 10-specific test runs

---

### Acceptance Checklist

**Pre-Migration**
- [ ] .NET 10 SDK installed and verified
- [ ] Baseline test results captured
- [ ] Branch created and switched (`upgrade-to-NET10`)
- [ ] No pending changes in working directory

**During Migration**
- [ ] All project files updated with `net10.0` target
- [ ] System.Memory package removed from ExampleLibrary
- [ ] API incompatibilities addressed in OxyPlot (49 instances)
- [ ] API incompatibilities addressed in ExampleLibrary (7 instances)
- [ ] All projects build for net10.0
- [ ] Unit tests run and pass on net10.0

**Post-Migration**
- [ ] WPF applications tested manually
- [ ] No visual or functional regressions observed
- [ ] Performance comparable to .NET 8
- [ ] Documentation updated
- [ ] All changes committed with meaningful messages
- [ ] Ready for code review / PR

**Release Readiness**
- [ ] PR created (if applicable)
- [ ] CI/CD pipeline passes
- [ ] Code review approved
- [ ] Merged to `develop` successfully

---

### Definition of Done

This migration is **COMPLETE** when:

1. All 7 projects build successfully for .NET 10.0
2. All unit tests pass on both .NET 8 and .NET 10
3. WPF applications function correctly on net10.0-windows
4. No critical bugs or regressions introduced
5. Code is committed to `upgrade-to-NET10` branch
6. Documentation reflects .NET 10 support
7. Team review completed (if required)
8. Changes merged to `develop` branch

---

### Rollback Criteria

Migration should be **ROLLED BACK** if:

1. Critical build failures that cannot be resolved within 2 hours
2. More than 10% of unit tests fail on .NET 10
3. WPF applications crash or exhibit severe visual corruption
4. Performance degradation exceeds 20% on .NET 10
5. Unresolvable API compatibility issues discovered
6. Project deadline requires deprioritizing this work

In case of rollback:
```bash
git checkout develop
git branch -D upgrade-to-NET10
# Address issues and retry later
```
