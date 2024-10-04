namespace Chirp.Razor;

public interface IMessageRepository
{
    public Task CreateMessage(newMessage MessageDTO);

    public Task<List<MessageDTO>> ReadMessages(string userName);

    public Task UpdateMessage(MessageDTO alteredMessage);


}