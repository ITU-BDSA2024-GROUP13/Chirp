using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string _filePath;

    public CSVDatabase(string filePath)
    {
        _filePath = filePath;
    }
    public List<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(_filePath);
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>().ToList();
            return records;
        }
        
    }

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
