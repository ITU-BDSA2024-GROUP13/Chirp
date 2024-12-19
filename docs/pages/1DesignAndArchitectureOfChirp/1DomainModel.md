

![[../../diagrams/class-diagrams/Domain.png]]


The Domain for chirp is based off of two *entities*, and one superclass, `IdentityUser`, which *Author* extends. They derive attributes such as `id`, `username`, `email` and an encrypted password.

*Authors* are the equivalent of application users. Apart from the extended attributes, *Authors* contain all *Cheeps* (messages), that they have written. They keep track of who they follow, who they are followed by, *Cheeps* they have liked, and *Cheeps* they have disliked.
They cannot both like and dislike a *cheep* at the same time.
Two *Authors* cannot have the same case-insensitive username or email i.e. An *Author* cannot be named "helge", if "Helge" already exists. <!-- one of the Helges needs to be small h-->

*Cheeps* are all the messages of the application.
*Cheeps* are identified with a unique id.
They contain attributes, like text, an optional image, the time of posting, and their associated *Author*.
They contain data for which *Authors* have liked or disliked.
They also contain a calculated float based on their amount of likes, which is used for sorting by relevance.

Additionally, the attribute `Text` in *cheep* cannot contain more than 160 characters and they can only have a single *Author*.

