using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace Client
{
    public class TcpClientService : IDisposable
    {
        private TcpClient? _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private readonly SemaphoreSlim _sendLock = new(1, 1);

        public bool IsConnected => _client != null && _client.Connected;
        public string? CurrentToken { get; set; }
        public UserDto? CurrentUser { get; set; }

        public async Task<(bool Ok, string Message)> ConnectAsync(string ip, int port)
        {
            try
            {
                Disconnect();
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);

                NetworkStream stream = _client.GetStream();
                _reader = MessageFraming.CreateReader(stream);
                _writer = MessageFraming.CreateWriter(stream);

                return (true, $"Đã kết nối tới {ip}:{port}");
            }
            catch (SocketException)
            {
                Disconnect();
                return (false, "Không thể kết nối tới máy chủ. Vui lòng kiểm tra IP, Cổng hoặc mạng.");
            }
            catch (IOException)
            {
                Disconnect();
                return (false, "Lỗi luồng dữ liệu mạng khi kết nối tới máy chủ.");
            }
            catch (Exception)
            {
                Disconnect();
                return (false, "Địa chỉ IP hoặc số cổng không hợp lệ.");
            }
        }

        public async Task<ResponsePacket> SendRequestAsync(RequestPacket request)
        {
            if (!IsConnected || _writer == null || _reader == null)
                return ResponsePacket.Fail(ProtocolConstants.ERR_SERVER_ERROR, "Chưa kết nối tới máy chủ.");

            await _sendLock.WaitAsync();
            try
            {
                if (!string.IsNullOrEmpty(CurrentToken) && string.IsNullOrEmpty(request.Token))
                    request.Token = CurrentToken;

                await MessageFraming.SendAsync(_writer, request);
                ResponsePacket? response = await MessageFraming.ReceiveAsync<ResponsePacket>(_reader);

                if (response == null)
                {
                    Disconnect();
                    return ResponsePacket.Fail(ProtocolConstants.ERR_SERVER_ERROR, "Máy chủ đã đóng kết nối.");
                }

                return response;
            }
            catch (IOException)
            {
                Disconnect();
                return ResponsePacket.Fail(ProtocolConstants.ERR_SERVER_ERROR, "Mất kết nối với máy chủ khi đang truyền dữ liệu.");
            }
            catch (SocketException)
            {
                Disconnect();
                return ResponsePacket.Fail(ProtocolConstants.ERR_SERVER_ERROR, "Lỗi kết nối mạng (Socket).");
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public void Disconnect()
        {
            try { _writer?.Dispose(); } catch { }
            try { _reader?.Dispose(); } catch { }
            try { _client?.Close(); _client?.Dispose(); } catch { }
            _writer = null;
            _reader = null;
            _client = null;
        }

        public void Dispose()
        {
            Disconnect();
            _sendLock.Dispose();
        }
    }
}
