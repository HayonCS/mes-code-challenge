using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace ConfigApi
{
    class DatabaseUtility
    {
        /// <summary>
        /// Removes all whitespaces from a given string.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>New string with no whitespaces.</returns>
        static string CleanString(string str)
        {
            return Regex.Replace(str, @"\s+", "");
        }

        /// <summary>
        /// Executes a given Sqlite command on the database.
        /// </summary>
        /// <param name="str">Command string</param>
        public static void ExecuteCommand(string command)
        {
            using (var connection = new SqliteConnection("Data Source=database.db"))
            {
                connection.Open();
                SqliteCommand com = connection.CreateCommand();
                com.CommandText = command;
                com.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets deserialized value from the database based on a given key.
        /// </summary>
        /// <param name="key">Database key</param>
        /// <returns>Deserialized value.</returns>
        public static object? GetValueByKey(string key)
        {
            using (var connection = new SqliteConnection("Data Source=database.db"))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = string.Format(@"SELECT value FROM storage WHERE key = '{0}'", key);
                string? result = (string?)command.ExecuteScalar();
                if (result != null)
                {
                    return JsonSerializer.Deserialize<object>(result);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets an array of deserialized values from the database based on a given value keyword.
        /// </summary>
        /// <param name="str">Value keyword</param>
        /// <returns>Array of deserialized values.</returns>
        public static object[] GetValuesByKeyword(string keyword)
        {
            using (var connection = new SqliteConnection("Data Source=database.db"))
            {
                List<object> values = new List<object>();
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT value FROM storage;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? value = reader.GetString(0);
                        if (value != null && CleanString(value).ToLower().Contains(CleanString(keyword).ToLower()))
                        {
                            object? obj = JsonSerializer.Deserialize<object>(value);
                            if (obj != null) values.Add(obj);
                        }
                    }
                }
                return values.ToArray();
            }
        }

        /// <summary>
        /// Inserts a given key and value into the database. The value is serialized going into the database.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void InsertKeyValue(string key, object value)
        {
            string command = @"
                                INSERT INTO storage
                                VALUES ('{0}', '{1}');
                            ";
            string jsonValue = JsonSerializer.Serialize(value) ?? throw new JsonException("The provided 'value' is not valid JSON.");
            ExecuteCommand(string.Format(command, key, jsonValue));
        }

        /// <summary>
        /// Checks if a given key exists in the database.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True or false.</returns>
        public static bool KeyExists(string key)
        {
            object? foundValue = GetValueByKey(key);
            return foundValue != null;
        }
    }
}