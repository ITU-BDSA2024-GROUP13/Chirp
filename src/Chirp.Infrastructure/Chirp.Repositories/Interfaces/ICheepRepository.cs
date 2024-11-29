namespace Chirp.Repositories;
using Chirp.Core.DTO;


public interface ICheepRepository
{
    public Task<int> CreateMessage(NewCheepDTO newMessage);

    public Task<CheepDTO> FindSpecificCheepbyId(int id);

    public Task<List<CheepDTO>> ReadPublicMessages(int takeValue, int skipValue);

    public Task<List<CheepDTO>> ReadPublicMessagesbyOldest(int takeValue, int skipValue);

    public Task<List<CheepDTO>> ReadPublicMessagesbyMostLiked(int takeValue, int skipValue);

    public Task<List<CheepDTO>> ReadPublicMessagesbyRelevance(int takeValue, int skipValue);

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int takeValue, int skipValue);

    public Task UpdateMessage(UpdateCheepDTO alteredMessage, int id);

    public  Task AddLike(int cheepId, string authorId);

    public  Task AddDisLike(int cheepId, string authorId);

    public  Task<List<AuthorDTO>> GetAllLikers(int cheepId);
    public  Task<List<AuthorDTO>> GetAllDislikers(int cheepId);

    public  Task RemoveLike(int cheepId, string authorId);

    public  Task RemoveDislike(int cheepId, string authorId);

    public  Task RemoveAllLikes(int cheepId);

    public  Task RemoveAllDislikes(int cheepId);


    public Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, List<string> followers, int takeValue, int skipValue);

    public Task RemoveCheepsFromUser(string userName);


}