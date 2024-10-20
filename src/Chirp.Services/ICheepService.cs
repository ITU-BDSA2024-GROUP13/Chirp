namespace Chirp.Services;

using Chirp.Repositories;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);

    public int CountFromAuthor(string author);
    public int CountFromAll();

}