namespace Chirp.Infrastructure.Services;

using Chirp.Infrastructure.Repositories;
using Chirp.Core.DTO.CheepDTO;
using Chirp.Core.DTO.AuthorDTO;


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

    public Task<List<CheepDTO>> ReadPublicMessagesbyOldest(int page)
    {
        return _cheepRepository.ReadPublicMessagesbyOldest(32, 32 * page);
    }

    public Task<List<CheepDTO>> ReadPublicMessagesbyMostLiked(int page)
    {
        return _cheepRepository.ReadPublicMessagesbyMostLiked(32, 32 * page);
    }

    public Task<List<CheepDTO>> ReadPublicMessagesbyMostRelevance(int page, string userName)
    {
        return _cheepRepository.ReadPublicMessagesbyRelevance(32, 32 * page, userName);
    }

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page)
    {
        return _cheepRepository.ReadUserMessages(userName, 32, 32 * page);
    }

    public async Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, int page)
    {
        List<string> followers = AuthorToString(await _authorRepository.GetFollowing(userName));
        return await _cheepRepository.ReadUserAndFollowerMessages(userName, followers, 32, 32 * page);
    }

    private static List<string> AuthorToString(List<AuthorDTO> authors)
    {
        return authors.Select(a => a.Name).ToList();
    }

    public async Task<int> CreateMessage(NewCheepDTO message)
    {

        if (message.Text.Length > 160)
        {
            Console.WriteLine("Message is too long!");
            return 0;
        }
        try {
            AuthorDTO author = await FindSpecificAuthorByName(message.Author);} 
        catch {
            NewAuthorDTO newAuthor = new() { Name = message.Author, Email = message.Author + "@mail.com" };
            await CreateAuthor(newAuthor);
            AuthorDTO createdAuthor = await FindSpecificAuthorByName(message.Author);
            message.AuthorId = createdAuthor.Id!;
        }

        return await _cheepRepository.CreateMessage(message);
    }

    public async Task<int> CountUserAndFollowerMessages(string userName)
    {
        List<string> followers = AuthorToString(await _authorRepository.GetFollowing(userName));

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

    public Task UpdateMessage(UpdateCheepDTO alteredMessage, int id)
    {
        return _cheepRepository.UpdateMessage(alteredMessage, id);
    }

    public async Task<string> CreateAuthor(NewAuthorDTO author)
    {
        return await _authorRepository.CreateAuthor(author);
    }
    
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
        return await _authorRepository.GetFollowing(userName);
    }

    public async Task<List<AuthorDTO>> GetFollowersbyId(string id)
    {
        return await _authorRepository.GetFollowingbyId(id);
    }

    public async Task<List<AuthorDTO>> GetFollowedby(string userName)
    {
        return await _authorRepository.GetFollowedby(userName);
    }

    public async Task<List<AuthorDTO>> GetFollowedbybyId(string id)
    {
        return await _authorRepository.GetFollowedbybyId(id);
    }


    public async Task Follow(string id, string followerId)
    {
        await _authorRepository.AddFollower(id, followerId);
    }

    public async Task Unfollow(string id, string followerId)
    {
        await _authorRepository.RemoveFollower(id, followerId);
    }

    public async Task<bool> IsFollowing(string id, string followerId)
    {
        var author = await FindSpecificAuthorById(id);

        var list = await _authorRepository.GetFollowing(author.Name);

        foreach (var a in list)
        {
            if (a.Id == followerId)
                return true;
        }

        return false;
    }

    public async Task<Boolean> HasLiked(string userName, int cheepId)
    {
        try
        {
            var author = await FindSpecificAuthorByName(userName);
            var likers = await _cheepRepository.GetAllLikers(cheepId);

            foreach (var a in likers)
            {
                if (a.Id == author.Id)
                    return true;
            }
            
            return false;
        }
        catch (NullReferenceException)
        {
            return false;
        }
    }

    public async Task<List<AuthorDTO>> FindAuthors(string userName)
    {
        return await _authorRepository.FindAuthors(userName, 5);
    }

    public async Task<bool> ForgetMe(string userName)
    {
        try
        {
            await _cheepRepository.RemoveCheepsFromUser(userName);
            await _authorRepository.RemoveAuthor(userName); // issue with removing author
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }


    public async Task AddLike(int cheepId, string authorId)
    {
        await _cheepRepository.AddLike(cheepId, authorId);
        await _cheepRepository.RemoveDislike(cheepId, authorId);
    }

    public async Task RemoveLike(int cheepId, string authorId)
    {
        await _cheepRepository.RemoveLike(cheepId, authorId);
    }

    
    public async Task<List<AuthorDTO>> GetAllLikers(int cheepId)
    {
        return await _cheepRepository.GetAllLikers(cheepId);
    }

    public async Task<List<AuthorDTO>> GetAllDislikers(int cheepId)
    {
        return await _cheepRepository.GetAllDislikers(cheepId);
    }

    public async Task AddDislike(int cheepId, string authorId)
    {
        await _cheepRepository.AddDisLike(cheepId, authorId);
        await _cheepRepository.RemoveLike(cheepId, authorId);
    }
    
    public async Task RemoveDislike(int cheepId, string authorId)
    {
        await _cheepRepository.RemoveDislike(cheepId, authorId);
    }

    public async Task<bool> HasDisliked(string userName, int cheepId)
    {
        try
        {
            var author = await FindSpecificAuthorByName(userName);
            var likers = await _cheepRepository.GetAllDislikers(cheepId);

            foreach (var a in likers)
            {
                if (a.Id == author.Id)
                    return true;
            }
            return false;

        }
        catch (NullReferenceException)
        {
            return false;
        }
    }

}