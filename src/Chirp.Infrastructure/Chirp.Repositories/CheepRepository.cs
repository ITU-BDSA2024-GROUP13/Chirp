using Microsoft.EntityFrameworkCore;
using Chirp.Core.DTO;
using Chirp.Core.Entities;
using System.ComponentModel;
using Humanizer;
using System.Diagnostics;

namespace Chirp.Repositories;


public class CheepRepository(CheepDBContext dbContext) : ICheepRepository
{

    private readonly CheepDBContext _dbContext = dbContext;
    private double _totalLikes = 0;

    public async Task<int> CreateMessage(NewCheepDTO message)
    {

        Cheep newCheep = new()
        {
            Dislikes = [],
            Likes = [],
            Text = message.Text,
            AuthorId = message.AuthorId,
            TimeStamp = HelperFunctions.FromUnixTimeToDateTime(message.Timestamp)
        };
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
            Likes = cheep.Likes.Count,
            Dislikes = cheep.Dislikes.Count
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result[0];
    }

    public async Task<List<CheepDTO>> ReadPublicMessages(int takeValue, int skipValue)
    {

        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).Include(p => p.Dislikes).OrderByDescending(message => message.TimeStamp)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            Id = message.CheepId,
            AuthorId = message.AuthorId,
            Author = message.Author.UserName!,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = message.Likes.Count,
            Dislikes = message.Dislikes.Count
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

