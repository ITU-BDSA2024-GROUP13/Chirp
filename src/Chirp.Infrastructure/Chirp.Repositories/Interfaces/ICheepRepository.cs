namespace Chirp.Repositories;
using Chirp.Core.DTO;

public interface ICheepRepository
{
    /// <summary>
    /// Creates a new cheep (message) in the system.
    /// </summary>
    /// <param name="newMessage">The data transfer object containing the details of the new cheep.</param>
    /// <returns>A task representing the asynchronous operation, with the result being the ID of the newly created cheep.</returns>
    public Task<int> CreateMessage(NewCheepDTO newMessage);

    /// <summary>
    /// Finds a specific cheep by its ID.
    /// </summary>
    /// <param name="id">The ID of the cheep to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with the result being the specific cheep matching the given ID.</returns>
    public Task<CheepDTO> FindSpecificCheepbyId(int id);

    /// <summary>
    /// Retrieves a list of public cheeps, paginated by the given take and skip values.
    /// </summary>
    /// <param name="takeValue">The number of cheeps to take.</param>
    /// <param name="skipValue">The number of cheeps to skip (used for pagination).</param>
    /// <returns>A task representing the asynchronous operation, with a list of public cheeps.</returns>
    public Task<List<CheepDTO>> ReadPublicMessages(int takeValue, int skipValue);

    /// <summary>
    /// Retrieves a list of public cheeps, sorted by oldest first, with pagination.
    /// </summary>
    /// <param name="takeValue">The number of cheeps to take.</param>
    /// <param name="skipValue">The number of cheeps to skip (used for pagination).</param>
    /// <returns>A task representing the asynchronous operation, with a list of public cheeps sorted by oldest.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyOldest(int takeValue, int skipValue);

    /// <summary>
    /// Retrieves a list of public cheeps, sorted by most liked, with pagination.
    /// </summary>
    /// <param name="takeValue">The number of cheeps to take.</param>
    /// <param name="skipValue">The number of cheeps to skip (used for pagination).</param>
    /// <returns>A task representing the asynchronous operation, with a list of public cheeps sorted by most liked.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyMostLiked(int takeValue, int skipValue);

    /// <summary>
    /// Retrieves a list of public cheeps, sorted by relevance to the given user, with pagination.
    /// </summary>
    /// <param name="takeValue">The number of cheeps to take.</param>
    /// <param name="skipValue">The number of cheeps to skip (used for pagination).</param>
    /// <param name="userName">The username of the user for whom the relevance of the cheeps is calculated.</param>
    /// <returns>A task representing the asynchronous operation, with a list of cheeps sorted by relevance.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyRelevance(int takeValue, int skipValue, string userName);

    /// <summary>
    /// Retrieves a list of cheeps posted by a specific user, paginated by the given take and skip values.
    /// </summary>
    /// <param name="userName">The username of the user whose cheeps to retrieve.</param>
    /// <param name="takeValue">The number of cheeps to take.</param>
    /// <param name="skipValue">The number of cheeps to skip (used for pagination).</param>
    /// <returns>A task representing the asynchronous operation, with a list of cheeps from the specified user.</returns>
    public Task<List<CheepDTO>> ReadUserMessages(string userName, int takeValue, int skipValue);

    /// <summary>
    /// Updates an existing cheep's content.
    /// </summary>
    /// <param name="alteredMessage">The data transfer object containing the updated content for the cheep.</param>
    /// <param name="id">The ID of the cheep to be updated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task UpdateMessage(UpdateCheepDTO alteredMessage, int id);

    /// <summary>
    /// Adds a like to a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to like.</param>
    /// <param name="authorId">The ID of the author who is liking the cheep.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task AddLike(int cheepId, string authorId);

    /// <summary>
    /// Adds a dislike to a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to dislike.</param>
    /// <param name="authorId">The ID of the author who is disliking the cheep.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task AddDisLike(int cheepId, string authorId);

    /// <summary>
    /// Retrieves all authors who liked a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to get likers for.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors who liked the cheep.</returns>
    public Task<List<AuthorDTO>> GetAllLikers(int cheepId);

    /// <summary>
    /// Retrieves all authors who disliked a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to get dislikers for.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors who disliked the cheep.</returns>
    public Task<List<AuthorDTO>> GetAllDislikers(int cheepId);

    /// <summary>
    /// Removes a like from a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep from which to remove the like.</param>
    /// <param name="authorId">The ID of the author who is removing their like.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveLike(int cheepId, string authorId);

    /// <summary>
    /// Removes a dislike from a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep from which to remove the dislike.</param>
    /// <param name="authorId">The ID of the author who is removing their dislike.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveDislike(int cheepId, string authorId);

    /// <summary>
    /// Removes all likes from a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to remove all likes from.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveAllLikes(int cheepId);

    /// <summary>
    /// Removes all dislikes from a specific cheep.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to remove all dislikes from.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveAllDislikes(int cheepId);

    public double RelevancePoints(int cheepid, string follower, string userName, double likeRatio, DateTime timeStamp, bool follows, bool disliked);

    public Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, List<string> followers, int takeValue, int skipValue);

    /// <summary>
    /// Removes all cheeps from a specific user.
    /// </summary>
    /// <param name="userName">The username of the user whose cheeps should be removed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveCheepsFromUser(string userName);
}
