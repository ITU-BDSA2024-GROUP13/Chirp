using Chirp.Repositories;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Repositories;


public class AuthorRepository : IAuhtorRepository {

    private readonly CheepDBContext _dbContext;
    public AuthorRepository(CheepDBContext dbContext)
    {
        _dbContext = dbContext;
    }

     public Task<int> CreateAuthor(CheepDTO newMessage){
        return null;

     }

    public async Task<List<AuthorDTO>> FindAuthorByName(string userName){
        var query = _dbContext.Authors.OrderByDescending(author => author.Name)
        .Where(author => author.Name.StartsWith(userName))
        .Select(author => new AuthorDTO{ 
            Id = author.AuthorId,
            name = author.Name,
            email = author.Email,
            });
        // Execute the query
        var result = await query.ToListAsync();
        
        for (int i = 0; i < result.Count; i++)
        {
            Console.WriteLine(result[i]);
        }

        return result;
    }

    public Task<List<AuthorDTO>> FindAuthorByEmail(string email){
        return null;
    }



}