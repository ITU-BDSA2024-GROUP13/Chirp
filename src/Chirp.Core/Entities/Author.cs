using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Author
{
    public required int AuthorId { get; set; }

    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Cheep> Cheeps { get; set; }

    public ICollection<Author> Followers { get; set; }

    public ICollection<Author> FollowedBy { get; set; }



    /*  public Author(){
          //Followers = new List<Author>();
         // Cheeps = new List<Cheep>();

      }*/



}