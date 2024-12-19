<!-- %Illustrate the organization of your code base. That is, illustrate which layers exist in your (onion) architecture. Make sure to illustrate which part of your code is residing in which layer. -->

### Design and architecture

![[../../diagrams/class-diagrams/Core.png]]

The `Chirp.Core` package contains the domain entities and data transfer objects, for database transactions.

The `DTO`'s are split up into two groups, one for each *entity*.
In order to obtain the SOLID principles. `DTO`'s are split up even further to strive for the single responsibility principle. Thus there is the `NewCheepDTO`, which is for sending data of new *cheeps* into the database. The default `CheepDTO` is for reading *cheeps*, for showing on the timeline.
The `UpdateCheepDTO` is for editing existing cheeps, by changing their content.
Lastly, there is the `CheepDTOForRelevance` which is used for the relevance sorting algorithm.

For the *author* entity, two `DTO`'s have been made for either creating an *author* or to get information on the *author* from the database. 


![[../../diagrams/class-diagrams/Repo.png]]

The `Chirp.Infrastructure.Repositories` package, contains classes and interfaces regarding the database and classes which seed or query the database.
`AuthorRepository` and `CheepRepository` queries the database depending on whether Author or Cheep is the main entity.

The `CheepDBContext` defines the database *entities* and the relations between them.

Both `AuthorRepository`, `CheepRepository` and `CheepDBContext` are dependency injected into the application.
This ensures one and only one instance of each.

The `DbInitializer` seeds the database with default *cheeps* and *authors*. This makes it easier to make in-memory testing.

The static class `HelperFunctions` provides functionality to the `CheepRepository`. Since *cheeps* contain `DateTime` and `DTO`'s should only store predefined types, `DateTime` needs to be converted to unixTime of type `long` and vice versa.

![[../../diagrams/class-diagrams/Service.png]]

The `Chirp.Services` package contains the `CheepService` class, which directly communicates with the page models.

The service transacts data between the page models and indirectly the database using the repositories.

`CheepService` contains the dependency injected `IAuthorRepository` and `ICheepRepository`.
The `CheepService` itself is also dependency, injected into the application. Page models refer to the same service, which refers to the same repositories, which refer to the same database.


![[../../diagrams/class-diagrams/Web.png]]

The `Chirp.Web` package contains all the pages, as well as the startup program.

The pages are made up of page models written in `C#` and the pages in `cshtml`.

The `cshtml` pages send requests to the model which are handled by reading or writing to the database using the application's associated service interface.

The scaffolded package `Area.Identity.Page.Account` is used to handle getting an identity token when logging in and managing the account using `Microsoft.AspNet.Identity`'s IdentityUser.

![[../../diagrams/class-diagrams/onion/Onion-coloured.png]]

The entire `Chirp` package fulfills the onion architecture. Since `Chirp.Core` does not need to refer to any of the outer layers. The same goes for the repository layer and the git service layer.


![[../../images/Test_coverage.png]]

The `test` package tests all the `infrastructure` and `core` using unit tests and integration tests.
The `web` package is tested via end-to-end tests using Playwright. Playwright does not provide code coverage.


