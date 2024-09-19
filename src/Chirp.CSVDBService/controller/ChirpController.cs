using Chirp.CLI.Client;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ChirpController : Controller
{
   private readonly ICSVService _csvService;

   public ChirpController(ICSVService csvService)
   {
       _csvService = csvService;
   }

   [HttpPost("read-cheeps-csv")]
   public async Task<IActionResult> GetCheepCSV([FromForm] IFormFileCollection file)
    {
        var cheeps = _csvService.ReadCSV<Cheep>(file[0].OpenReadStream());

        return Ok(cheeps);
    }
}