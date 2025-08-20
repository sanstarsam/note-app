using Dapper;
using Microsoft.Data.SqlClient;
using NoteApp.Models;
using System.Data;

namespace NoteModelApp.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetNotesByUserAsync(int userId);
        Task<Note?> GetNoteByIdAsync(int id, int userId);
        Task<int> CreateNoteAsync(Note note);
        Task<bool> UpdateNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(int id, int userId);
    }

    public class NoteRepository : INoteRepository
    {
        private readonly IDbConnection _db;

        public NoteRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Note>> GetNotesByUserAsync(int userId)
        {
            var sql = "SELECT * FROM [test].[note] WHERE user_id = @userId";
            return await _db.QueryAsync<Note>(sql, new { userId = userId });
        }

        public async Task<Note?> GetNoteByIdAsync(int id, int userId)
        {
            var sql = "SELECT * FROM [test].[note] WHERE id = @Id AND user_id = @userId";
            return await _db.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, userId = userId });
        }

        public async Task<int> CreateNoteAsync(Note note)
        {
            var sql = @"INSERT INTO [test].[note] (user_id, title, content, created_at, updated_at)
                    VALUES (@userId, @title, @content, GETDATE(), GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _db.ExecuteScalarAsync<int>(sql, new
            {
                userId = note.user_id,
                title = note.title,
                content = note.content
            });
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            var sql = @"UPDATE [test].[note] SET title = @title, content = @content, updated_at = GETDATE()
                    WHERE id = @id AND user_id = @userId";
            return await _db.ExecuteAsync(sql, new
            {
                id = note.id,
                userId = note.user_id,
                title = note.title,
                content = note.content
            }) > 0;
        }

        public async Task<bool> DeleteNoteAsync(int id, int userId)
        {
            var sql = "DELETE FROM [test].[note] WHERE id = @id AND user_id = @userId";
            return await _db.ExecuteAsync(sql, new { id = id, userId = userId }) > 0;
        }
    }
}
