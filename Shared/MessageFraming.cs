using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Shared
{
    public static class MessageFraming
    {
        public static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false // Bắt buộc false để mỗi gói JSON nằm trên 1 dòng duy nhất kết thúc bằng \n
        };

        public static StreamReader CreateReader(Stream networkStream) =>
            new(networkStream, Utf8NoBom, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);

        public static StreamWriter CreateWriter(Stream networkStream) =>
            new(networkStream, Utf8NoBom, bufferSize: 4096, leaveOpen: true) { AutoFlush = true };

        public static async Task SendAsync<T>(StreamWriter writer, T packet)
        {
            string jsonLine = JsonSerializer.Serialize(packet, JsonOptions);
            await writer.WriteLineAsync(jsonLine);
        }

        public static async Task<T?> ReceiveAsync<T>(StreamReader reader, CancellationToken token = default)
        {
            string? line = await reader.ReadLineAsync(token);
            if (string.IsNullOrWhiteSpace(line))
                return default;

            return JsonSerializer.Deserialize<T>(line, JsonOptions);
        }
    }
}
