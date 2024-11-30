namespace Chirp.Repositories;
using Chirp.Core.DTO;

public interface IAuthorRepository
{
    public Task<string> CreateAuthor(NewAuthorDTO newMessage);

    public Task<List<AuthorDTO>> FindAuthorByName(string userName);

    public Task<List<AuthorDTO>> FindAuthorByEmail(string email);

    public Task<AuthorDTO> FindSpecificAuthorByName(string userName);

    public Task<AuthorDTO> FindSpecificAuthorByEmail(string email);


    public Task<AuthorDTO> FindSpecificAuthorById(string id);


    public Task<List<AuthorDTO>> GetFollowing(string userName);

    public Task<List<AuthorDTO>> GetFollowingbyId(string id);

    public Task<List<AuthorDTO>> GetFollowedby(string userName);

    public Task<List<AuthorDTO>> GetFollowedbybyId(string id);

    public Task AddFollowing(string followingId, string followedId);
    public Task RemoveFollowing(string followingId, string followedId);
    public Task<List<AuthorDTO>> FindAuthors(string userName, int amount);

    public Task RemoveAllFollowing(string id);

    public Task RemoveAllLikedCheeps(string id);
    public Task RemoveAllFollowedby(string id);
    public Task RemoveAuthor(string id);

}