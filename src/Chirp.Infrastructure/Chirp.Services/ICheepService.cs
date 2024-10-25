namespace Chirp.Services;

using Chirp.Repositories;

public interface ICheepService
{
    public Task<int> CreateMessage(CheepDTO newMessage);

    public Task<List<CheepDTO>> ReadPublicMessages(int page);

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page);

    public Task<int> CountUserMessages(string userName);

    public Task<int>  CountPublicMessages();

    public Task UpdateMessage(CheepDTO alteredMessage, int id);

    public Task<int> CreateAuthor(AuthorDTO newMessage);

    public Task<List<AuthorDTO>> FindAuthorByName(string userName);

    public Task<List<AuthorDTO>> FindAuthorByEmail(string email);
}