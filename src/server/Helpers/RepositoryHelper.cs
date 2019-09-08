using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace server.Helpers
{
    public static class RepositoryHelper
    {
        public static int Create(string tableName, Dictionary<string, object> fieldsToSave)
        {
            if (fieldsToSave == null)
            {
                return 0;
            }
            var query = $"INSERT INTO {tableName}";
            var columns = new string[fieldsToSave.Count];
            var values = new string[fieldsToSave.Count];
            int i = 0;
            foreach (var pair in fieldsToSave)
            {
                columns[i] = $"`{pair.Key}`";
                values[i] = $"'{pair.Value}'";
                i++;
            }
            query = $"{query} ({String.Join(" , ", columns)}) VALUES ({String.Join(", ", values)})";

            using (var connection = new MySqlConnection(Startup.ConnectionString))
            {
                connection.Open();

                var command = new MySqlCommand(query, connection);
                if (command.ExecuteNonQuery() == 0)
                {
                    return 0;
                }
                else
                {
                    return (int)command.LastInsertedId;
                }
            }
        }

        public static bool Delete(string tableName, int id)
        {
            using (var connection = new MySqlConnection(Startup.ConnectionString))
            {
                connection.Open();

                string idPlaceholder = "@id";
                string query = $"DELETE FROM {tableName} WHERE id = {idPlaceholder}";
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue(idPlaceholder, id);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static IList<List<object>> Get(string tableName)
        {
            var result = new List<List<object>>();
            try
            {
                using (var connection = new MySqlConnection(Startup.ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM {tableName}";
                    var command = new MySqlCommand(query, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader[i]);
                            }
                            result.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository Helper (Get): {ex.Message}");
            }
            return result;
        }

        public static IList<object> GetById(string tableName, int id)
        {
            var result = new List<object>();
            try
            {
                using (var connection = new MySqlConnection(Startup.ConnectionString))
                {
                    connection.Open();

                    string idPlaceholder = "@id";
                    string query = $"SELECT * FROM {tableName} WHERE id = {idPlaceholder}";
                    var command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue(idPlaceholder, id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result.Add(reader[i]);
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository Helper (GetById): {ex.Message}");
            }
            return result;
        }

        public static bool Update(string tableName, Dictionary<string, object> fieldsToUpdate, int id)
        {
            if (fieldsToUpdate == null)
            {
                return false;
            }
            var query = $"UPDATE {tableName} SET";
            var values = new List<string>();
            foreach (var pair in fieldsToUpdate)
            {
                switch (pair.Value)
                {
                    case string s:
                        values.Add($"{pair.Key} = '{pair.Value}'");
                        break;
                    default:
                        values.Add($"{pair.Key} = {pair.Value}");
                        break;
                }
            }
            query = $"{query} {String.Join(", ", values.ToArray())} WHERE id = {id}";

            using (var connection = new MySqlConnection(Startup.ConnectionString))
            {
                connection.Open();

                var command = new MySqlCommand(query, connection);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
