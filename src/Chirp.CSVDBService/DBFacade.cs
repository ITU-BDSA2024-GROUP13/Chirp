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

    /*
    public bool INSERT_message(int user_id, string username, string email, string pw_hash)
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


    */

    public void ReadDatabase(string username)
    {
        var sqlQuery = $"SELECT * FROM user WHERE username = '{username}'";

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