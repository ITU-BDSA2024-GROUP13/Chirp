using Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;


namespace Chirp.Web.models;

public class CheepPartialView()
{
    public required int? Id { get; set; }
    public required string Author { get; set; }
    public required string Body { get; set; }
    public required bool isLiked { get; set; }
    public required long Date { get; set; }
    public required int Like { get; set; }
    public DateTime ToDateTime(long value)
    {
        return Repositories.HelperFunctions.FromUnixTimeToDateTime(value);
    }

    
}