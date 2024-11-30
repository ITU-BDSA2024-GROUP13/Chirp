namespace Chirp.Services;

using Chirp.Core.DTO;

public interface ICheepService
{

    public Task<CheepDTO> FindSpecificCheepbyId(int cheepId);

    public Task<int> CreateMessage(NewCheepDTO newMessage);

    public Task<List<CheepDTO>> ReadPublicMessages(int page);

    public Task<List<CheepDTO>> ReadPublicMessagesbyOldest(int page);

    public Task<List<CheepDTO>> ReadPublicMessagesbyMostLiked(int page);

    public Task<List<CheepDTO>> ReadPublicMessagesbyMostRelevance(int page, string userName);

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page);

    public Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, int page);

    public Task<int> CountUserAndFollowerMessages(string userName);

    public Task<int> CountUserMessages(string userName);

    public Task<int> CountPublicMessages();

    public Task UpdateMessage(UpdateCheepDTO alteredMessage, int id);

    public Task<string> CreateAuthor(NewAuthorDTO newMessage);

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

    public Task<Boolean> HasLiked(string userName, int cheepId);

    public  Task AddDislike(int cheepId, string authorId);

    public  Task RemoveDislike(int cheepId, string authorId);

    public Task<Boolean> HasDisliked(string userName, int cheepId);



}