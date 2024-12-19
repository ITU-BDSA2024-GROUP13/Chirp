<!-- List all necessary steps that Adrian or Helge have to perform to execute your test suites. Here, you can assume that we already cloned your repository in the step above.

Briefly describe what kinds of tests you have in your test suites and what they are testing.-->


![[../../images/Test_coverage.png]]

The ***test*** package tests all the ***infrastructure*** and ***core*** using unit tests and integration tests.

The `web` package is tested via end-to-end tests using Playwright. Playwright does not provide code coverage.


In order to run the ***infrastructure*** and ***core*** tests:

go to the `Chirp\test` folder in your terminal.

Write `dotnet test` in your terminal to run all tests except Playwright tests. 

If you want to see code coverage. Run `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=lcov.info`

This should cover three packages: ***Chirp.Core***, ***Chirp.Repositories*** and ***Chirp.Services***.

In order to run the Playwright test, you have to:

#### Install the right dependencies
Make sure you have Node.js and npm (Node Package Manager) installed and/or updated.
You can install Node.js from their website [Node.js](https://nodejs.org/en).


1. **update npm in a powershell terminal at the root of your pc** 
```bash
$ npm install -g npm
```

2. **Verify you have them installed**
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

6. ***In a new terminal*, start a server on the root folder in `Chirp`**
```bash
$ dotnet watch --project ./src/Chirp.Web
```

7. ***In the first terminal***
```bash
$ dotnet test
```

Once playwright is correctly installed you can go to the root folder of Chirp and write `dotnet test`. This will run all tests in the project.

## Test suites

There are 8 test suites each focusing on different aspects of the solution. Following the **onion-architecture** allows the tests to focus on each layer individually using testing types such as *unit tests*, isolate a chain of method calls for *Integration testing* and *End-to-End (E2E) Testing*.

| Test File                                                                | Unit Tests | Integration Tests | E2E Tests |
|--------------------------------------------------------------------------|------------|-------------------|-----------|
| [AuthorTest.cs](#authortests)                                | yes        | no                | no        |
| [AuthorRepositoryTests.cs](#authorrepositorytests) | yes        | yes               | no        |
| [CheepDBContextTest.cs](#cheepdbcontexttest)          | no         | yes               | no        |
| [CheepRepositoryTests.cs](#cheeprepositorytests)    | yes        | yes               | no        |
| [HelperFunctionsTests.cs](#helperfunctionstests)    | yes        | no                | no        |
| [CheepServiceTest.cs](#cheepservicetest)                | yes        | yes               | no        |
| [AzureTests.cs](#azuretests)                                  | no         | no                | yes       |
| [LocalTests.cs](#localtests)                                  | no         | no                | yes       |

Tabel 1: List of the test suites and their types of testing

## What is tested?

#### AuthorTests
>Focus: Validation of the Author datatype and its behavior.

##### Types of Testing
**Unit tests:**
- Property validations on the behavior of the `Author` datatype such as its required fields.


#### AuthorRepositoryTests
>Focus: Verifying the behavior of the repository pattern for Author.

##### Types of Testing
**Unit Tests:**
- Tests focused on individual repository methods like adding, retrieving updating and deleting authors.

**Integration Tests:**
- Tests that validate repository methods against an in-memory database

#### CheepDBContextTest
>Focus: Validating the setup and functionality of the database context.

##### Types of Testing
**Integration Tests:**
- Tests that involve actual database interactions, such as adding, retrieving updating and deleting authors, seeding and relationship checks.

*Examples:*
- Testing `CheepDBContext` initialization.
- Seeding the database correctly.
- Validating migrations and Schema enforcements.

#### CheepRepositoryTests
>Focus: Validating repository methods for managing Cheep entities.

##### Types of Testing
**Unit Tests:**
- Focused on verifying the behavior of repository methods like querying, filtering, or updating cheeps.

**Integration Test:**
- Tests repository functionality against an a seeded mock database to ensure correctness with real data structures.

#### HelperFunctionsTests
>Focus: Testing utility methods and reusable logic across the application.

##### Types of Testing
**Unit Tests:**
- Validates individual utility functions for correctness.

*Examples:*
- Converting a unix-timestamp to a date in string.
- Ensuring correct date formatting.

#### CheepServiceTest
>Focus: Testing the business logic for cheeps at the service level.

##### Types of Testing
**Unit Tests:**
- Focus on the correctness of service methods with mocked dependencies.

*Examples:*
- Input validation.
- Business rules (e.g., maximum length)

**Integration Tests:**
- Test the interaction of the `CheepService` with the repository and database.

#### AzureTests
>Focus: End-to-end testing (E2E) on a live Azure-hosted application.

##### Types of Testing
**E2E Tests:**
- These validate the full application behavior in a live Azure environment, including user login, navigation, and interaction with web elements.

*Examples:*
- `LoginTest`
- `LoginChanges`
- `LogOut`

#### LocalTests
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



