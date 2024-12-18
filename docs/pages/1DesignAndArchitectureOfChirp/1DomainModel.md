

![[../../diagrams/class-diagrams/Domain.png]]


The Domain for the Chirp location is based off of two entities, and one superclass, IdentityUser, which Author extends from. The will derive attributes such as id, username, email and an encrypted password.

Authors are the equivalent of application users. Apart from the extended attributes, authors contain all cheeps (messages), that they have written. They keep track of who they follow, and who they are followed by. And they keep track of which cheeps they have liked or disliked.
They cannot like and dislike one cheep at the same time.
Two authors cannot have the same case-insensitive username or email (An author cannot be named helge, if Helge already exists).


Cheeps are all the messages of the application.
Cheeps are identified with their id.
They contain attributes, like text, an optional image, the time of posting, and their associated author.
They contain data for which author has liked or disliked.
They also contain a calculated float based on their amount of likes, which is used for sorting for relevance.

Additionally, the cheep cannot be more than 160 characters long and they can only be owned by a single author.

