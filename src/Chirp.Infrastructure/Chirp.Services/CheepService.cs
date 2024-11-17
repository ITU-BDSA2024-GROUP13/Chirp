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

    public async Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, int page){
        List<string> followers = AuthorToString(await _authorRepository.GetFollowers(userName));
        return await _cheepRepository.ReadUserAndFollowerMessages(userName, followers, 32, 32 * page);
    }

    private List<string> AuthorToString(List<AuthorDTO> authors){

        return authors.Select(a => a.Name).ToList();

    }

    public async Task<int> CreateMessage(CheepDTO message) {

       if (message.Text.Count() > 160){
            Console.WriteLine("Message is too long!");
            return 0;
        }

        List<AuthorDTO> authorsList = await FindAuthorByName(message.Author);

        if (authorsList.Any() && !authorsList[0].Equals(message.Author)){
            AuthorDTO newAuthor = new() {Name = message.Author, Email = message.Author + "@mail.com" };
            await CreateAuthor(newAuthor);
        } else if (!authorsList.Any()){
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

    public async Task<AuthorDTO> FindSpecificAuthorById(int id)
    {
        return await _authorRepository.FindSpecificAuthorById(id);
    }

    public async Task<AuthorDTO> FindSpecificAuthorByName(string userName)
    {
        return await _authorRepository.FindSpecificAuthorByName(userName);
    }

    public async Task<List<AuthorDTO>> GetFollowers(string userName){
        return (List<AuthorDTO>)await _authorRepository.GetFollowers(userName);
    }

        public async Task<List<AuthorDTO>> GetFollowersbyId(int id)
    {
        return (List<AuthorDTO>)await _authorRepository.GetFollowersbyId(id);
    }

    ///<summary>
    /// Adds a single author, which this author will follow
    ///</summary>
    ///<param name="id"> The author who follows the followerId</param>
    ///<param name="followerId"> The author who will be followed</param>
    public async Task Follow(int id, int followerId)
    {
        await _authorRepository.AddFollower(id, followerId);
    }

    public async Task Unfollow(int id, int followerId)
    {
        await _authorRepository.RemoveFollower(id, followerId);
    }

    public async Task<bool> IsFollowing(int id, int followerId) {
        var list = await _authorRepository.GetFollowers("Helge");
        foreach(var author in list){
            if(author.Id == followerId)
                return false;
        }

        return true;
    }


}
