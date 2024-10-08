namespace Chirp.Razor;

public interface IMessageRepository
{
    public Task CreateMessage(MessageDTO newMessage);

    public Task<List<MessageDTO>> ReadMessages(string userName);

    public Task UpdateMessage(MessageDTO alteredMessage);


}