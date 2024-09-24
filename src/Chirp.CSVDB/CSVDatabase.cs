using System.Globalization;
namespace Chirp.CSVDB;

using CsvHelper;
using CsvHelper.Configuration;


/**
 * <summary>
 * class <c>CSVDatabase</c> is responsible for directly communicating with
 * any stored data contained in a csv file.
 * Additionally, this class implements the <c>IDatabaseRepository</c> interface
 * </summary>
 */
public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static string alternative = "../../../../../../data/chirp_cli_db.csv";
    private static string filePath = "./data/chirp_cli_db.csv";

    private readonly string _filePath;

    // Singleton for the database
    private static readonly IDatabaseRepository<T> Database = new CSVDatabase<T>(filePath);

    /**
     * <summary>
     * Constructor for <c>CSVDatabase</c>. Set to private to ensure no other instance
     * <param name="filePath">The csv-file to communicate with</param>
     * </summary>
     */
    private CSVDatabase(string filePath)
    {
        if (File.Exists(filePath))
        {
            _filePath = filePath ?? throw new FileNotFoundException(nameof(filePath));
        }
        else
        {
            _filePath = alternative;
        }
    }

    /**
     * <summary>
     * A getter method for the singleton of the Database
     * </summary>
     */
    public static IDatabaseRepository<T> GetDatabase()
    {
        return Database;
    }


    /**
     * <returns> A list of all records (limit for records is not implemented) </returns>
     */
    public List<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<T>().ToList();
        return records;
    }

    /**
     * <summary>
     * Stores a record or object, by writing into a csv file.
     * The csvConfiguration is set with <c>CultureInfo.InvariantCulture</c>>
     * in order to provide independency from the user's local settings.
     * </summary>
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