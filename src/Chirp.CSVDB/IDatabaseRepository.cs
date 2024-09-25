using System.Collections.Generic;

namespace Chirp.CSVDB;

/**
 * Interface <c>IDatabaseRepository</c> helps ensure that, whatever implements it,
 * can <c>Read()</c> and <c>Store()</c> information into a database.
 */
public interface IDatabaseRepository<T>
{
    public List<T> Read(int? limit = null);
    public void Store(T record);
}