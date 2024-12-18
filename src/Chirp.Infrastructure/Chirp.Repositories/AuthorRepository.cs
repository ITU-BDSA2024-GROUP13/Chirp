using Microsoft.EntityFrameworkCore;
using Chirp.Core.Entities;
using Chirp.Core.DTO.AuthorDTO;

namespace Chirp.Infrastructure.Repositories;

/// <summary>
/// Repository class for managing Author data in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AuthorRepository"/> class.
/// </remarks>
/// <param name="dbContext">The database context used to interact with the database.</param>
public class AuthorRepository(CheepDBContext dbContext) : IAuthorRepository
{
    private readonly CheepDBContext _dbContext = dbContext;

    /// <summary>
    /// Creates a new author in the database.
    /// </summary>
    /// <param name="author">The author details.</param>
    /// <returns>The ID of the created author.</returns>
    public async Task<string> CreateAuthor(NewAuthorDTO author)
    {
        Author newAuthor = new()
        {
            DislikedCheeps = [],
            LikedCheeps = [],
            UserName = author.Name,
            Email = author.Email,
            Cheeps = [],
            FollowedBy = [],
            Followers = []
        };
        var queryResult = await _dbContext.Authors.AddAsync(newAuthor); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        Console.WriteLine(queryResult.Entity.Id);
        return queryResult.Entity.Id;

    }

    /// <summary>
    /// Finds authors by their username that starts with the given value.
    /// </summary>
    /// <param name="userName">The starting substring of the author's username.</param>
    /// <returns>A list of <see cref="AuthorDTO"/> objects.</returns>
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

    /// <summary>
    /// Finds authors based on a partial username match.
    /// </summary>
    /// <param name="userName">The substring to match in the author's username.</param>
    /// <param name="amount">The number of authors to return.</param>
    /// <returns>A list of <see cref="AuthorDTO"/> objects.</returns>
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

    /// <summary>
    /// Finds a specific author by their username.
    /// </summary>
    /// <param name="userName">The username of the author.</param>
    /// <returns>An <see cref="AuthorDTO"/> object representing the author.</returns>
    /// <exception cref="NullReferenceException">Thrown if no author is found with the specified username.</exception>
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

    /// <summary>
    /// Finds a specific author by their email.
    /// </summary>
    /// <param name="email">The email of the author.</param>
    /// <returns>An <see cref="AuthorDTO"/> object representing the author.</returns>
    /// <exception cref="NullReferenceException">Thrown if no author is found with the specified email.</exception>
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

    /// <summary>
    /// Finds a specific author by their ID.
    /// </summary>
    /// <param name="id">The ID of the author.</param>
    /// <returns>An <see cref="AuthorDTO"/> object representing the author.</returns>
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

    /// <summary>
    /// Retrieves a list of authors who are following a specific author by username.
    /// </summary>
    /// <param name="userName">The username of the author whose followers are being retrieved.</param>
    /// <returns>A list of <see cref="AuthorDTO"/> objects representing the followers.</returns>
    public async Task<List<AuthorDTO>> GetFollowing(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.UserName!.Equals(userName))
        .Select(author => author.Followers);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
    }

    /// <summary>
    /// Retrieves a list of authors who are following a specific author by their ID.
    /// </summary>
    /// <param name="id">The ID of the author whose followers are being retrieved.</param>
    /// <returns>A list of <see cref="AuthorDTO"/> objects representing the followers.</returns>
    public async Task<List<AuthorDTO>> GetFollowingbyId(string id)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.Id == id)
        .Select(author => author.Followers);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
    }

    /// <summary>
    /// Retrieves a list of authors who are followed by a specific author by username.
    /// </summary>
    /// <param name="userName">The username of the author whose followed authors are being retrieved.</param>
    /// <returns>A list of <see cref="AuthorDTO"/> objects representing the followed authors.</returns>
    public async Task<List<AuthorDTO>> GetFollowedby(string userName)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
        .Where(author => author.UserName!.Equals(userName))
        .Select(author => author.FollowedBy);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
    }

    /// <summary>
    /// Retrieves a list of authors who are followed by a specific author by their ID.
    /// </summary>
    /// <param name="id">The ID of the author whose followed authors are being retrieved.</param>
    /// <returns>A list of <see cref="AuthorDTO"/> objects representing the followed authors.</returns>
    public async Task<List<AuthorDTO>> GetFollowedbybyId(string id)
    {
        var query = _dbContext.Authors.OrderBy(author => author.UserName)
       .Where(author => author.Id == id)
       .Select(author => author.FollowedBy);
        // Execute the query
        var result = await query.ToListAsync();

        return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
    }

    /// <summary>
    /// Adds a follower to the specified author.
    /// </summary>
    /// <param name="followingId">The ID of the author who is following.</param>
    /// <param name="followedId">The ID of the author being followed.</param>
    public async Task AddFollower(string followingId, string followedId)
    {
        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.Id == followingId);
        var follower = _dbContext.Authors.Include(p => p.FollowedBy).Single(e => e.Id == followedId);

        author.Followers.Add(follower);

        _dbContext.Entry(author).CurrentValues.SetValues(author.Followers);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }

    /// <summary>
    /// Removes a follower from the specified author.
    /// </summary>
    /// <param name="followingId">The ID of the author who is following.</param>
    /// <param name="followedId">The ID of the author being unfollowed.</param>
    /// <exception cref="InvalidDataException">Thrown if the author is not already following the specified author.</exception>
    public async Task RemoveFollower(string followingId, string followedId)
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
    }

    /// <summary>
    /// Removes all followers of the specified author.
    /// </summary>
    /// <param name="id">The ID of the author whose followers will be removed.</param>
    public async Task RemoveAllFollowing(string id)
    {
        Author author = _dbContext.Authors.Include(p => p.Followers).Single(e => e.Id == id);

        author.Followers.Clear();

        _dbContext.Entry(author).CurrentValues.SetValues(author.Followers);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }


    /// <summary>
    /// Removes all followed-by authors from the specified author's list of followed-by authors.
    /// </summary>
    /// <param name="id">The ID of the author whose followed-by authors will be removed.</param>
    public async Task RemoveAllFollowedby(string id)
    {
        Author author = _dbContext.Authors.Include(p => p.FollowedBy).Single(e => e.Id == id);

        author.FollowedBy.Clear();

        _dbContext.Entry(author).CurrentValues.SetValues(author.FollowedBy);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }

    /// <summary>
    /// Removes an author from the database.
    /// </summary>
    /// <param name="userName">The username of the author to be removed.</param>
    public async Task RemoveAuthor(string userName)
    {
        Author author = _dbContext.Authors.Single(e => e.UserName == userName);

        _dbContext.Remove(author);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }

    /// <summary>
    /// Finds authors by their email that starts with the given value.
    /// </summary>
    /// <param name="email">The starting substring of the author's email.</param>
    /// <returns>A list of <see cref="AuthorDTO"/> objects.</returns>
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