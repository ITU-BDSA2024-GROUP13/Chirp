namespace Chirp.Repositories;

/// <summary>
/// This is a author data transfer object that is used to transfer author data between the client and the server.
/// </summary>
public class AuthorDTO
{
   public int Id {get ; set; }
    public string name { get; set; }
    public string email { get; set; }   

}