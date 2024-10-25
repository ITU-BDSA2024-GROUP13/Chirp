namespace Chirp.Repositories;

public interface IAuhtorRepository
{
    public Task<int> CreateAuthor(CheepDTO newMessage);

    public Task<List<AuthorDTO>> FindAuthorByName(string userName);

    public Task<List<AuthorDTO>> FindAuthorByEmail(string email);

}