using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Author : IdentityUser
{

    //public required string Id { get; set; }

    //public required override string UserName { get; set; }
    
    //public required override string Email { get; set; }
    
    public required ICollection<Cheep> Cheeps { get; set; }

    public required ICollection<Author> Followers { get; set; }

    public required ICollection<Author> FollowedBy { get; set; }



    /*  public Author(){
          //Followers = new List<Author>();
         // Cheeps = new List<Cheep>();

      }*/



}