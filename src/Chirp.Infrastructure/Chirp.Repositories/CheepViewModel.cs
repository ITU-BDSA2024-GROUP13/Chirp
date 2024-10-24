namespace Chirp.Repositories;

public record CheepViewModel {
    public string Author {get;}
    public string Message {get;}
    public string Timestamp {get;}

    public CheepViewModel(string Author, string Message, string Timestamp){
        this.Author = Author;
        this.Message = Message;
        this.Timestamp = Timestamp;
    }
}