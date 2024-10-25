namespace Chirp.Services;

using Chirp.Repositories;
using Chirp.Core.DTO;


public class CheepService  : ICheepService
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository){
        _cheepRepository = cheepRepository ?? throw new ArgumentNullException();
        _authorRepository = authorRepository ?? throw new ArgumentNullException();
    }

    public Task<List<CheepDTO>> ReadPublicMessages(int page) {
        return _cheepRepository.ReadPublicMessages(32, 32*page);
    }

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page){
        return _cheepRepository.ReadUserMessages(userName, 32, 32*page);
    }

    public async Task<int> CreateMessage(CheepDTO message) {

        List<AuthorDTO> authorsList = await FindAuthorByName(message.Author);

        if (authorsList.Any() && !authorsList[0].Equals(message.Author)){
            AuthorDTO newAuthor = new() {Name = message.Author, Email = message.Author + "@mail.com" };
            await CreateAuthor(newAuthor);
        } else{
            AuthorDTO newAuthor = new() {Name = message.Author, Email = message.Author + "@mail.com" };
            await CreateAuthor(newAuthor);
        }

        return await _cheepRepository.CreateMessage(message);
    }
    
    public async Task<int>  CountPublicMessages(){
        var list = await _cheepRepository.ReadPublicMessages(int.MaxValue, 0);
        var result = list.Count;
        return result;
    }

    public async Task<int> CountUserMessages(string userName){
        var list = await _cheepRepository.ReadUserMessages(userName, int.MaxValue, 0);
        var result = list.Count;
        return result;
    }

    public Task UpdateMessage(CheepDTO alteredMessage, int id){
        return _cheepRepository.UpdateMessage(alteredMessage, id);
    }

    public async Task<int> CreateAuthor(AuthorDTO author){
        return await _authorRepository.CreateAuthor(author);
    }
    /** 
    * <summary>
    * Returns a list of author which starts with the string specified.
    * If a user types in 'Hel', it would return authors with names such as 'Helge', 'Helge2' etc..
    * </summary>
    */
    public async Task<List<AuthorDTO>> FindAuthorByName(string userName){
        return await _authorRepository.FindAuthorByName(userName);
    }

    public async Task<List<AuthorDTO>> FindAuthorByEmail(string email){
        return await _authorRepository.FindAuthorByEmail(email);

    }

}
