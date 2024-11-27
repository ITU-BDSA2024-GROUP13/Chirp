namespace Chirp.Services;

using Chirp.Repositories;
using Chirp.Core.DTO;
using NuGet.Packaging.Rules;

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public Task<CheepDTO> FindSpecificCheepbyId(int cheepId)
    {
        return _cheepRepository.FindSpecificCheepbyId(cheepId);
    }

    public Task<List<CheepDTO>> ReadPublicMessages(int page)
    {
        return _cheepRepository.ReadPublicMessages(32, 32 * page);
    }

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page)
    {
        return _cheepRepository.ReadUserMessages(userName, 32, 32 * page);
    }

    public async Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, int page)
    {
        List<string> followers = AuthorToString(await _authorRepository.GetFollowers(userName));
        return await _cheepRepository.ReadUserAndFollowerMessages(userName, followers, 32, 32 * page);
    }

    private List<string> AuthorToString(List<AuthorDTO> authors)
    {

        return authors.Select(a => a.Name).ToList();

    }

    public async Task<int> CreateMessage(CheepDTO message)
    {

        if (message.Text.Count() > 160)
        {
            Console.WriteLine("Message is too long!");
            return 0;
        }
        try{

            AuthorDTO author = await FindSpecificAuthorByName(message.Author);

        } catch{

            AuthorDTO newAuthor = new() { Name = message.Author, Email = message.Author + "@mail.com" };
            await CreateAuthor(newAuthor);
            AuthorDTO createdAuthor = await FindSpecificAuthorByName(message.Author);
            message.AuthorId = createdAuthor.Id!;
        }

        return await _cheepRepository.CreateMessage(message);
    }

        public async Task<int> CountUserAndFollowerMessages(string userName)
    {
        List<string> followers = AuthorToString(await _authorRepository.GetFollowers(userName));

        var list = await _cheepRepository.ReadUserAndFollowerMessages(userName, followers, int.MaxValue, 0);
        var result = list.Count;
        return result;    
    }

    public async Task<int> CountPublicMessages()
    {

        var list = await _cheepRepository.ReadPublicMessages(int.MaxValue, 0);
        var result = list.Count;
        return result;
    }

    public async Task<int> CountUserMessages(string userName)
    {
        var list = await _cheepRepository.ReadUserMessages(userName, int.MaxValue, 0);
        var result = list.Count;
        return result;
    }

    public Task UpdateMessage(CheepDTO alteredMessage, int id)
    {
        return _cheepRepository.UpdateMessage(alteredMessage, id);
    }

    /// <summary>
    /// Deprecated! The functionality is now handled with Identity.
    /// 
    /// Creates a new author using a data transfer object.
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public async Task<string> CreateAuthor(AuthorDTO author)
    {
        return await _authorRepository.CreateAuthor(author);
    }
    /** 
    * <summary>
    * Returns a list of author which starts with the string specified.
    * If a user types in 'Hel', it would return authors with names such as 'Helge', 'Helge2' etc..
    * </summary>
    */
    public async Task<List<AuthorDTO>> FindAuthorByName(string userName)
    {
        return await _authorRepository.FindAuthorByName(userName);
    }

    public async Task<List<AuthorDTO>> FindAuthorByEmail(string email)
    {
        return await _authorRepository.FindAuthorByEmail(email);

    }

    public async Task<AuthorDTO> FindSpecificAuthorById(string id)
    {
        return await _authorRepository.FindSpecificAuthorById(id);
    }

    public async Task<AuthorDTO> FindSpecificAuthorByName(string userName)
    {
        return await _authorRepository.FindSpecificAuthorByName(userName);
    }

     public async Task<AuthorDTO> FindSpecificAuthorByEmail(string email)
    {
        return await _authorRepository.FindSpecificAuthorByEmail(email);
    }

    public async Task<List<AuthorDTO>> GetFollowers(string userName)
    {
        return (List<AuthorDTO>)await _authorRepository.GetFollowers(userName);
    }

    public async Task<List<AuthorDTO>> GetFollowersbyId(string id)
    {
        return (List<AuthorDTO>)await _authorRepository.GetFollowersbyId(id);
    }

    public async Task<List<AuthorDTO>> GetFollowedby(string userName)
    {
        return (List<AuthorDTO>)await _authorRepository.GetFollowedby(userName);
    }

    public async Task<List<AuthorDTO>> GetFollowedbybyId(string id)
    {
        return (List<AuthorDTO>)await _authorRepository.GetFollowedbybyId(id);
    }

    ///<summary>
    /// Adds a single author, which this author will follow
    ///</summary>
    ///<param name="id"> The author who follows the followerId</param>
    ///<param name="followerId"> The author who will be followed</param>
    public async Task Follow(string id, string followerId)
    {
        await _authorRepository.AddFollower(id, followerId);
    }

    public async Task Unfollow(string id, string followerId)
    {
        Console.WriteLine("UNFOLLOWING");
        await _authorRepository.RemoveFollower(id, followerId);
    }

    public async Task<bool> IsFollowing(string id, string followerId)
    {
        var author = await FindSpecificAuthorById(id);

        var list = await _authorRepository.GetFollowers(author.Name);

        foreach (var a in list)
        {
            if (a.Id == followerId)
                return true;
        }

        return false;
    }

    public async Task<bool> HasLiked(string authorId, int cheepId)
    {
        var author = await FindSpecificAuthorById(authorId);
        var cheep = await FindSpecificCheepbyId(cheepId);

        

        foreach (var a in list)
        {
            if (a.Id == followerId)
                return true;
        }

        return false;
    }

    public async Task<List<AuthorDTO>> FindAuthors(string userName)
    {
        return await _authorRepository.FindAuthors(userName, 5);
    }

    public async Task AddLike(int cheepId, string authorId)
    {
        await _cheepRepository.AddLike(cheepId, authorId);
    }

    public async Task RemoveLike(int cheepId, string authorId)
    {
        await _cheepRepository.RemoveLike(cheepId, authorId);
    }
}