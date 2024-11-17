namespace Chirp.Repositories;
using Chirp.Core.DTO;


public interface ICheepRepository
{
    public Task<int> CreateMessage(CheepDTO newMessage);

    public Task<List<CheepDTO>> ReadPublicMessages(int takeValue, int skipValue);

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int takeValue, int skipValue);

    public Task UpdateMessage(CheepDTO alteredMessage, int id);

    public Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, List<string> followers, int takeValue, int skipValue);


    public Task<List<AuthorDTO>> GetUsersOfSearch(string searchValue, int amount);
}