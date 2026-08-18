# Project rules for Claude

## What this is

TenDigitPrimeCalculatorCSharp is a small console application that solves the Google billboard
problem: it walks the digits of the Euler number behind the decimal point and prints the first
window of ten consecutive digits that forms a prime number. The answer is `7427466391`, found at
index 98 of those digits. The repository is a code example, it is **not** published as a NuGet
package and there is no installer: no `GeneratePackageOnBuild`, no push script, no `Setup` folder.

One solution `src/TenDigitPrimeCalculatorCSharp.sln` with exactly two projects:

- `src/TenDigitPrimeCalculatorCSharp/TenDigitPrimeCalculatorCSharp.csproj`, `OutputType` `Exe`, the
  actual code.
- `src/TenDigitPrimeCalculatorCSharp.Tests/TenDigitPrimeCalculatorCSharp.Tests.csproj`, MSTest,
  added in version 1.0.8.0.

Layout inside `src/TenDigitPrimeCalculatorCSharp`:

- `TenDigitPrimeCalculator.cs` plus `ITenDigitPrimeCalculator.cs`: the public entry point.
  `CalculatePrimes` walks the digits and returns the first ten digit prime as a string, or an empty
  string if there is none. The private helper `IsPrime` does the primality check by trial division.
- `Program.cs`: `Main` creates the calculator, prints the result and waits for a key press.
- `GlobalUsings.cs`: an empty `#pragma` block, the project needs no global using of its own.
- `License.txt`: a byte identical copy of the `License.txt` in the repository root. Nothing
  references it, no `.csproj` item includes it, it is simply tracked. Leave it alone.

Layout inside `src/TenDigitPrimeCalculatorCSharp.Tests`:

- `TenDigitPrimeCalculatorTests.cs`: the documented result, the ten digit shape of the result, an
  independent primality check of the result, the proof that no earlier window is prime, two calls
  and two instances returning the same value, the console output and the test data itself.
- `TestDataProvider.cs`: the expected prime `7427466391`, its index `98` and the first 108 digits of
  the Euler number. Those digits are written down a second time on purpose. The calculator holds its
  own copy in a private constant, so the tests compare it against data that does not come from the
  code under test.
- `GlobalUsings.cs`: all usings of the test project.

The private helper `IsPrimeReference` in the test class is a second primality implementation,
stepping in steps of six instead of two. It exists so that a mistake in `IsPrime` cannot be
confirmed by a test that repeats the same mistake. Keep the two implementations different.

Repository root: `README.md` (the only user documentation), `Changelog.md`, `License.txt` (MIT),
`.gitignore` and `.gitattributes`. There is no `Updating.md`, no `HowToUse.md`, no screenshots and no
`.github` folder.

## Build

```powershell
dotnet build src/TenDigitPrimeCalculatorCSharp.sln
```

```powershell
dotnet test src/TenDigitPrimeCalculatorCSharp.sln
```

- Single target framework `net10.0` in both projects, no multi-targeting, no `RuntimeIdentifiers`.
  Nothing in the code is Windows specific.
- All build properties live directly in the two `.csproj` files and are duplicated there. There is
  **no** `Directory.Build.props` in this repository.
- `TreatWarningsAsErrors` is enabled in both projects, so every warning breaks the build, NuGet
  warnings (`NU****`) from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.9-1` for the first
  commit after tag `1.0.8`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/TenDigitPrimeCalculatorCSharp.sln --source https://api.nuget.org/v3/index.json`.
- Tests are MSTest, in the single test project `src/TenDigitPrimeCalculatorCSharp.Tests`, which
  follows the same package set as the sibling repositories: `Microsoft.NET.Test.Sdk`,
  `MSTest.TestAdapter`, `MSTest.TestFramework`, `coverlet.collector` and `GitVersion.MsBuild`.
  `dotnet test` runs 8 tests, they need no network, no fixture and no file outside the repository,
  and a test run leaves the working tree untouched. Never claim a test run happened without running
  it.
