namespace Chirp.Repositories;

using System.Data;
using Microsoft.Data.Sqlite;



/** 
* <summary>
* Facade for database operations related to users and messages in the Chirp application.
* Provides methods to insert and select data from the database.
* </summary>
*/ 
public class DBFacade
{
    /** 
    * <summary>
    * File path to the SQLite database.
    * </summary>
    */ 
    string sqlDBFilePath = "data/chirps.db";
    int page = 0;

    /** <summary>
    * Initializes a new instance of the DBFacade class and initializes SQLite batteries.
    * </summary>
    */ 
    public DBFacade()
    {
        SQLitePCL.Batteries.Init();
    }

    /** 
    * <summary>
    * Inserts a new user record into the 'user' table.
    * </summary>
    * <param name="user_id">The ID of the user.</param>
    * <param name="username">The username of the user.</param>
    * <param name="email">The email address of the user.</param>
    * <param name="pw_hash">The hashed password of the user.</param>
    * <returns>Returns true if the insertion was successful, false otherwise.</returns>
    */
    public bool INSERT_USER(int user_id, string username, string email, string pw_hash)
    {
        var sqlQuery = $"INSERT INTO user VALUES({user_id}, '{username}', '{email}', '{pw_hash}')";

        try
        {
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

    /** 
    * <summary>
    * Inserts a new message record into the 'message' table.
    * </summary>
    * <param name="message_id">The ID of the message.</param>
    * <param name="author_id">The ID of the author (user) who created the message.</param>
    * <param name="text">The text content of the message.</param>
    * <param name="pub_date">The publication date of the message as Unix timestamp.</param>
    * <returns>Returns true if the insertion was successful, false otherwise.</returns>
    */
    public bool INSERT_MESSAGE(int message_id, int author_id, string text, int pub_date)
    {
        var sqlQuery = $"INSERT INTO message VALUES({message_id}, {author_id}, '{text}', {pub_date})";


        try
        {
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

    /**
    * <summary>
    * Retrieves messages created by a specific user and prints the username and message text to the console.
    * </summary>
    * <param name="username">The username of the user whose messages you want to retrieve.</param>
    */
    public List<CheepViewModel> SELECT_MESSAGE_FROM_USER(string username, int page)
    {
        List<CheepViewModel> list = new();

        string author = "";
        string message = "";
        long timestamp = 0;
        string name = "";

        var sqlQuery = $"SELECT U.username, M.text, M.pub_date FROM message M, user U" + 
        $" WHERE M.author_id = U.user_id AND U.username = '{username}'" + 
        $" ORDER BY M.pub_date DESC LIMIT 32 OFFSET {page * 32}";
        try
        {
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dataRecord = (IDataRecord)reader;
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++){

                        name = reader.GetName(i);
                        if (values[i] != null){
                            switch(name){
                            case "username":
                                author =  (String) values[i];
                                break;

                            case "text":
                                message = (String) values[i];
                                break;
                            case "pub_date":
                                
                                timestamp = (long) values[i];
                                list.Add(new CheepViewModel(author, message, Convert.ToString(HelperFunctions.FromUnixTimeToDateTime(timestamp))));
                                break;
                            }
                        }


                    }
                        
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return list;

    }

    public int COUNT_MESSAGE_FROM_USER(string username){

        var sqlQuery = $"SELECT Count(*) FROM (SELECT U.username, M.text, M.pub_date FROM message M, user U" + 
        $" WHERE M.author_id = U.user_id AND U.username = '{username}')";
        int count = 0;

        try
        {
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dataRecord = (IDataRecord)reader;
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++){
                        //casting to long to cast to int32
                        count = (int)(long)values[i];
                        }
                }
                        
            }
        }
                catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return count;
        }
        

    public List<CheepViewModel> SELECT_ALL_MESSAGES(int page)
    {
        List<CheepViewModel> list = new();

        string author = "";
        string message = "";
        long timestamp = 0;
        string name = "";

        var sqlQuery = $"SELECT U.username, M.text, M.pub_date FROM message M, user U"
        + " WHERE M.author_id = U.user_id"
        + $" ORDER BY M.pub_date DESC LIMIT 32 OFFSET {page * 32}";
        try
        {
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dataRecord = (IDataRecord)reader;
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++){

                        name = reader.GetName(i);
                        if (values[i] != null){
                            switch(name){
                            case "username":
                                author =  (String) values[i];
                                break;

                            case "text":
                                message = (String) values[i];
                                break;
                            case "pub_date":
                                timestamp = (long) values[i];
                                list.Add(new CheepViewModel(author, message, Convert.ToString(HelperFunctions.FromUnixTimeToDateTime(timestamp))));
                                break;
                            }
                        }


                    }
                        
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return list;
    }

    public int COUNT_MESSAGE_FROM_ALL(){

        var sqlQuery = $"SELECT Count(*) FROM (SELECT * from message)";
        int count = 0;

        try
        {
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dataRecord = (IDataRecord)reader;
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++){
                        //casting to long to cast to int32
                        count = (int)(long)values[i];
                        }
                }
                        
            }
        }
                catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return count;
        }
    

    /**
        * <summary>
        * Reads and prints all data from the 'user' table.
        * </summary>
        */
    public void ReadDatabase()
    {
        var sqlQuery = @"SELECT * FROM user";

        try
        {
            using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dataRecord = (IDataRecord)reader;
                    //for (int i = 0; i < dataRecord.FieldCount; i++)
                        // Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    //for (int i = 0; i < fieldCount; i++)
                        // Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
