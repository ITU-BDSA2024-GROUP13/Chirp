namespace Chirp.Services;

using Chirp.Core.DTO;

public interface ICheepService
{
    public Task<int> CreateMessage(CheepDTO newMessage);

    public Task<List<CheepDTO>> ReadPublicMessages(int page);

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page);

    public Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, int page);

    public Task<int> CountUserAndFollowerMessages(string userName);

    public Task<int> CountUserMessages(string userName);

    public Task<int> CountPublicMessages();

    public Task UpdateMessage(CheepDTO alteredMessage, int id);

    public Task<string> CreateAuthor(AuthorDTO newMessage);

    public Task<List<AuthorDTO>> FindAuthorByName(string userName);

    public Task<List<AuthorDTO>> FindAuthorByEmail(string email);

    public Task<AuthorDTO> FindSpecificAuthorById(string id);

    public Task<AuthorDTO> FindSpecificAuthorByName(string userName);

    public Task<AuthorDTO> FindSpecificAuthorByEmail(string email);

    public Task<List<AuthorDTO>> FindAuthors(string userName);


    public Task<List<AuthorDTO>> GetFollowers(string userName);

    public Task<List<AuthorDTO>> GetFollowersbyId(string id);

    public Task<List<AuthorDTO>> GetFollowedby(string userName);

    public Task<List<AuthorDTO>> GetFollowedbybyId(string id);


    public Task Follow(string id, string followerId);

    public Task Unfollow(string id, string followerId);

    public Task<Boolean> IsFollowing(string id, string followerId);

    public  Task AddLike(int cheepId, string authorId);

    public  Task RemoveLike(int cheepId, string authorId);


}