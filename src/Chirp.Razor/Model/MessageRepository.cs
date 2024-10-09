namespace Chirp.Razor.Model;

public class MessageRepository: IMessageRepository
{
    public Task CreateMessage(MessageDTO newMessage)
    {
        throw new NotImplementedException();
    }

    public Task<List<MessageDTO>> ReadMessages(string userName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateMessage(MessageDTO alteredMessage)
    {
        throw new NotImplementedException();
    }
}