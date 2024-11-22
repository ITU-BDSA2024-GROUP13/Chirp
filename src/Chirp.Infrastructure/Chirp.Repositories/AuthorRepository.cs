using Microsoft.EntityFrameworkCore;

namespace Chirp.Repositories;

using System.ComponentModel;
using Chirp.Core.DTO;
using Chirp.Core.Entities;

public class AuthorRepository : IAuthorRepository
{

    private readonly CheepDBContext _dbContext;
    public AuthorRepository(CheepDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateAuthor(AuthorDTO author)
    {
        Author newAuthor = new() { AuthorId = author.Id, Name = author.Name, Email = author.Email, 
        Cheeps = new List<Cheep>(), FollowedBy = new List<Author>(), Followers = new List<Author>()};
        var queryResult = await _dbContext.Authors.AddAsync(newAuthor); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        Console.WriteLine(queryResult.Entity.AuthorId);
        return queryResult.Entity.AuthorId;

    }

    public async Task<List<AuthorDTO>> FindAuthorByName(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.Name.StartsWith(userName))
        .Select(author => new AuthorDTO
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
            count = author.Cheeps.Count,
        });
        // Execute the query
        var result = await query.ToListAsync();

        /*
        for (int i = 0; i < result.Count; i++)
        {
            Console.WriteLine(result[i].Name);
            Console.WriteLine(result[i].count);

        }
        }*/

        return result;
    }

    public async Task<List<AuthorDTO>> FindAuthors(string userName, int amount)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Take(amount)
        .Where(author => author.Name.ToLower().Contains(userName.ToLower()))
        .Select(author => new AuthorDTO
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        });
        // Execute the query
        var result = await query.ToListAsync();

        /*
        for (int i = 0; i < result.Count; i++)
        {
            Console.WriteLine(result[i].Name);
        }*/

        return result;
    }

    public async Task<AuthorDTO> FindSpecificAuthorByName(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.Name == userName)
        .Select(author => new AuthorDTO
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        });
        // Execute the query
        var result = await query.ToListAsync();
        if (result.Any())
        {
            return result[0];
        }
        else
        {
            throw new NullReferenceException("No authors were found with this name");
        }
    }

    public async Task<AuthorDTO> FindSpecificAuthorById(int id)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.AuthorId == id)
        .Select(author => new AuthorDTO
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result[0];
    }

    public async Task<List<AuthorDTO>> GetFollowers(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.Name.Equals(userName))
        .Select(author => author.Followers);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.AuthorId, Email = i.Email, Name = i.Name }).ToList();
    }

 

    public async Task<List<AuthorDTO>> GetFollowersbyId(int id)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.AuthorId == id)
        .Select(author => author.Followers);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.AuthorId, Email = i.Email, Name = i.Name }).ToList();
    }

     
    public async Task<List<AuthorDTO>> GetFollowedby(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.Name.Equals(userName))
        .Select(author => author.FollowedBy);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.AuthorId, Email = i.Email, Name = i.Name }).ToList();    }
    
    public async Task<List<AuthorDTO>> GetFollowedbybyId(int id){
         var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.AuthorId == id)
        .Select(author => author.FollowedBy);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.AuthorId, Email = i.Email, Name = i.Name }).ToList();
    }


    public async Task AddFollower(int id, int followerId)
    {

        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.AuthorId == id);
        var follower = _dbContext.Authors.Include(p => p.FollowedBy).Single(e => e.AuthorId == followerId);

        author.Followers.Add(follower);


        _dbContext.Entry(author).CurrentValues.SetValues(author.Followers);


        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }
    

    public async Task RemoveFollower(int id, int followerId)
    {

        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.AuthorId == id);
        Author follower = _dbContext.Authors.Single(e => e.AuthorId == followerId);

        if (author.Followers.Any(f => f.AuthorId == follower.AuthorId))
        {
            author.Followers.Remove(follower);
            _dbContext.Entry(author).CurrentValues.SetValues(author.Followers);
        }
        else
        {
            throw new InvalidDataException("Cannot unfollow an author you don't already follow");
        }




        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task RemoveAllFollowers(int id)
    {
        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.AuthorId == id);

        author.Followers.Clear();

        _dbContext.Entry(author).CurrentValues.SetValues(author.Followers);


        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;    
    
    }

    public async Task RemoveAllFollowedby(int id)
    {
        Author author = _dbContext.Authors.Include(p => p.FollowedBy).Single(e => e.AuthorId == id);

        author.FollowedBy.Clear();

        _dbContext.Entry(author).CurrentValues.SetValues(author.FollowedBy);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;     
    }

    
    public async Task RemoveAuthor(int id)
    {
        Author author = _dbContext.Authors.Single(e => e.AuthorId == id);

        _dbContext.Remove(author);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task<List<AuthorDTO>> FindAuthorByEmail(string email)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Name)
        .Where(author => author.Email.StartsWith(email))
        .Select(author => new AuthorDTO
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

}