    public async Task<List<CheepDTO>> ReadPublicMessagesbyOldest(int takeValue, int skipValue)
    {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).Include(p => p.Dislikes).OrderBy(message => message.TimeStamp)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            Id = message.CheepId,
            AuthorId = message.AuthorId,
            Author = message.Author.UserName!,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = message.Likes.Count,
            Dislikes = message.Dislikes.Count
        });
        // Execute the query
        var result = await query.ToListAsync();

        return result;
    }

    public async Task<List<CheepDTO>> ReadPublicMessagesbyMostLiked(int takeValue, int skipValue)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).Include(p => p.Dislikes)
        .OrderByDescending(message => message.Likes.Count)
        .ThenByDescending(message => message.TimeStamp)
        .Skip(skipValue)
        .Take(takeValue)
        .Select(message => new CheepDTO
        {
            Id = message.CheepId,
            AuthorId = message.AuthorId,
            Author = message.Author.UserName!,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = message.Likes.Count,
            Dislikes = message.Dislikes.Count
        });
        // Execute the query
        var result = await query.ToListAsync();

        watch.Stop();
        Console.WriteLine(watch.Elapsed.Milliseconds);

        return result;
    }

    public async Task<List<CheepDTO>> ReadPublicMessagesbyRelevance(int takeValue, int skipValue, string userName)
    {

        var author = _dbContext.Authors.Single(e => e.UserName == userName);

        var query1 = _dbContext.Cheeps.Include(p => p.Likes).Include(p => p.Dislikes).Include(p => p.Author.FollowedBy)
        .Select(message => new CheepDTOForRelevance
        {
            Id = message.CheepId,
            Author = message.Author.UserName!,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Dislikes = message.Dislikes.Count,
            LocalLikeRatio = message.LocalLikeRatio, 
            isFollowing = message.Author.FollowedBy.Contains(author),
            isDisliked = message.Dislikes.Contains(author)

        })
        .ToListAsync();

        var list = await query1;


        Dictionary<int, double> relevanceMap = new Dictionary<int, double>();

        foreach (var cheep in list)
        {

            var points = await RelevancePoints(cheep.Id, cheep.Author!,
            userName, cheep.LocalLikeRatio, HelperFunctions.FromUnixTimeToDateTime(cheep.Timestamp),
            cheep.isFollowing, cheep.isDisliked);

            relevanceMap.Add(cheep.Id, points);
        }

        list.OrderByDescending(message => relevanceMap[message.Id]);

        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).Include(p => p.Dislikes)
        .Select(message => new CheepDTO
        {
            Id = message.CheepId,
            AuthorId = message.AuthorId,
            Author = message.Author.UserName!,
            Text = message.Text,
            Timestamp = ((DateTimeOffset)message.TimeStamp).ToUnixTimeMilliseconds(),
            Likes = message.Likes.Count,
            Dislikes = message.Dislikes.Count
        })
        .ToListAsync();
        // Execute the query
        var result = await query;

        var something = result.OrderByDescending(
          message => relevanceMap[message.Id])
        .Skip(skipValue)
        .Take(takeValue)
        .ToList();

        return something;
    }


    public async Task<double> RelevancePoints(int cheepid, string follower, string userName, double likeRatio, DateTime timeStamp, 
    bool follows, bool disliked)
    {
        var followsPoints = 0;
        var dislikedPoints = 0;
        if (follows){
            followsPoints = 24;
        }
        if (disliked){
            dislikedPoints = -24;
        }

        return likeRatio - (DateTime.UtcNow - timeStamp).TotalHours 
        + followsPoints
        + dislikedPoints;
    }

    public async Task<List<CheepDTO>> ReadUserMessages(string userName, int takeValue, int skipValue)
    {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).Include(p => p.Dislikes).OrderByDescending(message => message.TimeStamp)
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
            Likes = message.Likes.Count,
            Dislikes = message.Dislikes.Count

        });
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDTO>> ReadUserAndFollowerMessages(string userName, List<string> followers, int takeValue, int skipValue)
    {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Include(p => p.Likes).Include(p => p.Dislikes).OrderByDescending(message => message.TimeStamp)
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
            Likes = message.Likes.Count,
            Dislikes = message.Dislikes.Count
        });
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }

    public async Task UpdateMessage(UpdateCheepDTO alteredMessage, int id)
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

        if (!cheep.Likes.Contains(author))
        {
            cheep.Likes.Add(author);
            _totalLikes++;
            if (cheep.Likes.Any())
            {
                cheep.LocalLikeRatio = (float)Math.Log((double)cheep.Likes.Count + 1, 5);
            }
            else
            {
                cheep.LocalLikeRatio = 0;
            }
        }

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Likes);
        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.LocalLikeRatio);

        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task RemoveLike(int cheepId, string authorId)
    {

        Cheep cheep = _dbContext.Cheeps.Include(p => p.Likes).Single(e => e.CheepId == cheepId);
        var author = _dbContext.Authors.Include(p => p.LikedCheeps).Single(e => e.Id == authorId);

        if (cheep.Likes.Contains(author))
        {
            cheep.Likes.Remove(author);
            _totalLikes--;
            if (cheep.Likes.Any())
            {
                cheep.LocalLikeRatio = (float)Math.Log((double)cheep.Likes.Count + 1, 5);
            }
            else
            {
                cheep.LocalLikeRatio = 0;
            }
        }

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Likes);
        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.LocalLikeRatio);

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
        if (result.Any())
        {
            return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
        }
        else
        {
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

    public async Task AddDisLike(int cheepId, string authorId)
    {
        Cheep cheep = _dbContext.Cheeps.Include(p => p.Dislikes).Single(e => e.CheepId == cheepId);
        var author = _dbContext.Authors.Include(p => p.DislikedCheeps).Single(e => e.Id == authorId);

        if (!cheep.Dislikes.Contains(author))
        {
            cheep.Dislikes.Add(author);
        }

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Dislikes);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task<List<AuthorDTO>> GetAllDislikers(int cheepId)
    {
        var query = _dbContext.Cheeps.Include(p => p.Dislikes)
        .Where(cheep => cheep.CheepId == cheepId)
        .Select(cheep => cheep.Dislikes);
        // Execute the query
        var result = await query.ToListAsync();
        if (result.Any())
        {
            return result[0].Select(i => new AuthorDTO() { Id = i.Id, Email = i.Email!, Name = i.UserName! }).ToList();
        }
        else
        {
            throw new NullReferenceException("There are no likes on this cheep");
        }

    }

    public async Task RemoveDislike(int cheepId, string authorId)
    {
        Cheep cheep = _dbContext.Cheeps.Include(p => p.Dislikes).Single(e => e.CheepId == cheepId);
        var author = _dbContext.Authors.Include(p => p.DislikedCheeps).Single(e => e.Id == authorId);

        if (cheep.Dislikes.Contains(author))
        {
            cheep.Dislikes.Remove(author);
        }

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Dislikes);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }

    public async Task RemoveAllDislikes(int cheepId)
    {
        Cheep cheep = _dbContext.Cheeps.Include(p => p.Dislikes).Single(e => e.CheepId == cheepId);

        cheep.Dislikes.Clear();

        _dbContext.Entry(cheep).CurrentValues.SetValues(cheep.Dislikes);
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
        return;
    }


}