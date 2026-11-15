using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace Client
{
    public partial class FormDangNhap : Form
    {
        private readonly TcpClientService _tcpService = new();

        // Khu vực tiêu đề
        private Label lblTieuDe = null!;
        private Label lblPhuDe = null!;

        // Khu vực cấu hình kết nối Server
        private GroupBox grpServer = null!;
        private Label lblServerIp = null!;
        private TextBox txtServerIp = null!;
        private Label lblServerPort = null!;
        private TextBox txtServerPort = null!;
        private Button btnKetNoi = null!;
        private Button btnTest20Chuoi = null!;
        private Label lblTrangThaiTitle = null!;
        private Label lblTrangThaiKetNoi = null!;

        // Khu vực đăng nhập tài khoản
        private GroupBox grpDangNhap = null!;
        private Label lblUsername = null!;
        private TextBox txtUsername = null!;
        private Label lblPassword = null!;
        private TextBox txtPassword = null!;
        private Label lblThongBao = null!;
        private Button btnDangNhap = null!;
        private Button btnMoDangKy = null!;

        public FormDangNhap()
        {
            InitializeComponent();
            KhoiTaoGiaoDien();
        }

        private void KhoiTaoGiaoDien()
        {
            // Thiết lập thuộc tính Form
            this.Text = "Hệ thống Quản lý Người dùng (TCP Client)";
            this.ClientSize = new Size(500, 580);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.FormClosing += FormDangNhap_FormClosing;

            // 1. Tiêu đề
            lblTieuDe = new Label
            {
                Text = "HỆ THỐNG ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 15),
                Size = new Size(460, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblPhuDe = new Label
            {
                Text = "Bài tập 3 - Lập trình mạng căn bản (TCP Client-Server)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 45),
                Size = new Size(460, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 2. Khu vực cấu hình Server
            grpServer = new GroupBox
            {
                Text = " Cấu hình kết nối máy chủ ",
                Location = new Point(20, 75),
                Size = new Size(460, 165),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            lblServerIp = new Label
            {
                Text = "Địa chỉ IP:",
                Location = new Point(15, 28),
                Size = new Size(95, 20)
            };

            txtServerIp = new TextBox
            {
                Text = "127.0.0.1",
                Location = new Point(115, 25),
                Size = new Size(180, 25),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblServerPort = new Label
            {
                Text = "Cổng (Port):",
                Location = new Point(15, 63),
                Size = new Size(95, 20)
            };

            txtServerPort = new TextBox
            {
                Text = ProtocolConstants.DEFAULT_PORT.ToString(),
                Location = new Point(115, 60),
                Size = new Size(180, 25),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnKetNoi = new Button
            {
                Text = "Kết nối /\nKiểm tra",
                Location = new Point(310, 24),
                Size = new Size(135, 61),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnKetNoi.FlatAppearance.BorderSize = 0;
            btnKetNoi.Click += async (s, e) => await XuLyKetNoiAsync();

            lblTrangThaiTitle = new Label
            {
                Text = "Trạng thái:",
                Location = new Point(15, 98),
                Size = new Size(80, 20)
            };

            lblTrangThaiKetNoi = new Label
            {
                Text = "Chưa kết nối",
                Location = new Point(100, 98),
                Size = new Size(200, 20),
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            btnTest20Chuoi = new Button
            {
                Text = "Test gửi 20 chuỗi - Bước 4",
                Location = new Point(15, 123),
                Size = new Size(430, 32),
                BackColor = Color.FromArgb(71, 85, 105),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTest20Chuoi.FlatAppearance.BorderSize = 0;
            btnTest20Chuoi.Click += async (s, e) => await XuLyTest20ChuoiAsync();

            grpServer.Controls.AddRange(new Control[]
            {
                lblServerIp, txtServerIp,
                lblServerPort, txtServerPort,
                btnKetNoi,
                lblTrangThaiTitle, lblTrangThaiKetNoi,
                btnTest20Chuoi
            });

            // 3. Khu vực đăng nhập
            grpDangNhap = new GroupBox
            {
                Text = " Thông tin đăng nhập ",
                Location = new Point(20, 250),
                Size = new Size(460, 290),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            lblUsername = new Label
            {
                Text = "Tên đăng nhập:",
                Location = new Point(15, 33),
                Size = new Size(110, 20)
            };

            txtUsername = new TextBox
            {
                Location = new Point(130, 30),
                Size = new Size(305, 25),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblPassword = new Label
            {
                Text = "Mật khẩu:",
                Location = new Point(15, 73),
                Size = new Size(110, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(130, 70),
                Size = new Size(305, 25),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };
            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    btnDangNhap.PerformClick();
                }
            };

            lblThongBao = new Label
            {
                Text = string.Empty,
                Location = new Point(15, 105),
                Size = new Size(430, 45),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            btnDangNhap = new Button
            {
                Text = "ĐĂNG NHẬP",
                Location = new Point(15, 155),
                Size = new Size(430, 42),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.Click += async (s, e) => await XuLyDangNhapAsync();

            btnMoDangKy = new Button
            {
                Text = "Chưa có tài khoản? Đăng ký ngay",
                Location = new Point(15, 210),
                Size = new Size(430, 35),
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnMoDangKy.FlatAppearance.BorderSize = 0;
            btnMoDangKy.Click += (s, e) => MoFormDangKy();

            grpDangNhap.Controls.AddRange(new Control[]
            {
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                lblThongBao,
                btnDangNhap,
                btnMoDangKy
            });

            // Thêm vào Form
            this.Controls.AddRange(new Control[]
            {
                lblTieuDe,
                lblPhuDe,
                grpServer,
                grpDangNhap
            });
        }

        private void CapNhatTrangThaiKetNoi(bool daKetNoi, string? thongDiep = null)
        {
            if (daKetNoi)
            {
                lblTrangThaiKetNoi.Text = "Đã kết nối";
                lblTrangThaiKetNoi.ForeColor = Color.ForestGreen;
            }
            else
            {
                lblTrangThaiKetNoi.Text = "Chưa kết nối";
                lblTrangThaiKetNoi.ForeColor = Color.Red;
            }

            if (!string.IsNullOrEmpty(thongDiep))
            {
                lblThongBao.ForeColor = daKetNoi ? Color.ForestGreen : Color.Red;
                lblThongBao.Text = thongDiep;
            }
        }

        private void DatTrangThaiDangXuLy(bool dangXuLy)
        {
            btnKetNoi.Enabled = !dangXuLy;
            btnTest20Chuoi.Enabled = !dangXuLy;
            btnDangNhap.Enabled = !dangXuLy;
            btnMoDangKy.Enabled = !dangXuLy;
            txtServerIp.Enabled = !dangXuLy;
            txtServerPort.Enabled = !dangXuLy;
            txtUsername.Enabled = !dangXuLy;
            txtPassword.Enabled = !dangXuLy;
            this.Cursor = dangXuLy ? Cursors.WaitCursor : Cursors.Default;
        }

        private async Task XuLyKetNoiAsync()
        {
            string ip = txtServerIp.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                CapNhatTrangThaiKetNoi(false, "Vui lòng nhập địa chỉ IP máy chủ.");
                txtServerIp.Focus();
                return;
            }

            if (!int.TryParse(txtServerPort.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                CapNhatTrangThaiKetNoi(false, "Cổng máy chủ không hợp lệ (hợp lệ từ 1 đến 65535).");
                txtServerPort.Focus();
                return;
            }

            DatTrangThaiDangXuLy(true);
            try
            {
                lblThongBao.ForeColor = Color.DarkBlue;
                lblThongBao.Text = "Đang kết nối tới máy chủ...";

                var (ok, msg) = await _tcpService.ConnectAsync(ip, port);
                CapNhatTrangThaiKetNoi(ok, msg);
            }
            catch
            {
                CapNhatTrangThaiKetNoi(false, "Không thể kết nối tới máy chủ. Vui lòng kiểm tra lại mạng hoặc IP/Cổng.");
            }
            finally
            {
                DatTrangThaiDangXuLy(false);
            }
        }

        private async Task XuLyTest20ChuoiAsync()
        {
            string ip = txtServerIp.Text.Trim();
            if (!int.TryParse(txtServerPort.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                CapNhatTrangThaiKetNoi(false, "Cổng máy chủ không hợp lệ.");
                return;
            }

            DatTrangThaiDangXuLy(true);
            try
            {
                // Nếu chưa kết nối thì tự động kết nối trước
                if (!_tcpService.IsConnected)
                {
                    lblThongBao.ForeColor = Color.DarkBlue;
                    lblThongBao.Text = "Đang kết nối tới máy chủ trước khi kiểm thử...";
                    var (connectOk, connectMsg) = await _tcpService.ConnectAsync(ip, port);
                    CapNhatTrangThaiKetNoi(connectOk, connectMsg);
                    if (!connectOk)
                    {
                        return;
                    }
                }

                lblThongBao.ForeColor = Color.DarkBlue;
                lblThongBao.Text = "Đang gửi liên tiếp 20 gói tin PING (Bước 4)...";

                int thanhCong = 0;
                var sw = System.Diagnostics.Stopwatch.StartNew();

                for (int i = 1; i <= 20; i++)
                {
                    var req = new RequestPacket
                    {
                        Command = ProtocolConstants.CMD_PING,
                        Username = $"Chuoi-Ping-{i:D2}"
                    };

                    var res = await _tcpService.SendRequestAsync(req);
                    if (res.Success && res.Code == ProtocolConstants.STATUS_OK)
                    {
                        thanhCong++;
                    }
                }

                sw.Stop();

                if (thanhCong == 20)
                {
                    lblThongBao.ForeColor = Color.ForestGreen;
                    lblThongBao.Text = $"Hoàn thành Bước 4: Nhận đủ 20/20 phản hồi ({sw.ElapsedMilliseconds} ms)!";
                    MessageBox.Show(
                        $"[KẾT QUẢ KIỂM THỬ BƯỚC 4]\n\n" +
                        $"• Đã gửi liên tiếp: 20 gói tin PING.\n" +
                        $"• Đã nhận thành công: {thanhCong}/20 phản hồi PONG hợp lệ.\n" +
                        $"• Tổng thời gian xử lý: {sw.ElapsedMilliseconds} ms.\n\n" +
                        $"Kết luận: Kỹ thuật phân tách gói tin hoạt động hoàn hảo, không bị dính gói hay mất gói.",
                        "Kiểm thử Bước 4 thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    lblThongBao.ForeColor = Color.Red;
                    lblThongBao.Text = $"Kiểm thử thất bại: Chỉ nhận được {thanhCong}/20 phản hồi.";
                    MessageBox.Show(
                        $"Kiểm thử chưa đạt yêu cầu: Chỉ nhận được {thanhCong}/20 phản hồi từ máy chủ.",
                        "Cảnh báo kiểm thử",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch
            {
                CapNhatTrangThaiKetNoi(false, "Đã xảy ra sự cố trong quá trình kiểm thử đường truyền.");
            }
            finally
            {
                DatTrangThaiDangXuLy(false);
            }
        }

        private async Task XuLyDangNhapAsync()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                lblThongBao.ForeColor = Color.Red;
                lblThongBao.Text = "Vui lòng nhập tên đăng nhập.";
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                lblThongBao.ForeColor = Color.Red;
                lblThongBao.Text = "Vui lòng nhập mật khẩu.";
                txtPassword.Focus();
                return;
            }

            string ip = txtServerIp.Text.Trim();
            if (!int.TryParse(txtServerPort.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                CapNhatTrangThaiKetNoi(false, "Cổng máy chủ không hợp lệ (1 - 65535).");
                return;
            }

            DatTrangThaiDangXuLy(true);
            try
            {
                // Nếu chưa kết nối TCP thì tự động kết nối
                if (!_tcpService.IsConnected)
                {
                    lblThongBao.ForeColor = Color.DarkBlue;
                    lblThongBao.Text = "Đang kết nối tới máy chủ...";
                    var (connectOk, connectMsg) = await _tcpService.ConnectAsync(ip, port);
                    CapNhatTrangThaiKetNoi(connectOk, connectMsg);
                    if (!connectOk)
                    {
                        return;
                    }
                }

                lblThongBao.ForeColor = Color.DarkBlue;
                lblThongBao.Text = "Đang xác thực tài khoản...";

                var req = new RequestPacket
                {
                    Command = ProtocolConstants.CMD_DANG_NHAP,
                    Username = username,
                    Password = password
                };

                var res = await _tcpService.SendRequestAsync(req);
                if (res.Success)
                {
                    _tcpService.CurrentToken = res.Token;
                    _tcpService.CurrentUser = res.User;

                    lblThongBao.Text = string.Empty;
                    txtPassword.Clear();

                    this.Hide();

                    var formChinh = new FormChinh(_tcpService);
                    formChinh.FormClosed += (s, ev) =>
                    {
                        CapNhatTrangThaiKetNoi(_tcpService.IsConnected);
                        lblThongBao.Text = string.Empty;
                        this.Show();
                    };
                    formChinh.Show();
                }
                else
                {
                    lblThongBao.ForeColor = Color.Red;
                    lblThongBao.Text = string.IsNullOrWhiteSpace(res.Message)
                        ? "Đăng nhập thất bại. Vui lòng kiểm tra lại tài khoản."
                        : res.Message;
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch
            {
                lblThongBao.ForeColor = Color.Red;
                lblThongBao.Text = "Đã xảy ra sự cố trong quá trình đăng nhập. Vui lòng thử lại.";
                txtPassword.Clear();
            }
            finally
            {
                DatTrangThaiDangXuLy(false);
            }
        }

        private void MoFormDangKy()
        {
            string ip = txtServerIp.Text.Trim();
            string port = txtServerPort.Text.Trim();

            using var formDangKy = new FormDangKy(_tcpService, ip, port);
            if (formDangKy.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(formDangKy.RegisteredUsername))
            {
                txtUsername.Text = formDangKy.RegisteredUsername;
                txtPassword.Clear();
                txtPassword.Focus();
                lblThongBao.ForeColor = Color.ForestGreen;
                lblThongBao.Text = "Đăng ký thành công! Vui lòng nhập mật khẩu để đăng nhập.";
            }
        }

        private void FormDangNhap_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                _tcpService.Disconnect();
                _tcpService.Dispose();
            }
            catch
            {
                // Bỏ qua lỗi dọn dẹp khi tắt ứng dụng
            }
        }
    }
}
