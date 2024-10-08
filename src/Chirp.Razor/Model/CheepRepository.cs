using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Chirp.Razor.Model;


public class CheepRepository : ICheepRepository {

    private readonly CheepDBContext _dbContext;
    public CheepRepository(CheepDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateMessage(MessageDTO message){

        Cheep newCheep = new(message.text, message.timestamp);
        var queryResult = await _dbContext.cheeps.AddAsync(newCheep); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return queryResult.Entity.Id;
    }

    public async Task<List<MessageDTO>> ReadMessages(string userName){

        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.cheeps.Select(message => new { message.authorId, message.text });
        // Execute the query
        var result = await query.ToListAsync();


    }

    public Task UpdateMessage(MessageDTO alteredMessage){
    }



}