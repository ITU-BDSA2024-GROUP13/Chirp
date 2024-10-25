using Chirp.Repositories;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Repositories;


public class AuthorRepository : IAuhtorRepository {

     public Task<int> CreateAuthor(CheepDTO newMessage){
        return null;

     }

    public Task<List<AuthorDTO>> FindAuthorByName(string userName){
        return null;
    }

    public Task<List<AuthorDTO>> FindAuthorByEmail(string email){
        return null;
    }



}