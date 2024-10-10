using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.Model;


public class CheepRepository : ICheepRepository {

    private readonly CheepDBContext _dbContext;
    public CheepRepository(CheepDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateMessage(CheepDTO message){

        Cheep newCheep = new(message.text, HelperFunctions.FromUnixTimeToDateTime(message.timestamp));
        var queryResult = await _dbContext.cheeps.AddAsync(newCheep); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return queryResult.Entity.Id;
    }

    public async Task<List<CheepDTO>> ReadMessages(string userName){

        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.cheeps.Select(message => new CheepDTO());
        // Execute the query
        var result = await query.ToListAsync();

        return result;


    }

    public async Task UpdateMessage(CheepDTO alteredMessage, int id){

        var cheep = _dbContext.cheeps.Single(e => e.Id == id);
        var entityEntry = _dbContext.Entry(cheep);
        _dbContext.Entry(cheep).CurrentValues.SetValues(alteredMessage);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;

        
    }



}