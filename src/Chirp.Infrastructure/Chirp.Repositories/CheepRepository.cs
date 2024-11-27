using Microsoft.EntityFrameworkCore;
using Chirp.Core.DTO;
using Chirp.Core.Entities;
using System.ComponentModel;

namespace Chirp.Repositories;


public class CheepRepository(CheepDBContext dbContext) : ICheepRepository
{

    private readonly CheepDBContext _dbContext = dbContext;
    public async Task<int> CreateMessage(CheepDTO message)
    {

        Cheep newCheep = new() { Likes = new List<Author>(), Text = message.Text, AuthorId = message.AuthorId, TimeStamp = HelperFunctions.FromUnixTimeToDateTime(message.Timestamp) };
        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return queryResult.Entity.CheepId;
    }

     public async Task<CheepDTO> FindSpecificCheepbyId(int id)
    {
        var query = _dbContext.Cheeps
        .Where(cheep => cheep.CheepId == id)
        .Select(cheep => new CheepDTO
        {
            Id = cheep.CheepId,
            AuthorId = cheep.AuthorId,
            Author = cheep.Author.UserName!,
            Text = cheep.Text,
            Timestamp = ((DateTimeOffset)cheep.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = cheep.Likes.Count

        });
        // Execute the query
        var result = await query.ToListAsync();

        return result[0];
    }

    public async Task<List<CheepDTO>> ReadPublicMessages(int takeValue, int skipValue)
    {

        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).OrderByDescending(message => message.TimeStamp)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            Id = message.CheepId,
            AuthorId = message.AuthorId,
            Author = message.Author.UserName!,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = message.Likes.Count
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

    public async Task<List<CheepDTO>> ReadUserMessages(string userName, int takeValue, int skipValue)
    {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).OrderByDescending(message => message.TimeStamp)
        .Where(message => message.Author.UserName == userName)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            Id = message.CheepId,
            AuthorId = message.AuthorId,
            Author = message.Author.UserName!,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = message.Likes.Count
            
        });
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, List<string> followers, int takeValue, int skipValue)
    {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).OrderByDescending(message => message.TimeStamp)
        .Where(message => message.Author.UserName == userName || followers.Contains(message.Author.UserName!))
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            Id = message.CheepId,
            AuthorId = message.AuthorId,
            Author = message.Author.UserName!,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = message.Likes.Count
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

    public async Task AddLike(int cheepId, string authorId)
    {

        Cheep cheep = _dbContext.Cheeps.Include(p => p.Likes).Single(e => e.CheepId == cheepId);
        var author = _dbContext.Authors.Include(p => p.LikedCheeps).Single(e => e.Id == authorId);

        if (!cheep.Likes.Contains(author)){
            cheep.Likes.Add(author);
        }

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Likes);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task RemoveLike(int cheepId, string authorId)
    {

        Cheep cheep = _dbContext.Cheeps.Include(p => p.Likes).Single(e => e.CheepId == cheepId);
        var author = _dbContext.Authors.Include(p => p.LikedCheeps).Single(e => e.Id == authorId);

        if (cheep.Likes.Contains(author)){
            cheep.Likes.Remove(author);
        }

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Likes);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task RemoveAllLikes(int cheepId)
    {

        Cheep cheep = _dbContext.Cheeps.Include(p => p.Likes).Single(e => e.CheepId == cheepId);

        cheep.Likes.Clear();

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Likes);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task<List<AuthorDTO>> GetAllLikers(int cheepId)
    {

        var query = _dbContext.Cheeps.Include(p => p.Likes)
        .Where(cheep => cheep.CheepId == cheepId)
        .Select(cheep => cheep.Likes);
        // Execute the query
        var result = await query.ToListAsync();
        if (result.Any()){
            return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
        } else{
            throw new NullReferenceException("There are no likes on this cheep");
        }

    }
    public async Task RemoveCheepsFromUser(string userName)
    {

        var cheeps = _dbContext.Cheeps.Where(c => c.Author.UserName == userName);
        _dbContext.RemoveRange(cheeps);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;

    }
}