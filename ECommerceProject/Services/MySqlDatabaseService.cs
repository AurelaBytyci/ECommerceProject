using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceProject.Services
{
    public class MySqlDatabaseService
    {
        private readonly string _connectionString;

        public MySqlDatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<string>> GetDataAsync()
        {
            var data = new List<string>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("SELECT * FROM MyTable", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        data.Add(reader.GetString(0));
                    }
                }
            }

            return data;
        }
    }
}
