%Illustrate the organization of your code base. That is, illustrate which layers exist in your (onion) architecture. Make sure to illustrate which part of your code is residing in which layer.

Design and architecture

![[../../diagrams/class-diagrams/Core.png]]

The Chirp.Core package contains the domain entities and data transfer objects, for database transactions.

The DTO's are split up in two groups. One for each entity.
In order to obtain the SOLID principlies. DTO's are split up even further to strive for the single responsibilty principle. Thus there is the NewCheepDTO, which is for sending data of new cheeps into the database. The default CheepDTO is for reading cheeps, so hey show on the timeline.
The UpdateCheepDTO is for editing  existing cheeps, by chaning their content (their text).
Lastly there is the CheepDTOForRelevance which is used specifically for the relevance sorting algorithm.

For the author entity, two DTO's have been made for either creating an author or to read an author. 


![[../../diagrams/class-diagrams/Repo.png]]


![[../../diagrams/class-diagrams/Services.png]]

![[../../diagrams/class-diagrams/Web.png]]




