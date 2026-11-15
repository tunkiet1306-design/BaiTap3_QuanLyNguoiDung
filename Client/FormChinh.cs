using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace Client
{
    public partial class FormChinh : Form
    {
        private readonly TcpClientService _tcpService;
        private bool _isLoggingOut = false;

        // Tiêu đề
        private Label lblTieuDe = null!;
        private Label lblPhuDe = null!;

        // Khu vực thông tin người dùng
        private GroupBox grpThongTin = null!;
        private Label lblUserIdTitle = null!;
        private TextBox txtUserId = null!;
        private Label lblUsernameTitle = null!;
        private TextBox txtUsername = null!;
        private Label lblFullNameTitle = null!;
        private TextBox txtFullName = null!;
        private Label lblEmailTitle = null!;
        private TextBox txtEmail = null!;
        private Label lblBirthdayTitle = null!;
        private TextBox txtBirthday = null!;

        // Khu vực thông tin phiên làm việc
        private GroupBox grpPhien = null!;
        private Label lblTokenTitle = null!;
        private TextBox txtToken = null!;
        private Label lblTrangThaiTokenTitle = null!;
        private Label lblTrangThaiToken = null!;

        // Các nút điều khiển & thông báo
        private Label lblThongBao = null!;
        private Button btnLamMoiThongTin = null!;
        private Button btnDangXuat = null!;

        public FormChinh(TcpClientService tcpService)
        {
            _tcpService = tcpService;

            InitializeComponent();
            KhoiTaoGiaoDien();
            HienThiThongTin(_tcpService.CurrentUser, _tcpService.CurrentToken);
        }

        private void KhoiTaoGiaoDien()
        {
            // Thiết lập thuộc tính Form
            this.Text = "Hệ thống Quản lý Người dùng - Màn hình chính";
            this.ClientSize = new Size(540, 540);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.FormClosing += FormChinh_FormClosing;

            // 1. Tiêu đề
            lblTieuDe = new Label
            {
                Text = "THÔNG TIN TÀI KHOẢN",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 15),
                Size = new Size(500, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblPhuDe = new Label
            {
                Text = "Quản lý phiên làm việc TCP Client - Server",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 43),
                Size = new Size(500, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 2. GroupBox hồ sơ người dùng
            grpThongTin = new GroupBox
            {
                Text = " Hồ sơ người dùng ",
                Location = new Point(20, 70),
                Size = new Size(500, 230),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            // Mã người dùng (ID)
            lblUserIdTitle = new Label
            {
                Text = "Mã người dùng (ID):",
                Location = new Point(20, 32),
                Size = new Size(130, 20)
            };
            txtUserId = new TextBox
            {
                Location = new Point(160, 29),
                Size = new Size(315, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(233, 236, 239),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Tên đăng nhập
            lblUsernameTitle = new Label
            {
                Text = "Tên đăng nhập:",
                Location = new Point(20, 68),
                Size = new Size(130, 20)
            };
            txtUsername = new TextBox
            {
                Location = new Point(160, 65),
                Size = new Size(315, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(233, 236, 239),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Họ và tên
            lblFullNameTitle = new Label
            {
                Text = "Họ và tên:",
                Location = new Point(20, 104),
                Size = new Size(130, 20)
            };
            txtFullName = new TextBox
            {
                Location = new Point(160, 101),
                Size = new Size(315, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(233, 236, 239),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Email
            lblEmailTitle = new Label
            {
                Text = "Địa chỉ Email:",
                Location = new Point(20, 140),
                Size = new Size(130, 20)
            };
            txtEmail = new TextBox
            {
                Location = new Point(160, 137),
                Size = new Size(315, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(233, 236, 239),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Ngày sinh
            lblBirthdayTitle = new Label
            {
                Text = "Ngày sinh:",
                Location = new Point(20, 176),
                Size = new Size(130, 20)
            };
            txtBirthday = new TextBox
            {
                Location = new Point(160, 173),
                Size = new Size(315, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(233, 236, 239),
                BorderStyle = BorderStyle.FixedSingle
            };

            grpThongTin.Controls.AddRange(new Control[]
            {
                lblUserIdTitle, txtUserId,
                lblUsernameTitle, txtUsername,
                lblFullNameTitle, txtFullName,
                lblEmailTitle, txtEmail,
                lblBirthdayTitle, txtBirthday
            });

            // 3. GroupBox thông tin phiên & Token
            grpPhien = new GroupBox
            {
                Text = " Phiên làm việc & Token xác thực ",
                Location = new Point(20, 310),
                Size = new Size(500, 110),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            lblTokenTitle = new Label
            {
                Text = "Mã phiên (Token):",
                Location = new Point(20, 32),
                Size = new Size(130, 20)
            };
            txtToken = new TextBox
            {
                Location = new Point(160, 29),
                Size = new Size(315, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(233, 236, 239),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTrangThaiTokenTitle = new Label
            {
                Text = "Trạng thái Token:",
                Location = new Point(20, 68),
                Size = new Size(130, 20)
            };
            lblTrangThaiToken = new Label
            {
                Text = "Hợp lệ / Đang hoạt động",
                Location = new Point(160, 68),
                Size = new Size(315, 20),
                ForeColor = Color.ForestGreen,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            grpPhien.Controls.AddRange(new Control[]
            {
                lblTokenTitle, txtToken,
                lblTrangThaiTokenTitle, lblTrangThaiToken
            });

            // 4. Nhãn thông báo kết quả
            lblThongBao = new Label
            {
                Text = string.Empty,
                Location = new Point(20, 425),
                Size = new Size(500, 30),
                ForeColor = Color.ForestGreen,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            // 5. Nút bấm
            btnLamMoiThongTin = new Button
            {
                Text = "Làm mới thông tin (LAYTHONGTIN)",
                Location = new Point(20, 465),
                Size = new Size(260, 42),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLamMoiThongTin.FlatAppearance.BorderSize = 0;
            btnLamMoiThongTin.Click += async (s, e) => await XuLyLamMoiThongTinAsync();

            btnDangXuat = new Button
            {
                Text = "Đăng xuất (DANGXUAT)",
                Location = new Point(290, 465),
                Size = new Size(230, 42),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.Click += async (s, e) => await XuLyDangXuatAsync();

            this.Controls.AddRange(new Control[]
            {
                lblTieuDe,
                lblPhuDe,
                grpThongTin,
                grpPhien,
                lblThongBao,
                btnLamMoiThongTin,
                btnDangXuat
            });
        }

        private void HienThiThongTin(UserDto? user, string? token)
        {
            if (user != null)
            {
                txtUserId.Text = user.UserId.ToString();
                txtUsername.Text = user.Username;
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                // Hiển thị ngày sinh theo đúng định dạng dd/MM/yyyy
                txtBirthday.Text = user.Birthday.ToString("dd/MM/yyyy");
            }
            else
            {
                txtUserId.Text = "N/A";
                txtUsername.Text = "N/A";
                txtFullName.Text = "N/A";
                txtEmail.Text = "N/A";
                txtBirthday.Text = "N/A";
            }

            txtToken.Text = string.IsNullOrEmpty(token) ? "(Không có Token)" : token;
            lblTrangThaiToken.Text = "Hợp lệ / Đang hoạt động";
            lblTrangThaiToken.ForeColor = Color.ForestGreen;
        }

        private void DatTrangThaiDangXuLy(bool dangXuLy)
        {
            btnLamMoiThongTin.Enabled = !dangXuLy;
            btnDangXuat.Enabled = !dangXuLy;
            this.Cursor = dangXuLy ? Cursors.WaitCursor : Cursors.Default;
        }

        private async Task XuLyLamMoiThongTinAsync()
        {
            DatTrangThaiDangXuLy(true);
            lblThongBao.ForeColor = Color.DarkBlue;
            lblThongBao.Text = "Đang gửi yêu cầu làm mới thông tin từ máy chủ...";

            try
            {
                var req = new RequestPacket
                {
                    Command = ProtocolConstants.CMD_LAY_THONG_TIN,
                    Token = _tcpService.CurrentToken
                };

                var res = await _tcpService.SendRequestAsync(req);

                // Nếu Server trả về mã ERR_TOKEN_EXPIRED thì thông báo hết hạn phiên và tự động đóng FormChinh quay về FormDangNhap
                if (res.Code == ProtocolConstants.ERR_TOKEN_EXPIRED)
                {
                    lblTrangThaiToken.Text = "Đã hết hạn";
                    lblTrangThaiToken.ForeColor = Color.Red;

                    MessageBox.Show(
                        "Phiên đăng nhập của bạn đã hết hạn. Vui lòng đăng nhập lại!",
                        "Hết hạn phiên đăng nhập",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    _isLoggingOut = true;
                    _tcpService.Disconnect();
                    _tcpService.CurrentToken = null;
                    _tcpService.CurrentUser = null;
                    this.Close();
                    return;
                }

                if (res.Success && res.User != null)
                {
                    _tcpService.CurrentUser = res.User;
                    HienThiThongTin(res.User, _tcpService.CurrentToken);
                    lblThongBao.ForeColor = Color.ForestGreen;
                    lblThongBao.Text = "Làm mới thông tin thành công!";
                }
                else
                {
                    lblThongBao.ForeColor = Color.Red;
                    lblThongBao.Text = string.IsNullOrWhiteSpace(res.Message)
                        ? "Không thể lấy thông tin người dùng từ máy chủ."
                        : res.Message;
                }
            }
            catch
            {
                lblThongBao.ForeColor = Color.Red;
                lblThongBao.Text = "Đã xảy ra sự cố khi kết nối tới máy chủ. Vui lòng thử lại.";
            }
            finally
            {
                DatTrangThaiDangXuLy(false);
            }
        }

        private async Task XuLyDangXuatAsync()
        {
            var luaChon = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (luaChon != DialogResult.Yes) return;

            DatTrangThaiDangXuLy(true);
            lblThongBao.ForeColor = Color.DarkBlue;
            lblThongBao.Text = "Đang thực hiện đăng xuất...";

            try
            {
                // Gửi lệnh DANGXUAT tới Server nếu đang kết nối
                if (_tcpService.IsConnected && !string.IsNullOrEmpty(_tcpService.CurrentToken))
                {
                    var req = new RequestPacket
                    {
                        Command = ProtocolConstants.CMD_DANG_XUAT,
                        Token = _tcpService.CurrentToken
                    };

                    await _tcpService.SendRequestAsync(req);
                }
            }
            catch
            {
                // Bỏ qua lỗi truyền thông khi đăng xuất
            }
            finally
            {
                _isLoggingOut = true;
                _tcpService.Disconnect();
                _tcpService.CurrentToken = null;
                _tcpService.CurrentUser = null;
                this.Close();
            }
        }

        private void FormChinh_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!_isLoggingOut)
            {
                // Xử lý khi người dùng bấm nút X đỏ tắt cửa sổ FormChinh:
                // Nếu đang còn kết nối thì đóng kết nối an toàn để Server gỡ phiên ngay lập tức
                try
                {
                    if (_tcpService.IsConnected)
                    {
                        _tcpService.Disconnect();
                        _tcpService.CurrentToken = null;
                        _tcpService.CurrentUser = null;
                    }
                }
                catch
                {
                    // Tuyệt đối không để lộ lỗi ngoại lệ
                }
            }
        }

        private void FormChinh_Load(object sender, EventArgs e)
        {

        }
    }
}
