using System.Net.Sockets;

namespace Server
{
    public class ClientSession
    {
        public string SessionId { get; set; } = string.Empty;

        public string RemoteEndPoint { get; set; } = string.Empty;

        public string? Username { get; set; }

        public TcpClient TcpClient { get; set; } = null!;

        public DateTime ConnectedAt { get; set; }
    }
}