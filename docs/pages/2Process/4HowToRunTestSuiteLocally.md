<!-- List all necessary steps that Adrian or Helge have to perform to execute your test suites. Here, you can assume that we already cloned your repository in the step above.

Briefly describe what kinds of tests you have in your test suites and what they are testing.-->

In order to run the test, you have to:

#### Install the right dependencies
Ensure you have Node.js and npm (Node Package Manager) installed and/or updated.
You can install Node.js from their website [Node.js](https://nodejs.org/en).

1. **update npm** 
```bash
$ npm install -g npm
```

2. **verify you have them installed**
```bash
$ node -v
$ npm -v
```

3. **Install playwright package**
```bash
$ npm install -g playwright
```

4. **Move to Playwright folder**
```bash
$ cd ./testPlaywright/PlaywrightTests
```

5. **Install the Playwright Script**
```bash
$ pwsh .\bin\Debug\net8.0\playwright.ps1 install
```

6. ***In a new terminal*, start a server**
```bash
$ dotnet watch --project ./src/Chirp.Web
```

7. ***In the first terminal*, move to the root directory of the project and type**
```bash
$ dotnet test
```

## Test suites

There are 8 test suites each focusing on different aspects of the solution. Following the **onion-architecture** allows the tests to focus on each layer individually using testing types such as *unit tests*, isolate a chain of method calls for *Integration testing* and *End-to-End Testing (E2E) testing*.

| Test File                                                                | Unit Tests | Integration Tests | E2E Tests |
|--------------------------------------------------------------------------|------------|-------------------|-----------|
| [AuthorTest.cs](#authortests-authortests)                                | yes        | no                | no        |
| [AuthorRepositoryTests.cs](#authorrepositorytests-authorrepositorytests) | yes        | yes               | no        |
| [CheepDBContextTest.cs](#cheepdbcontexttest-cheepdbcontexttest)          | no         | yes               | no        |
| [CheepRepositoryTests.cs](#cheeprepositorytests-cheeprepositorytests)    | yes        | yes               | no        |
| [HelperFunctionsTests.cs](#helperfunctionstests-helperfunctionstests)    | yes        | no                | no        |
| [CheepServiceTest.cs](#cheepservicetest-cheepservicetest)                | yes        | yes               | no        |
| [AzureTests.cs](#azuretests-azuretests)                                  | no         | no                | yes       |
| [LocalTests.cs](#localtests-localtests)                                  | no         | no                | yes       |

Tabel 1: List of the test suites and their types of testing

## What is tested?

#### AuthorTests {#AuthorTests}
>Focus: Validation of the Author datatype and its behavior.

##### Types of Testing
**Unit tests:**
- Property validations on the behavior of the `Author` datatype such as its required fields.


#### AuthorRepositoryTests {#AuthorRepositoryTests}
>Focus: Verifying the behavior of the repository pattern for Author.

##### Types of Testing
**Unit Tests:**
- Tests focused on individual repository methods like adding, retrieving updating and deleting authors.

**Integration Tests:**
- Tests that validate repository methods against an in-memory database

#### CheepDBContextTest {#CheepDBContextTest}
>Focus: Validating the setup and functionality of the database context.

##### Types of Testing
**Integration Tests:**
- Tests that involve actual database interactions, such as adding, retrieving updating and deleting authors, seeding and relationship checks.

*Examples:*
- Testing `CheepDBContext` initialization.
- Seeding the database correctly.
- Validating migrations and Schema enforcements.

#### CheepRepositoryTests {#CheepRepositoryTests}
>Focus: Validating repository methods for managing Cheep entities.

##### Types of Testing
**Unit Tests:**
- Focused on verifying the behavior of repository methods like querying, filtering, or updating cheeps.

**Integration Test:**
- Tests repository functionality against an a seeded mock database to ensure correctness with real data structures.

#### HelperFunctionsTests {#HelperFunctionsTests}
>Focus: Testing utility methods and reusable logic across the application.

##### Types of Testing
**Unit Tests:**
- Validates individual utility functions for correctness.

*Examples:*
- Converting a unix-timestamp to a date in string.
- Ensuring correct date formatting.

#### CheepServiceTest {#CheepServiceTest}
>Focus: Testing the business logic for cheeps at the service level.

##### Types of Testing
**Unit Tests:**
- Focus on the correctness of service methods with mocked dependencies.

*Examples:*
- Input validation.
- Business rules (e.g., maximum length)

**Integration Tests:**
- Test the interaction of the `CheepService` with the repository and database.

#### AzureTests {#AzureTests}
>Focus: End-to-end testing (E2E) on a live Azure-hosted application.

##### Types of Testing
**E2E Tests:**
- These validate the full application behavior in a live Azure environment, including user login, navigation, and interaction with web elements.

*Examples:*
- `LoginTest`
- `LoginChanges`
- `LogOut`

#### LocalTests {#LocalTests}
>Focus: Testing the application on a local development server.

##### Types of Testing
**E2E Tests:**
- Simulates user interactions with the application through Playwright.

*Examples:*
- `LocalLogin`
- `LocalLoginChanges`
- `LocalLogOut`
- `LocalShowingCheeps`
- `LocalNavItems`
