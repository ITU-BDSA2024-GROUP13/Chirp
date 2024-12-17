namespace Chirp.Infrastructure.Repositories;
using Chirp.Core.DTO.AuthorDTO;

/// <summary>
/// Defines the repository interface for performing CRUD and query operations on authors in the Chirp application.
/// Provides methods for managing authors, followers, and relationships between them.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Creates a new author in the system.
    /// </summary>
    /// <param name="newMessage">The data transfer object containing the details of the new author to be created.</param>
    /// <returns>A task representing the asynchronous operation, with the result being the ID of the created author.</returns>
    public Task<string> CreateAuthor(NewAuthorDTO newMessage);

    /// <summary>
    /// Finds authors by their username.
    /// </summary>
    /// <param name="userName">The username to search for.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors that match the given username.</returns>
    public Task<List<AuthorDTO>> FindAuthorByName(string userName);

    /// <summary>
    /// Finds authors by their email.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors that match the given email.</returns>
    public Task<List<AuthorDTO>> FindAuthorByEmail(string email);

    /// <summary>
    /// Finds a specific author by their username.
    /// </summary>
    /// <param name="userName">The username of the author to search for.</param>
    /// <returns>A task representing the asynchronous operation, with the specific author matching the given username.</returns>
    public Task<AuthorDTO> FindSpecificAuthorByName(string userName);

    /// <summary>
    /// Finds a specific author by their email.
    /// </summary>
    /// <param name="email">The email address of the author to search for.</param>
    /// <returns>A task representing the asynchronous operation, with the specific author matching the given email.</returns>
    public Task<AuthorDTO> FindSpecificAuthorByEmail(string email);

    /// <summary>
    /// Finds a specific author by their ID.
    /// </summary>
    /// <param name="id">The ID of the author to search for.</param>
    /// <returns>A task representing the asynchronous operation, with the specific author matching the given ID.</returns>
    public Task<AuthorDTO> FindSpecificAuthorById(string id);

    /// <summary>
    /// Gets a list of authors that the given author is following, identified by their username.
    /// </summary>
    /// <param name="userName">The username of the author whose following list is to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors that the specified author is following.</returns>
    public Task<List<AuthorDTO>> GetFollowing(string userName);

    /// <summary>
    /// Gets a list of authors that the given author is following, identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the author whose following list is to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors that the specified author is following.</returns>
    public Task<List<AuthorDTO>> GetFollowingbyId(string id);

    /// <summary>
    /// Gets a list of authors who are following the given author, identified by their username.
    /// </summary>
    /// <param name="userName">The username of the author whose followers list is to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors who are following the specified author.</returns>
    public Task<List<AuthorDTO>> GetFollowedby(string userName);

    /// <summary>
    /// Gets a list of authors who are following the given author, identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the author whose followers list is to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors who are following the specified author.</returns>
    public Task<List<AuthorDTO>> GetFollowedbybyId(string id);

    /// <summary>
    /// Adds a new follower to an author.
    /// </summary>
    /// <param name="id">The ID of the author to whom a follower is being added.</param>
    /// <param name="followerId">The ID of the follower being added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task AddFollower(string id, string followerId);

    /// <summary>
    /// Removes a follower from an author.
    /// </summary>
    /// <param name="id">The ID of the author from whom a follower is being removed.</param>
    /// <param name="followerId">The ID of the follower to be removed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveFollower(string id, string followerId);

    /// <summary>
    /// Finds a list of authors by their username, limited to a specific number of results.
    /// </summary>
    /// <param name="userName">The username to search for.</param>
    /// <param name="amount">The number of results to return.</param>
    /// <returns>A task representing the asynchronous operation, with a list of authors matching the username.</returns>
    public Task<List<AuthorDTO>> FindAuthors(string userName, int amount);

    /// <summary>
    /// Removes all authors being followed by a specific author.
    /// </summary>
    /// <param name="id">The ID of the author whose followings should be removed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveAllFollowing(string id);


    /// <summary>
    /// Removes all authors who are following a specific author.
    /// </summary>
    /// <param name="id">The ID of the author whose followers should be removed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveAllFollowedby(string id);

    /// <summary>
    /// Removes an author from the system.
    /// </summary>
    /// <param name="id">The ID of the author to be removed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RemoveAuthor(string id);
}
