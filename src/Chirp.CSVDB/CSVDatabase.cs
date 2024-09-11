using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Chirp.CSVDB;

/**
 * <summary>
 * class <c>CSVDatabase</c> is responsible for directly communicating with
 * any stored data contained in a csv file.
 * Additionally, this class implements the <c>IDatabaseRepository</c> interface
 * </summary>
 */
public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string _filePath;

    /**
     * <summary>Constructor for <c>CSVDatabase</c>
     * <param name="filePath">The csv-file to communicate with</param>
     * </summary>
     * 
     */
    public CSVDatabase(string filePath)
    {
        _filePath = filePath;
    }
    

    /**
     * <summary>
     * <returns> A list of all records (limit for records is not implemented) </returns>
     * </summary>
     */
    public List<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<T>().ToList();
        return records;
    }

    /**
     * Stores a record or object, by writing into a csv file.
     * The csvConfiguration is set with <c>CultureInfo.InvariantCulture</c>>
     * in order to provide independency from the user's local settings.
     */
    public void Store(T record)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        using var stream = File.Open(_filePath, FileMode.Append);
        using var writer = new StreamWriter(stream);
        writer.WriteLine();
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecord(record);
        }
    }
}
