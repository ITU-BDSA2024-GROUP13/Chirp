namespace Chirp.Services;

using Chirp.Repositories;
using Chirp.Core.DTO;

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    /// <summary>
    /// Finds a specific cheep by its ID.
    /// </summary>
    /// <param name="cheepId">The unique identifier of the cheep.</param>
    /// <returns>A task that represents the asynchronous operation, containing the cheep DTO.</returns>
    public Task<CheepDTO> FindSpecificCheepbyId(int cheepId)
    {
        return _cheepRepository.FindSpecificCheepbyId(cheepId);
    }

    /// <summary>
    /// Reads public messages with pagination support.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cheep DTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessages(int page)
    {
        return _cheepRepository.ReadPublicMessages(32, 32 * page);
    }

    /// <summary>
    /// Reads public messages, ordered by the oldest first, with pagination support.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cheep DTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyOldest(int page)
    {
        return _cheepRepository.ReadPublicMessagesbyOldest(32, 32 * page);
    }

    /// <summary>
    /// Reads public messages, ordered by the most liked, with pagination support.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cheep DTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyMostLiked(int page)
    {
        return _cheepRepository.ReadPublicMessagesbyMostLiked(32, 32 * page);
    }

    /// <summary>
    /// Reads public messages, ordered by relevance for the specified user, with pagination support.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="userName">The user name used to determine message relevance.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cheep DTOs.</returns>
    public Task<List<CheepDTO>> ReadPublicMessagesbyMostRelevance(int page, string userName)
    {
        return _cheepRepository.ReadPublicMessagesbyRelevance(32, 32 * page, userName);
    }

    /// <summary>
    /// Reads messages of a specific user, with pagination support.
    /// </summary>
    /// <param name="userName">The name of the user whose messages are being fetched.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cheep DTOs.</returns>
    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page)
    {
        return _cheepRepository.ReadUserMessages(userName, 32, 32 * page);
    }

    /// <summary>
    /// Reads messages of a user and their followers, with pagination support.
    /// </summary>
    /// <param name="userName">The name of the user whose and their followers' messages are being fetched.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cheep DTOs.</returns>
    public async Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, int page)
    {
        List<string> followers = AuthorToString(await _authorRepository.GetFollowing(userName));
        return await _cheepRepository.ReadUserAndFollowerMessages(userName, followers, 32, 32 * page);
    }

    /// <summary>
    /// Converts a list of AuthorDTO objects into a list of author names (strings).
    /// </summary>
    /// <param name="authors">A list of AuthorDTO objects to be converted.</param>
    /// <returns>A list of strings representing the names of the authors.</returns>
    private static List<string> AuthorToString(List<AuthorDTO> authors)
    {
        return authors.Select(a => a.Name).ToList();
    }

    /// <summary>
    /// Creates a new cheep message.
    /// </summary>
    /// <param name="message">The new cheep message data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation, returning the ID of the newly created message.</returns>
    public async Task<int> CreateMessage(NewCheepDTO message)
    {

        if (message.Text.Length > 160)
        {
            Console.WriteLine("Message is too long!");
            return 0;
        }
        try {
            AuthorDTO author = await FindSpecificAuthorByName(message.Author);} 
        catch {
            NewAuthorDTO newAuthor = new() { Name = message.Author, Email = message.Author + "@mail.com" };
            await CreateAuthor(newAuthor);
            AuthorDTO createdAuthor = await FindSpecificAuthorByName(message.Author);
            message.AuthorId = createdAuthor.Id!;
        }

        return await _cheepRepository.CreateMessage(message);
    }

    /// <summary>
    /// Counts the total number of messages for a user and their followers.
    /// </summary>
    /// <param name="userName">The name of the user whose message count is being retrieved.</param>
    /// <returns>A task that represents the asynchronous operation, containing the total count of messages.</returns>
    public async Task<int> CountUserAndFollowerMessages(string userName)
    {
        List<string> followers = AuthorToString(await _authorRepository.GetFollowing(userName));

        var list = await _cheepRepository.ReadUserAndFollowerMessages(userName, followers, int.MaxValue, 0);
        var result = list.Count;
        return result;
    }

    /// <summary>
    /// Counts the total number of public messages.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the total count of public messages.</returns>
    public async Task<int> CountPublicMessages()
    {
        var list = await _cheepRepository.ReadPublicMessages(int.MaxValue, 0);
        var result = list.Count;
        return result;
    }

    /// <summary>
    /// Counts the total number of messages for a specific user.
    /// </summary>
    /// <param name="userName">The name of the user whose message count is being retrieved.</param>
    /// <returns>A task that represents the asynchronous operation, containing the total count of user messages.</returns>
    public async Task<int> CountUserMessages(string userName)
    {
        var list = await _cheepRepository.ReadUserMessages(userName, int.MaxValue, 0);
        var result = list.Count;
        return result;
    }

    /// <summary>
    /// Updates an existing message with new data.
    /// </summary>
    /// <param name="alteredMessage">The updated cheep data transfer object.</param>
    /// <param name="id">The ID of the message to update.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    public Task UpdateMessage(UpdateCheepDTO alteredMessage, int id)
    {
        return _cheepRepository.UpdateMessage(alteredMessage, id);
    }

    /// <summary>
    /// Deprecated! The functionality is now handled with Identity.
    /// 
    /// Creates a new author using a data transfer object.
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public async Task<string> CreateAuthor(NewAuthorDTO author)
    {
        return await _authorRepository.CreateAuthor(author);
    }
    
    /// <summary>
    /// Finds authors whose names start with the specified string.
    /// </summary>
    /// <param name="userName">The partial user name to search for.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of matching authors.</returns>
    public async Task<List<AuthorDTO>> FindAuthorByName(string userName)
    {
        return await _authorRepository.FindAuthorByName(userName);
    }

    /// <summary>
    /// Finds authors by their email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of matching authors.</returns>
    public async Task<List<AuthorDTO>> FindAuthorByEmail(string email)
    {
        return await _authorRepository.FindAuthorByEmail(email);
    }

    /// <summary>
    /// Finds a specific author by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    /// <returns>A task representing the asynchronous operation, containing the author's data transfer object.</returns>
    public async Task<AuthorDTO> FindSpecificAuthorById(string id)
    {
        return await _authorRepository.FindSpecificAuthorById(id);
    }

    /// <summary>
    /// Finds a specific author by their name.
    /// </summary>
    /// <param name="userName">The name of the author.</param>
    /// <returns>A task representing the asynchronous operation, containing the author's data transfer object.</returns>
    public async Task<AuthorDTO> FindSpecificAuthorByName(string userName)
    {
        return await _authorRepository.FindSpecificAuthorByName(userName);
    }

    /// <summary>
    /// Finds a specific author by their email address.
    /// </summary>
    /// <param name="email">The email address of the author.</param>
    /// <returns>A task representing the asynchronous operation, containing the author's data transfer object.</returns>
    public async Task<AuthorDTO> FindSpecificAuthorByEmail(string email)
    {
        return await _authorRepository.FindSpecificAuthorByEmail(email);
    }

    /// <summary>
    /// Gets the list of authors that a specific user follows.
    /// </summary>
    /// <param name="userName">The name of the user whose followees are being fetched.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of authors the user follows.</returns>
    public async Task<List<AuthorDTO>> GetFollowers(string userName)
    {
        return await _authorRepository.GetFollowing(userName);
    }

    /// <summary>
    /// Gets the list of authors that are followed by a specific user, identified by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user whose followees are being fetched.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of authors the user follows.</returns>
    public async Task<List<AuthorDTO>> GetFollowersbyId(string id)
    {
        return await _authorRepository.GetFollowingbyId(id);
    }

    /// <summary>
    /// Gets the list of authors who follow a specific user.
    /// </summary>
    /// <param name="userName">The name of the user whose followers are being fetched.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of authors who follow the user.</returns>
    public async Task<List<AuthorDTO>> GetFollowedby(string userName)
    {
        return await _authorRepository.GetFollowedby(userName);
    }

    /// <summary>
    /// Gets the list of authors who follow a specific user, identified by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user whose followers are being fetched.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of authors who follow the user.</returns>
    public async Task<List<AuthorDTO>> GetFollowedbybyId(string id)
    {
        return await _authorRepository.GetFollowedbybyId(id);
    }

    /// <summary>
    /// Adds a single author to the list of authors that the specified user will follow.
    /// </summary>
    /// <param name="id">The ID of the author who will follow.</param>
    /// <param name="followerId">The ID of the author to follow.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Follow(string id, string followerId)
    {
        await _authorRepository.AddFollower(id, followerId);
    }

    /// <summary>
    /// Removes a specific author from the list of authors that the specified user is following.
    /// </summary>
    /// <param name="id">The ID of the author who will stop following.</param>
    /// <param name="followerId">The ID of the author to unfollow.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Unfollow(string id, string followerId)
    {
        await _authorRepository.RemoveFollower(id, followerId);
    }

    /// <summary>
    /// Checks if the specified user is following the specified author.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="followerId">The ID of the author to check if is being followed.</param>
    /// <returns>A task representing the asynchronous operation, returning a boolean indicating whether the user is following the author.</returns>
    public async Task<bool> IsFollowing(string id, string followerId)
    {
        var author = await FindSpecificAuthorById(id);

        var list = await _authorRepository.GetFollowing(author.Name);

        foreach (var a in list)
        {
            if (a.Id == followerId)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the specified user has liked the given cheep.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="cheepId">The ID of the cheep.</param>
    /// <returns>A task representing the asynchronous operation, returning a boolean indicating whether the user has liked the cheep.</returns>
    public async Task<Boolean> HasLiked(string userName, int cheepId)
    {
        try
        {
            var author = await FindSpecificAuthorByName(userName);
            var likers = await _cheepRepository.GetAllLikers(cheepId);

            foreach (var a in likers)
            {
                if (a.Id == author.Id)
                    return true;
            }
            
            return false;
        }
        catch (NullReferenceException)
        {
            return false;
        }
    }

    /// <summary>
    /// Finds authors based on a partial user name, limiting the result to 5 authors.
    /// </summary>
    /// <param name="userName">The partial user name to search for.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of matching authors.</returns>
    public async Task<List<AuthorDTO>> FindAuthors(string userName)
    {
        return await _authorRepository.FindAuthors(userName, 5);
    }

    /// <summary>
    /// Deletes a user's data from the system, including their cheeps and author profile.
    /// </summary>
    /// <param name="userName">The name of the user to delete.</param>
    /// <returns>A task representing the asynchronous operation, returning true if successful.</returns>
    public async Task<bool> ForgetMe(string userName)
    {
        try
        {
            await _cheepRepository.RemoveCheepsFromUser(userName);
            await _authorRepository.RemoveAuthor(userName); // issue with removing author
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    /// <summary>
    /// Adds a like to a specific cheep by the specified author.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to like.</param>
    /// <param name="authorId">The ID of the author who is liking the cheep.</param>
    /// <returns>A task representing the asynchronous operation.</returns>    
    public async Task AddLike(int cheepId, string authorId)
    {
        await _cheepRepository.AddLike(cheepId, authorId);
        await _cheepRepository.RemoveDislike(cheepId, authorId);
    }

    /// <summary>
    /// Removes a like from a specific cheep by the specified author.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to remove the like from.</param>
    /// <param name="authorId">The ID of the author who is removing their like.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveLike(int cheepId, string authorId)
    {
        await _cheepRepository.RemoveLike(cheepId, authorId);
    }

    
    public async Task<List<AuthorDTO>> GetAllLikers(int cheepId)
    {
        return await _cheepRepository.GetAllLikers(cheepId);
    }

    public async Task<List<AuthorDTO>> GetAllDislikers(int cheepId)
    {
        return await _cheepRepository.GetAllDislikers(cheepId);
    }

    /// <summary>
    /// Adds a dislike to a specific cheep by the specified author.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to dislike.</param>
    /// <param name="authorId">The ID of the author who is disliking the cheep.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddDislike(int cheepId, string authorId)
    {
        await _cheepRepository.AddDisLike(cheepId, authorId);
        await _cheepRepository.RemoveLike(cheepId, authorId);
    }
    
    /// <summary>
    /// Removes a dislike from a specific cheep by the specified author.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to remove the dislike from.</param>
    /// <param name="authorId">The ID of the author who is removing their dislike.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveDislike(int cheepId, string authorId)
    {
        await _cheepRepository.RemoveDislike(cheepId, authorId);
    }

    /// <summary>
    /// Checks if the specified user has disliked the given cheep.
    /// </summary>
    /// <param name="userName">The name of the user.</param>
    /// <param name="cheepId">The ID of the cheep.</param>
    /// <returns>A task representing the asynchronous operation, returning a boolean indicating whether the user has disliked the cheep.</returns>
    public async Task<bool> HasDisliked(string userName, int cheepId)
    {
        try
        {
            var author = await FindSpecificAuthorByName(userName);
            var likers = await _cheepRepository.GetAllDislikers(cheepId);

            foreach (var a in likers)
            {
                if (a.Id == author.Id)
                    return true;
            }
            return false;

        }
        catch (NullReferenceException)
        {
            return false;
        }
    }

}