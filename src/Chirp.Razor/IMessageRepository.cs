namespace Chirp.Razor;

public interface IMessageRepository
{
    public Task CreateMessage(MessageDTO);
}