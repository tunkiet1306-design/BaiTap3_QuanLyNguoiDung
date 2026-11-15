using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Server
{
    public class NguoiDungEntity
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
    }

    public class NguoiDungRepository
    {
        private const string ConnectionString =
           @"Server=localhost\SQLEXPRESS;Database=QuanLyNguoiDung;Trusted_Connection=True;TrustServerCertificate=True;";

        public async Task<bool> KiemTraTonTaiUsernameAsync(string username)
        {
            using var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand("SELECT COUNT(1) FROM NguoiDung WHERE Username = @u", conn);
            cmd.Parameters.AddWithValue("@u", username);
            int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task ThemNguoiDungAsync(NguoiDungEntity entity)
        {
            using var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync();
            const string sql = @"
                INSERT INTO NguoiDung (Username, PasswordHash, PasswordSalt, Email, FullName, Birthday)
                VALUES (@u, @h, @s, @e, @f, @b);";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", entity.Username);
            cmd.Parameters.AddWithValue("@h", entity.PasswordHash);
            cmd.Parameters.AddWithValue("@s", entity.PasswordSalt);

            cmd.Parameters.AddWithValue("@e", string.IsNullOrWhiteSpace(entity.Email) ? DBNull.Value : entity.Email);
            cmd.Parameters.AddWithValue("@f", string.IsNullOrWhiteSpace(entity.FullName) ? DBNull.Value : entity.FullName);
            cmd.Parameters.AddWithValue("@b", entity.Birthday == DateTime.MinValue ? DBNull.Value : entity.Birthday);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<NguoiDungEntity?> LayTheoUsernameAsync(string username)
        {
            using var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, PasswordSalt, Email, FullName, Birthday
                FROM NguoiDung WHERE Username = @u;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", username);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new NguoiDungEntity
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    PasswordSalt = reader.GetString(3),
                    Email = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    FullName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Birthday = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6)
                };
            }
            return null;
        }
    }
}
