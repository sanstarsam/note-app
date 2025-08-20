using Dapper;
using Microsoft.Data.SqlClient;
using NoteApp.Data;
using NoteApp.Models;
using System.Data;

namespace NoteApp.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<int> CreateAsync(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM [test].[user] WHERE user_name = @Username";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM [test].[user] WHERE id = @Id";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(User user)
        {
            var sql = @"INSERT INTO [test].[user] (user_name, password_hash)
                    VALUES (@user_name, @password_hash);
                    SELECT CAST(SCOPE_IDENTITY() as int);";
            
            return await _db.ExecuteScalarAsync<int>(sql, user);
        }
    }
}
