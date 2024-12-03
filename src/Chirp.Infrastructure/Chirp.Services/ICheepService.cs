namespace Chirp.Services;

using Chirp.Core.DTO;

public interface ICheepService
{
    /// <summary>
    /// Finds a specific cheep by its ID.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with a CheepDTO as the result.</returns>
    public Task<CheepDTO> FindSpecificCheepbyId(int cheepId);

    /// <summary>
    /// Creates a new message (cheep).
    /// </summary>
    /// <param name="newMessage">The new message to be created.</param>
    /// <returns>A task representing the asynchronous operation, with the ID of the created cheep.</returns>
    public Task<int> CreateMessage(NewCheepDTO newMessage);

    /// <summary>
    /// Retrieves a paginated list of public messages.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with a list of CheepDTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessages(int page);

    /// <summary>
    /// Retrieves a paginated list of public messages ordered by the oldest first.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with a list of CheepDTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyOldest(int page);

    /// <summary>
    /// Retrieves a paginated list of public messages ordered by the most liked.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with a list of CheepDTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyMostLiked(int page);

    /// <summary>
    /// Retrieves a paginated list of public messages ordered by relevance to the specified user.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="userName">The username to determine message relevance.</param>
    /// <returns>A task representing the asynchronous operation, with a list of CheepDTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyMostRelevance(int page, string userName);

