using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    internal class Packets
    {

    }
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
    }
    public class RequestPacket
    {
        public string Command { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthday { get; set; }
    }
    public class ResponsePacket
    {
        public bool Success { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public UserDto? User { get; set; }

        public static ResponsePacket Ok(string message, UserDto? user = null, string? token = null) =>
            new() { Success = true, Code = ProtocolConstants.STATUS_OK, Message = message, User = user, Token = token };

        public static ResponsePacket Fail(string errorCode, string message) =>
            new() { Success = false, Code = errorCode, Message = message };
    }
}
