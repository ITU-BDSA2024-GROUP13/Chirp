namespace Chirp.Razor;

public interface IMessageRepository
{
    public Task CreateMessage(MessageDTO);

    public Task<List<MessageDTO>> ReadMessages(string userName);
    
    
}