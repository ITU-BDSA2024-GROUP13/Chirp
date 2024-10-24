using Chirp.Repositories;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Repositories;


public class CheepRepository : ICheepRepository {

    private readonly CheepDBContext _dbContext;
    public CheepRepository(CheepDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateMessage(CheepDTO message){

        Cheep newCheep = new() {CheepId = 5000, Text = message.text, AuthorId = message.authorId, TimeStamp = HelperFunctions.FromUnixTimeToDateTime(message.timestamp)};
        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return queryResult.Entity.CheepId;
    }

    public async Task<List<CheepDTO>> ReadPublicMessages(int page){
        var takeValue = 32;
        var skipValue = 32 * page;
        if (page < 0){
            takeValue = int.MaxValue;
            skipValue = 0;
        }

        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.OrderByDescending(message => message.TimeStamp)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO{ 
            authorId = message.AuthorId,
            author = message.Author.Name,
            text = message.Text,
            timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds()
            });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

        public async Task<List<CheepDTO>> ReadUserMessages(string userName, int page){
            var takeValue = 32;
            var skipValue = 32 * page;
            if (page < 0){
                takeValue = int.MaxValue;
                skipValue = 0;
            }

            // Formulate the query - will be translated to SQL by EF Core
            var query = _dbContext.Cheeps.OrderByDescending(message => message.TimeStamp)
            .Where(message => message.Author.Name == userName)
            .Skip(skipValue)
            .Take(takeValue)
            .Select(message => new CheepDTO{ 
                authorId = message.AuthorId,
                author = message.Author.Name,
                text = message.Text,
                timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds()
                });
            // Execute the query
            var result = await query.ToListAsync();
            return result;
    }

    public async Task<int> CountUserMessages(string userName){
        var list = await ReadUserMessages(userName, -1);
        var result = list.Count;

        return result;
    }

    public async Task<int>  CountPublicMessages(){
        var list = await ReadPublicMessages(-1);
        var result = list.Count;
        return result;
    }

    public async Task UpdateMessage(CheepDTO alteredMessage, int id){

        var cheep = _dbContext.Cheeps.Single(e => e.CheepId == id);
        var entityEntry = _dbContext.Entry(cheep);
        _dbContext.Entry(cheep).CurrentValues.SetValues(alteredMessage);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;

        
    }



}