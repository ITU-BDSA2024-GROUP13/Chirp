<!-- %Illustrate the organization of your code base. That is, illustrate which layers exist in your (onion) architecture. Make sure to illustrate which part of your code is residing in which layer. -->

Design and architecture

![[../../diagrams/class-diagrams/Core.png]]

The Chirp.Core package contains the domain entities and data transfer objects, for database transactions.

The DTO's are split up in two groups. One for each entity.
In order to obtain the SOLID principles. DTO's are split up even further to strive for the single responsibilty principle. Thus there is the NewCheepDTO, which is for sending data of new cheeps into the database. The default CheepDTO is for reading cheeps, so they show on the timeline.
The UpdateCheepDTO is for editing existing cheeps, by changing their content (their text).
Lastly there is the CheepDTOForRelevance which is used specifically for the relevance sorting algorithm.

For the author entity, two DTO's have been made for either creating an author or to get information on the author from the database. 


![[../../diagrams/class-diagrams/Repo.png]]

The `Chirp.Infrastructure.Repositories` package, contains classes and interfaces regarding the database and classes which seed or query the database.
AuthorRepository and CheepRepository queries the database depending on whether Author or Cheep is the main entity.

The `CheepDBContext` defines the database entities and the relations between them.

Both `AuthorRepository`, `CheepRepository` and `CheepDBContext` are dependency injected into the application.
This ensures one and only one instance of each.

The `DbInitializer` seeds the database with default cheeps and authors. This makes it easier to make in-memory testing.

The static class `HelperFunctions` provides functionality to the CheepRepository. Since cheeps contain DateTime and DTO's should only store predefined types, DateTime needs to be converted to unixTime of type long and vice versa.

![[../../diagrams/class-diagrams/Service.png]]

The Chirp.Services package contains the CheepService class, which directly communicates with the page models.

The service transacts data between the page models and indirectly the database via the repositories.

CheepService contains the dependency injected IAuthorRepository and ICheepRepository.
The CheepService itself is also dependency injected into the application. So page models refer to the same service, which refers to the same repositories, which refer to the same database.


![[../../diagrams/class-diagrams/Web.png]]

The Chirp.Web package contains all the pages, as well as the startup program.

The pages are made up of page models written in c sharp
and pages in cshtml.

The cshtml pages send requests to the model which can be handled by reading or writing to the databse via the application's associated service interface.

The scaffolded package Area.Indetity.Page.Account is used to handle getting an identity token when logging in and managing the account using Microft.AspNet.Identity's IdentityUser.

![[../../diagrams/class-diagrams/onion/Onion-coloured.png]]

The entire Chirp package fulfills the onion architecture. Since Chirp.Core does not need to refer to any of the out layers. The same goes for repository layer and service layer.


