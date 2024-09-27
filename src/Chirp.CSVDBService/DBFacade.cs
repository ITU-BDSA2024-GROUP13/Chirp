namespace Chirp.CSVDBService;

using Chirp.CLI.Client;
using System.Data;
using Microsoft.Data.Sqlite;


public class DBFacade
{
    string sqlDBFilePath = "../../data/chirps.db";

    public DBFacade()
    {
        SQLitePCL.Batteries.Init();
    }

    public bool INSERT_USER(int user_id, string username, string email, string pw_hash)
    {
        
        var sqlQuery = $"INSERT INTO user VALUES({user_id}, '{username}', '{email}', '{pw_hash}')";

        try{
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
            {

                
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                return true;
            }
        } 
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool INSERT_MESSAGE(int message_id, int auther_id, string text, int pub_date)
    {
        
        var sqlQuery = $"INSERT INTO message VALUES({message_id}, {auther_id}, '{text}', {pub_date})";

        try{
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
            {

                
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                return true;
            }
        } 
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    
    public void SELECT_MESSAGE_FROM_USER(string username)
    {
        
        var sqlQuery = $"SELECT U.username, M.text FROM user U, message M WHERE U.user_id = M.author_id AND U.username = '{username}';";

        try {
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                for (int i = 0; i < dataRecord.FieldCount; i++)
                    Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // for documentation on how to retrieve complete columns from query results
                Object[] values = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);
                for (int i = 0; i < fieldCount; i++)
                    Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
            }
        }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    
    public void ReadDatabase()
    {
        var sqlQuery = @"SELECT * FROM user";

        try {
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                for (int i = 0; i < dataRecord.FieldCount; i++)
                    Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // for documentation on how to retrieve complete columns from query results
                Object[] values = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);
                for (int i = 0; i < fieldCount; i++)
                    Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
            }
        }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}