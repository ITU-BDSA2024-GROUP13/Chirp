namespace Chirp.Razor.Model;

public interface ICheepRepository
{
    public Task<int> CreateMessage(CheepDTO newMessage);

    public Task<List<CheepDTO>> ReadMessages(string userName);

    public Task UpdateMessage(CheepDTO alteredMessage, int id);



}