    /// <summary>
    /// Retrieves a paginated list of messages from a specific user.
    /// </summary>
    /// <param name="userName">The username of the author to retrieve messages from.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with a list of CheepDTOs.</returns>
    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page);

    /// <summary>
    /// Retrieves a paginated list of messages from a user and their followers.
    /// </summary>
    /// <param name="userName">The username of the author to retrieve messages from.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with a list of CheepDTOs.</returns>
    public Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, int page);

    /// <summary>
    /// Counts the number of messages from a user and their followers.
    /// </summary>
    /// <param name="userName">The username to count messages for.</param>
    /// <returns>A task representing the asynchronous operation, with the count of messages.</returns>
    public Task<int> CountUserAndFollowerMessages(string userName);

    /// <summary>
    /// Counts the number of messages from a specific user.
    /// </summary>
    /// <param name="userName">The username to count messages for.</param>
    /// <returns>A task representing the asynchronous operation, with the count of messages.</returns>
    public Task<int> CountUserMessages(string userName);

    /// <summary>
    /// Counts the total number of public messages.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the total count of messages.</returns>
    public Task<int> CountPublicMessages();

    /// <summary>
    /// Updates an existing message with new information.
    /// </summary>
    /// <param name="alteredMessage">The updated message data.</param>
    /// <param name="id">The ID of the message to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task UpdateMessage(UpdateCheepDTO alteredMessage, int id);

    /// <summary>
    /// Creates a new author.
    /// </summary>
    /// <param name="newMessage">The data for the new author.</param>
    /// <returns>A task representing the asynchronous operation, with the result indicating the success of the creation.</returns>
    public Task<string> CreateAuthor(NewAuthorDTO newMessage);

    /// <summary>
    /// Finds authors by their username.
    /// </summary>
    /// <param name="userName">The username to search for.</param>
    /// <returns>A task representing the asynchronous operation, with a list of AuthorDTOs.</returns>
    public Task<List<AuthorDTO>> FindAuthorByName(string userName);

    /// <summary>
    /// Finds authors by their email.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <returns>A task representing the asynchronous operation, with a list of AuthorDTOs.</returns>
    public Task<List<AuthorDTO>> FindAuthorByEmail(string email);

    /// <summary>
    /// Finds a specific author by their ID.
    /// </summary>
    /// <param name="id">The ID of the author to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with the AuthorDTO.</returns>
    public Task<AuthorDTO> FindSpecificAuthorById(string id);

    /// <summary>
    /// Finds a specific author by their username.
    /// </summary>
    /// <param name="userName">The username of the author to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with the AuthorDTO.</returns>
    public Task<AuthorDTO> FindSpecificAuthorByName(string userName);

    /// <summary>
    /// Finds a specific author by their email.
    /// </summary>
    /// <param name="email">The email of the author to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with the AuthorDTO.</returns>
    public Task<AuthorDTO> FindSpecificAuthorByEmail(string email);

    /// <summary>
    /// Finds authors related to a user (e.g., following or followed by).
    /// </summary>
    /// <param name="userName">The username to search for related authors.</param>
    /// <returns>A task representing the asynchronous operation, with a list of AuthorDTOs.</returns>
    public Task<List<AuthorDTO>> FindAuthors(string userName);

    /// <summary>
    /// Retrieves a list of followers for a specific user by their username.
    /// </summary>
    /// <param name="userName">The username of the user whose followers are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of AuthorDTOs representing the followers.</returns>
    public Task<List<AuthorDTO>> GetFollowers(string userName);

    /// <summary>
    /// Retrieves a list of followers for a specific author by their ID.
    /// </summary>
    /// <param name="id">The ID of the author whose followers are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of AuthorDTOs representing the followers.</returns>
    public Task<List<AuthorDTO>> GetFollowersbyId(string id);

    /// <summary>
    /// Retrieves a list of authors that a specific user follows.
    /// </summary>
    /// <param name="userName">The username of the user whose followees are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of AuthorDTOs representing the followed authors.</returns>
    public Task<List<AuthorDTO>> GetFollowedby(string userName);

    /// <summary>
    /// Retrieves a list of authors that a specific author follows by their ID.
    /// </summary>
    /// <param name="id">The ID of the author whose followees are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of AuthorDTOs representing the followed authors.</returns>
    public Task<List<AuthorDTO>> GetFollowedbybyId(string id);

    /// <summary>
    /// Allows one author to follow another.
    /// </summary>
    /// <param name="id">The ID of the author who is following.</param>
    /// <param name="followerId">The ID of the author being followed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Follow(string id, string followerId);

    /// <summary>
    /// Allows one author to unfollow another.
    /// </summary>
    /// <param name="id">The ID of the author who is unfollowing.</param>
    /// <param name="followerId">The ID of the author being unfollowed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Unfollow(string id, string followerId);

    /// <summary>
    /// Checks if one author is following another.
    /// </summary>
    /// <param name="id">The ID of the author to check.</param>
    /// <param name="followerId">The ID of the potential follower.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the user is following the other author.</returns>
    public Task<Boolean> IsFollowing(string id, string followerId);

    /// <summary>
    /// Removes all data related to a specific user (including messages and author information).
    /// </summary>
    /// <param name="userName">The username of the author to remove.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
    public Task<bool> ForgetMe(string userName);

    /// <summary>
    /// Adds a like to a specific cheep from an author.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to like.</param>
    /// <param name="authorId">The ID of the author who is liking the cheep.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task AddLike(int cheepId, string authorId);

    /// <summary>
    /// Removes a like from a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to remove the like from.</param>
    /// <param name="authorId">The ID of the author who is removing the like.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveLike(int cheepId, string authorId);

    /// <summary>
    /// Checks if a specific user has liked a cheep.
    /// </summary>
    /// <param name="userName">The username of the user to check.</param>
    /// <param name="cheepId">The ID of the cheep to check for a like.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the user has liked the cheep.</returns>
    public Task<Boolean> HasLiked(string userName, int cheepId);

    /// <summary>
    /// Adds a dislike to a specific cheep from an author.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to dislike.</param>
    /// <param name="authorId">The ID of the author who is disliking the cheep.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task AddDislike(int cheepId, string authorId);

    /// <summary>
    /// Removes a dislike from a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to remove the dislike from.</param>
    /// <param name="authorId">The ID of the author who is removing the dislike.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveDislike(int cheepId, string authorId);

    /// <summary>
    /// Checks if a specific user has disliked a cheep.
    /// </summary>
    /// <param name="userName">The username of the user to check.</param>
    /// <param name="cheepId">The ID of the cheep to check for a dislike.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the user has disliked the cheep.</returns>
    public Task<Boolean> HasDisliked(string userName, int cheepId);
}
