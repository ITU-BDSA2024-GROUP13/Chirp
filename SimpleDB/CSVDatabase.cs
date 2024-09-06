namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string _filePath;

    public CSVDatabase(string filePath)
    {
        _filePath = filePath;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
        throw new NotImplementedException();
    }

    public void Store(T record)
    {
        throw new NotImplementedException();
    }
}