- Beyond the tests, a behaviour change is verified by running the program and checking that it prints
  `'7427466391' is a prime number`. The tests are fast because the answer sits at index 98, so a run
  is 99 iterations, not thousands.

## Code conventions

Follow the surrounding code, it is consistent throughout every file:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private members included, no exceptions.
  Implementations of an interface member additionally carry `<inheritdoc cref="..."/>` and
  `<seealso cref="..."/>` pointing at that interface.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into the `GlobalUsings.cs` of the respective project, inside the
  existing `#pragma warning disable IDE0065` block, never at the top of a file. The editorconfig
  requires usings inside the namespace (`csharp_using_directive_placement=inside_namespace:warning`),
  which global usings cannot satisfy, that is what the pragma is for. Do not add other pragmas. The
  comment text in that block is German because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.
- The two `.cs` files of the calculator use string concatenation for their console output, the tests
  use interpolation. Keep each file the way it is instead of unifying the style.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **The digits are a hard coded constant.** `EDigits` in `TenDigitPrimeCalculator.cs` holds 2757
  digits of the Euler number behind the decimal point as one string literal, starting with
  `7182818284`, so the leading `2` and the decimal point are not part of it. Nothing computes those
  digits, and nothing verifies them beyond the first 108 the tests know. A digit changed by accident
  in the middle of that literal would not be caught by anything.
- **`Sequence` is a constant of ten, the sliding window is not.** `CalculatePrimes` steps one digit
  at a time and cuts ten digits out. The loop bound uses `EDigits.Length - Sequence`, not a literal
  ten, so the window size lives in one place.
- **A window may start with a zero.** `Convert.ToInt64("0452353602")` returns a nine digit number,
  and the code checks it like any other candidate. That is harmless for the result because the
  answer starts with a seven, but a window starting with a zero is not a ten digit number, whatever
  the primality check says about it.
- **`IsPrime` is trial division, and that is on purpose.** It checks values below two, the value two
  itself and even values, then divides by every odd number up to the square root. For ten digit
  candidates that is at most about 50000 divisions, which is fast enough. Do not replace it with a
  sieve or a probabilistic test, the point of the example is that the plain approach suffices.
- **The result of `CalculatePrimes` never leaves the process.** `Program.Main` prints it and throws
  it away. `CalculatePrimes` also prints every step itself, so the program writes about 300 lines
  before the answer. Judge a run by the last lines, not by the exit code alone.
- **The program waits for a key press, unless its input is redirected.** `Main` returns early when
  `Console.IsInputRedirected` is true. Without that check `Console.ReadKey` throws
  `InvalidOperationException` at the very end of an otherwise successful run, which is what happens
  when the program is started from a script. Do not remove the check to "simplify" the ending.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no `.github` folder and no pipeline file here.
- **`src/TenDigitPrimeCalculatorCSharp.sln.DotSettings`** is tracked and holds nothing but a
  ReSharper user dictionary with the single word `H_00E4mmer`. Leave it alone.
- **The solution knows only `Any CPU`.** `dotnet sln add` likes to add `x64` and `x86` platform
  configurations when a project is added. They were removed again, the sibling repositories have
  `Any CPU` only. If you add a project, check the solution diff afterwards.
- **`.gitattributes` sets `* text=auto`** and every rule of the Visual Studio template below it is
  commented out. There is no binary file in this repository, so nothing needs its own rule yet. Any
  binary file added later does.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.8.0 (2026-08-18)** : Short description.`
3. Commit that.
4. Tag the commit with the plain version number, no `v` prefix (`1.0.8`, `1.0.7`, ...). The existing
   tags are lightweight tags, create new ones the same way.
5. Push the commits and the tag.

The version in the `Changelog.md` has four parts (`1.0.8.0`), the tag has three (`1.0.8`).
GitVersion turns the tag into the assembly version, so an untagged commit produces something like
`1.0.8-1+Branch.master.Sha...`. There is no installer to build and no package to push, so the
release ends with the push.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
