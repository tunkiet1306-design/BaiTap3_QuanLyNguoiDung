using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace Server
{
    public class ClientSession
    {
        public string SessionId { get; set; } = string.Empty;
        public string RemoteEndPoint { get; set; } = string.Empty;
        public string? Username { get; set; }
        public TcpClient TcpClient { get; set; } = null!;
    }

    internal class Program
    {
        private static readonly ConcurrentDictionary<string, ClientSession> ActiveClients = new();
        private static readonly TokenManager TokenMgr = new();
        private static readonly CommandRouter Router = new(new NguoiDungRepository(), TokenMgr);

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int port = ProtocolConstants.DEFAULT_PORT;

            using CancellationTokenSource cts = new();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                Log("Nhận tín hiệu dừng Server (Ctrl+C). Đang đóng Server tử tế...");
                cts.Cancel();
            };

            TcpListener listener = new(IPAddress.Any, port);
            try
            {
                listener.Start();
                Log($"Server đang lắng nghe tại IPAddress.Any:{port}. Nhấn Ctrl+C để dừng.");

                while (!cts.Token.IsCancellationRequested)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync(cts.Token);
                    _ = HandleClientAsync(client, cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // Bắt OperationCanceledException rồi bỏ qua theo Mục III.8.5
            }
            catch (Exception ex)
            {
                Log($"Lỗi khởi chạy Server: {ex.Message}");
            }
            finally
            {
                listener.Stop();
                Log("Server đã dừng hoàn toàn.");
            }
        }

        private static async Task HandleClientAsync(TcpClient client, CancellationToken serverToken)
        {
            string sessionId = Guid.NewGuid().ToString("N")[..8];
            string endpoint = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";

            var session = new ClientSession
            {
                SessionId = sessionId,
                RemoteEndPoint = endpoint,
                TcpClient = client
            };

            ActiveClients[sessionId] = session;
            Log($"[+] Client kết nối: Session={sessionId}, IP={endpoint}. Đang kết nối: {ActiveClients.Count}");

            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = MessageFraming.CreateReader(stream))
                using (StreamWriter writer = MessageFraming.CreateWriter(stream))
                {
                    while (!serverToken.IsCancellationRequested)
                    {
                        RequestPacket? request = await MessageFraming.ReceiveAsync<RequestPacket>(reader, serverToken);
                        if (request == null)
                        {
                            Log($"[-] Client [{sessionId}] ({endpoint}) đã đóng cửa sổ kết nối.");
                            break;
                        }

                        Log($"[REQ] Session={sessionId} | Lệnh={request.Command} | User={request.Username}");
                        ResponsePacket response = await Router.ProcessAsync(request, session);
                        await MessageFraming.SendAsync(writer, response);
                        Log($"[RES] Session={sessionId} | Lệnh={request.Command} | Mã={response.Code}");

                        if (request.Command == ProtocolConstants.CMD_DANG_XUAT)
                        {
                            Log($"[-] Client [{sessionId}] đã gửi lệnh DANGXUAT.");
                            break;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Log($"[!] Client [{sessionId}] mất mạng đột ngột (IOException): {ex.Message}");
            }
            catch (SocketException ex)
            {
                Log($"[!] Lỗi Socket tại phiên [{sessionId}]: {ex.SocketErrorCode}");
            }
            catch (OperationCanceledException)
            {
                Log($"[!] Đóng phiên [{sessionId}] do Server dừng.");
            }
            catch (Exception ex)
            {
                Log($"[!] Lỗi ngoại lệ tại phiên [{sessionId}]: {ex.Message}");
            }
            finally
            {
                ActiveClients.TryRemove(sessionId, out _);
                Log($"[x] Đã gỡ Session={sessionId}. Số client còn lại: {ActiveClients.Count}");
            }
        }

        public static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
    }
}
