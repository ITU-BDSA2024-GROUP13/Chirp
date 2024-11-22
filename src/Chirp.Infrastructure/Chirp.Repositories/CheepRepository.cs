using Microsoft.EntityFrameworkCore;
using Chirp.Core.DTO;
using Chirp.Core.Entities;

namespace Chirp.Repositories;


public class CheepRepository(CheepDBContext dbContext) : ICheepRepository
{

    private readonly CheepDBContext _dbContext = dbContext;
    public async Task<int> CreateMessage(CheepDTO message)
    {

        Cheep newCheep = new() { Text = message.Text, AuthorId = message.AuthorId, TimeStamp = HelperFunctions.FromUnixTimeToDateTime(message.Timestamp) };
        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return queryResult.Entity.CheepId;
    }

    public async Task<List<CheepDTO>> ReadPublicMessages(int takeValue, int skipValue)
    {

        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.OrderByDescending(message => message.TimeStamp)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            AuthorId = message.AuthorId,
            Author = message.Author.Name,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds()
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

    public async Task<List<CheepDTO>> ReadUserMessages(string userName, int takeValue, int skipValue)
    {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.OrderByDescending(message => message.TimeStamp)
        .Where(message => message.Author.Name == userName)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            AuthorId = message.AuthorId,
            Author = message.Author.Name,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds()
        });
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, List<string> followers, int takeValue, int skipValue)
    {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.OrderByDescending(message => message.TimeStamp)
        .Where(message => message.Author.Name == userName || followers.Contains(message.Author.Name))
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            AuthorId = message.AuthorId,
            Author = message.Author.Name,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds()
        });
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }




    public async Task UpdateMessage(CheepDTO alteredMessage, int id)
    {

        var cheep = _dbContext.Cheeps.Single(e => e.CheepId == id);
        var entityEntry = _dbContext.Entry(cheep);
        _dbContext.Entry(cheep).CurrentValues.SetValues(alteredMessage);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    /// <summary>
    /// This is a method which is a query to get all the users of a given search. 
    /// It will only return the amount of users specified.
    /// </summary>
    /// <param name="searchValue"></param>
    /// <param name="amount"></param>
    /// <returns></returns>

    public async Task<List<AuthorDTO>> GetUsersOfSearch(string searchValue, int amount)
    {

        var query = _dbContext.Authors.
        Where(author => author.Name.ToLower().Contains(searchValue.ToLower()))
        .Take(amount)
        .Select(author => new AuthorDTO
        {
            Name = author.Name,
            Email = author.Email
        });

        var result = await query.ToListAsync();

        return result;
    }

    public async Task RemoveCheepsFromUser(string userName)
    {

        var cheeps = _dbContext.Cheeps.Where(c => c.Author.Name == userName);
        _dbContext.RemoveRange(cheeps);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;

    }
}