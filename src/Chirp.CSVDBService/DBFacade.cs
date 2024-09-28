namespace Chirp.CSVDBService
{
    using Chirp.CLI.Client;
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
        string sqlDBFilePath = "../../data/chirps.db";

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
        public void SELECT_MESSAGE_FROM_USER(string username)
        {
            var sqlQuery = $"SELECT U.username, M.text FROM user U, message M WHERE U.user_id = M.author_id AND U.username = '{username}';";

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
                        for (int i = 0; i < dataRecord.FieldCount; i++)
                            Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

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
                        for (int i = 0; i < dataRecord.FieldCount; i++)
                            Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

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
}
