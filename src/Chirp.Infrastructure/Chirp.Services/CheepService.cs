namespace Chirp.Services;

using Chirp.Repositories;

public class CheepService (ICheepRepository repository) : ICheepService
{
    private readonly ICheepRepository repository;

    public Task<List<CheepDTO>> ReadPublicMessages(int page) {
        return repository.ReadPublicMessages(32, 32*page);
    }

    public Task<List<CheepDTO>> ReadUserMessages(string userName, int page){
        return repository.ReadUserMessages(userName, 32, 32*page);
    }

    public async Task<int> CreateMessage(CheepDTO message) {
        return await repository.CreateMessage(message);
    }
    
    public async Task<int>  CountPublicMessages(){
        var list = await ReadPublicMessages(-1);
        var result = list.Count;
        return result;
    }

    public async Task<int> CountUserMessages(string userName){
        var list = await ReadUserMessages(userName, -1);
        var result = list.Count;
        return result;
    }

}
