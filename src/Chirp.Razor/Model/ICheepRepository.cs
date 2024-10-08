namespace Chirp.Razor.Model;

public interface ICheepRepository
{
    public Task<int> CreateMessage(MessageDTO newMessage);

    public Task<List<MessageDTO>> ReadMessages(string userName);

    public Task UpdateMessage(MessageDTO alteredMessage, int id);



}