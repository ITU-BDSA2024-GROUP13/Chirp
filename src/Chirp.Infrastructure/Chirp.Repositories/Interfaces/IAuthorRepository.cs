namespace Chirp.Repositories;
using Chirp.Core.DTO;

public interface IAuthorRepository
{
    public Task<int> CreateAuthor(AuthorDTO newMessage);

    public Task<List<AuthorDTO>> FindAuthorByName(string userName);

    public Task<List<AuthorDTO>> FindAuthorByEmail(string email);

    public Task<AuthorDTO> FindSpecificAuthorByName(string userName);

    public Task<List<AuthorDTO>> GetFollowers(string userName);

    public Task AddFollower(int id, int followerId);
    public Task RemoveFollower(int id, int followerId);




    public Task<List<AuthorDTO>> FindAuthors(string userName, int amount);

}