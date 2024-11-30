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

    public async Task<string> CreateAuthor(NewAuthorDTO author)
    {
        Author newAuthor = new() { DislikedCheeps = new List<Cheep>(), LikedCheeps = new List<Cheep>(), 
        UserName = author.Name, Email = author.Email, 
        Cheeps = new List<Cheep>(), FollowedBy = new List<Author>(), Followers = new List<Author>()};
        var queryResult = await _dbContext.Authors.AddAsync(newAuthor); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        Console.WriteLine(queryResult.Entity.Id);
        return queryResult.Entity.Id;

    }

    public async Task<List<AuthorDTO>> FindAuthorByName(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.UserName!.StartsWith(userName))
        .Select(author => new AuthorDTO
        {
            Id = author.Id,
            Name = author.UserName!,
            Email = author.Email!,
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

    public async Task<List<AuthorDTO>> FindAuthors(string userName, int amount)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.UserName!.ToLower().Contains(userName.ToLower()))
        .Take(amount)
        .Select(author => new AuthorDTO
        {
            Id = author.Id,
            Name = author.UserName!,
            Email = author.Email!,
        });
        // Execute the query
        var result = await query.ToListAsync();


        return result;
    }

    public async Task<AuthorDTO> FindSpecificAuthorByName(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.UserName == userName)
        .Select(author => new AuthorDTO
        {
            Id = author.Id,
            Name = author.UserName!,
            Email = author.Email!,
        });
        // Execute the query
        var result = await query.ToListAsync();
        if (result.Any())
        {
            return result[0];
        }
        else
        {
            throw new NullReferenceException("No authors were found with this name: " + userName);
        }
    }
       public async Task<AuthorDTO> FindSpecificAuthorByEmail(string email)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
                .Where(author => author.Email == email)
                .Select(author => new AuthorDTO
                {
                    Id = author.Id,
                    Name = author.UserName!,
                    Email = author.Email!,
                });
                // Execute the query
                var result = await query.ToListAsync();

        if (result.Any())
        {
            return result[0];
        }
        else
        {
            throw new NullReferenceException("No authors were found with this email");
        }    
        
    }

    public async Task<AuthorDTO> FindSpecificAuthorById(string id)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.Id == id)
        .Select(author => new AuthorDTO
        {
            Id = author.Id,
            Name = author.UserName!,
            Email = author.Email!,
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result[0];
    }

    public async Task<List<AuthorDTO>> GetFollowing(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.UserName!.Equals(userName))
        .Select(author => author.Followers);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
    }




 

    public async Task<List<AuthorDTO>> GetFollowingbyId(string id)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.Id == id)
        .Select(author => author.Followers);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
    }

     
    public async Task<List<AuthorDTO>> GetFollowedby(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.UserName!.Equals(userName))
        .Select(author => author.FollowedBy);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();    }
    
    public async Task<List<AuthorDTO>> GetFollowedbybyId(string id){
         var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.Id == id)
        .Select(author => author.FollowedBy);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
    }


    public async Task AddFollowing(string followingId, string followedId)
    {

        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.Id == followingId);
        var follower = _dbContext.Authors.Include(p => p.FollowedBy).Single(e => e.Id == followedId);

        author.Followers.Add(follower);


        _dbContext.Entry(author).CurrentValues.SetValues(author.Followers);


        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }
    

    public async Task RemoveFollowing(string followingId, string followedId)
    {

        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.Id == followingId);
        Author follower = _dbContext.Authors.Single(e => e.Id == followedId);

        if (author.Followers.Any(f => f.Id == follower.Id))
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

    public async Task RemoveAllFollowing(string id)
    {
        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.Id == id);

        author.Followers.Clear();

        _dbContext.Entry(author).CurrentValues.SetValues(author.Followers);


        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;    
    
    }


    public async Task RemoveAllLikedCheeps(string id)
    {
        Author author = _dbContext.Authors.Include(p => p.LikedCheeps).Single(e => e.Id == id);

        author.LikedCheeps.Clear();

        _dbContext.Entry(author).CurrentValues.SetValues(author.LikedCheeps);


        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;    
    
    }


    public async Task RemoveAllFollowedby(string id)
    {
        Author author = _dbContext.Authors.Include(p => p.FollowedBy).Single(e => e.Id == id);

        author.FollowedBy.Clear();

        _dbContext.Entry(author).CurrentValues.SetValues(author.FollowedBy);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;     
    }

    
    public async Task RemoveAuthor(string id)
    {
        Author author = _dbContext.Authors.Single(e => e.Id == id);

        _dbContext.Remove(author);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task<List<AuthorDTO>> FindAuthorByEmail(string email)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.Email!.StartsWith(email))
        .Select(author => new AuthorDTO
        {
            Id = author.Id,
            Name = author.UserName!,
            Email = author.Email!,
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

 
}