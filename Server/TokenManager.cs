using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Server
{
    public class TokenInfo
    {
        public string Username { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }
    public class TokenManager
    {
        private readonly ConcurrentDictionary<string, TokenInfo> _tokens = new();
        private readonly TimeSpan _tokenLifetime = TimeSpan.FromMinutes(30);

        public string GenerateToken(string username)
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
            string token = Convert.ToBase64String(randomBytes);
            _tokens[token] = new TokenInfo
            {
                Username = username,
                ExpiryTime = DateTime.UtcNow.Add(_tokenLifetime)
            };
            return token;
        }

        public bool ValidateToken(string? token, out string username)
        {
            username = string.Empty;
            if (string.IsNullOrWhiteSpace(token)) return false;

            if (_tokens.TryGetValue(token, out TokenInfo? info))
            {
                if (DateTime.UtcNow <= info.ExpiryTime)
                {
                    username = info.Username;
                    return true;
                }
                _tokens.TryRemove(token, out _);
            }
            return false;
        }

        public void RevokeToken(string? token)
        {
            if (!string.IsNullOrWhiteSpace(token))
                _tokens.TryRemove(token, out _);
        }
    }
}
