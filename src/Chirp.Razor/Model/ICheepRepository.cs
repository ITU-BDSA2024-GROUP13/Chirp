namespace Chirp.Razor.Model;

public interface ICheepRepository
{
    public Task<int> CreateMessage(CheepDTO newMessage);

    public Task<List<CheepDTO>> ReadPublicMessages(int page);

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page);

    public Task<int> CountUserMessages(string userName);

    public Task<int>  CountPublicMessages();



    public Task UpdateMessage(CheepDTO alteredMessage, int id);



}