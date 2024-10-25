using Chirp.Repositories;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Repositories;


public class AuthorRepository : IAuthorRepository {

    private readonly CheepDBContext _dbContext;
    public AuthorRepository(CheepDBContext dbContext)
    {
        _dbContext = dbContext;
    }

     public async Task<int> CreateAuthor(AuthorDTO author){
        Author newAuthor = new() {Name = author.name, Email = author.email};
        var queryResult = await _dbContext.Authors.AddAsync(newAuthor); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        Console.WriteLine(queryResult.Entity.AuthorId);
        return queryResult.Entity.AuthorId;

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
            Console.WriteLine(result[i].name);
        }

        return result;
    }

    public async Task<List<AuthorDTO>> FindAuthorByEmail(string email){
 var query = _dbContext.Authors.OrderByDescending(author => author.Name)
        .Where(author => author.Email.StartsWith(email))
        .Select(author => new AuthorDTO{ 
            Id = author.AuthorId,
            name = author.Name,
            email = author.Email,
            });
        // Execute the query
        var result = await query.ToListAsync();
        
        for (int i = 0; i < result.Count; i++)
        {
            Console.WriteLine(result[i].email);
        }

        return result;    
        }



}