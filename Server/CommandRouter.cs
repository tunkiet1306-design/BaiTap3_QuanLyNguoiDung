using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Server
{
    public class CommandRouter
    {
        private readonly NguoiDungRepository _repo;
        private readonly TokenManager _tokenManager;

        public CommandRouter(NguoiDungRepository repo, TokenManager tokenManager)
        {
            _repo = repo;
            _tokenManager = tokenManager;
        }

        public async Task<ResponsePacket> ProcessAsync(RequestPacket req, ClientSession session)
        {
            try
            {
                return req.Command switch
                {
                    ProtocolConstants.CMD_PING => ResponsePacket.Ok($"PONG: {req.Username}"),
                    ProtocolConstants.CMD_DANG_KY => await XuLyDangKyAsync(req),
                    ProtocolConstants.CMD_DANG_NHAP => await XuLyDangNhapAsync(req, session),
                    ProtocolConstants.CMD_LAY_THONG_TIN => await XuLyLayThongTinAsync(req),
                    ProtocolConstants.CMD_DANG_XUAT => XuLyDangXuat(req, session),
                    _ => ResponsePacket.Fail(ProtocolConstants.ERR_INVALID_DATA, "Lệnh không hợp lệ.")
                };
            }
            catch (Exception ex)
            {
                Program.Log($"[ERROR] Lỗi xử lý lệnh {req.Command}: {ex.Message}");
                return ResponsePacket.Fail(ProtocolConstants.ERR_SERVER_ERROR, "Lỗi máy chủ nội bộ.");
            }
        }

        private async Task<ResponsePacket> XuLyDangKyAsync(RequestPacket req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password) ||
                string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.FullName))
            {
                return ResponsePacket.Fail(ProtocolConstants.ERR_INVALID_DATA, "Vui lòng điền đầy đủ thông tin.");
            }

            if (await _repo.KiemTraTonTaiUsernameAsync(req.Username.Trim()))
            {
                return ResponsePacket.Fail(ProtocolConstants.ERR_USER_EXISTS, "Tên đăng nhập đã tồn tại.");
            }

            var (salt, hash) = MatKhau.TaoBam(req.Password);
            var entity = new NguoiDungEntity
            {
                Username = req.Username.Trim(),
                PasswordHash = hash,
                PasswordSalt = salt,
                Email = req.Email.Trim(),
                FullName = req.FullName.Trim(),
                Birthday = req.Birthday ?? DateTime.Today
            };

            await _repo.ThemNguoiDungAsync(entity);
            return ResponsePacket.Ok("Đăng ký tài khoản thành công!");
        }

        private async Task<ResponsePacket> XuLyDangNhapAsync(RequestPacket req, ClientSession session)
        {
            const string ThongBaoSai = "Tên đăng nhập hoặc mật khẩu không đúng.";
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return ResponsePacket.Fail(ProtocolConstants.ERR_AUTH_FAILED, ThongBaoSai);

            var entity = await _repo.LayTheoUsernameAsync(req.Username.Trim());
            if (entity == null || !MatKhau.KiemTra(req.Password, entity.PasswordSalt, entity.PasswordHash))
                return ResponsePacket.Fail(ProtocolConstants.ERR_AUTH_FAILED, ThongBaoSai);

            session.Username = entity.Username;
            string token = _tokenManager.GenerateToken(entity.Username);

            var dto = new UserDto
            {
                UserId = entity.UserId,
                Username = entity.Username,
                Email = entity.Email,
                FullName = entity.FullName,
                Birthday = entity.Birthday
            };

            return ResponsePacket.Ok("Đăng nhập thành công!", dto, token);
        }

        private async Task<ResponsePacket> XuLyLayThongTinAsync(RequestPacket req)
        {
            if (!_tokenManager.ValidateToken(req.Token, out string username))
                return ResponsePacket.Fail(ProtocolConstants.ERR_TOKEN_EXPIRED, "Phiên đăng nhập đã hết hạn.");

            var entity = await _repo.LayTheoUsernameAsync(username);
            if (entity == null)
                return ResponsePacket.Fail(ProtocolConstants.ERR_INVALID_DATA, "Không tìm thấy người dùng.");

            var dto = new UserDto
            {
                UserId = entity.UserId,
                Username = entity.Username,
                Email = entity.Email,
                FullName = entity.FullName,
                Birthday = entity.Birthday
            };

            return ResponsePacket.Ok("Lấy thông tin thành công.", dto, req.Token);
        }

        private ResponsePacket XuLyDangXuat(RequestPacket req, ClientSession session)
        {
            _tokenManager.RevokeToken(req.Token);
            session.Username = null;
            return ResponsePacket.Ok("Đã đăng xuất.");
        }
    }
}
