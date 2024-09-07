namespace SimpleDB;

interface IDatabaseRepository<T>
{
    public List<T> Read(int? limit = null);
    public void Store(T record);
}