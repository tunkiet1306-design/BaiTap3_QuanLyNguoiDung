using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace Client
{
    public partial class FormDangKy : Form
    {
        private readonly TcpClientService _tcpService;
        private readonly string _serverIp;
        private readonly string _serverPort;

        /// <summary>
        /// Tên đăng nhập vừa đăng ký thành công (để tự điền vào FormDangNhap)
        /// </summary>
        public string? RegisteredUsername { get; private set; }

        // Các điều khiển giao diện
        private Label lblTieuDe = null!;
        private Label lblPhuDe = null!;
        private GroupBox grpThongTin = null!;

        private Label lblUsername = null!;
        private TextBox txtUsername = null!;

        private Label lblFullName = null!;
        private TextBox txtFullName = null!;

        private Label lblEmail = null!;
        private TextBox txtEmail = null!;

        private Label lblBirthday = null!;
        private DateTimePicker dtpBirthday = null!;

        private Label lblPassword = null!;
        private TextBox txtPassword = null!;

        private Label lblConfirmPassword = null!;
        private TextBox txtConfirmPassword = null!;

        private Label lblLoiDangKy = null!;
        private Button btnDangKy = null!;
        private Button btnHuy = null!;

        public FormDangKy(TcpClientService tcpService, string serverIp = "127.0.0.1", string serverPort = "9000")
        {
            _tcpService = tcpService;
            _serverIp = string.IsNullOrWhiteSpace(serverIp) ? "127.0.0.1" : serverIp;
            _serverPort = string.IsNullOrWhiteSpace(serverPort) ? ProtocolConstants.DEFAULT_PORT.ToString() : serverPort;

            InitializeComponent();
            KhoiTaoGiaoDien();
        }

        private void KhoiTaoGiaoDien()
        {
            // Thiết lập thuộc tính Form
            this.Text = "Đăng ký tài khoản người dùng mới";
            this.ClientSize = new Size(500, 570);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Tiêu đề
            lblTieuDe = new Label
            {
                Text = "ĐĂNG KÝ TÀI KHOẢN MỚI",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 15),
                Size = new Size(460, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblPhuDe = new Label
            {
                Text = "Vui lòng nhập đầy đủ và chính xác các thông tin bên dưới",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 43),
                Size = new Size(460, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // GroupBox thông tin đăng ký
            grpThongTin = new GroupBox
            {
                Text = " Thông tin người dùng ",
                Location = new Point(20, 70),
                Size = new Size(460, 480),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            // 1. Tên đăng nhập
            lblUsername = new Label
            {
                Text = "Tên đăng nhập (*):",
                Location = new Point(20, 35),
                Size = new Size(130, 20)
            };
            txtUsername = new TextBox
            {
                Location = new Point(155, 32),
                Size = new Size(280, 25),
                BorderStyle = BorderStyle.FixedSingle
            };

            // 2. Họ và tên
            lblFullName = new Label
            {
                Text = "Họ và tên (*):",
                Location = new Point(20, 75),
                Size = new Size(130, 20)
            };
            txtFullName = new TextBox
            {
                Location = new Point(155, 72),
                Size = new Size(280, 25),
                BorderStyle = BorderStyle.FixedSingle
            };

            // 3. Email
            lblEmail = new Label
            {
                Text = "Địa chỉ Email (*):",
                Location = new Point(20, 115),
                Size = new Size(130, 20)
            };
            txtEmail = new TextBox
            {
                Location = new Point(155, 112),
                Size = new Size(280, 25),
                BorderStyle = BorderStyle.FixedSingle
            };

            // 4. Ngày sinh
            lblBirthday = new Label
            {
                Text = "Ngày sinh (*):",
                Location = new Point(20, 155),
                Size = new Size(130, 20)
            };
            dtpBirthday = new DateTimePicker
            {
                Location = new Point(155, 152),
                Size = new Size(280, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MaxDate = DateTime.Today,
                Value = new DateTime(2000, 1, 1)
            };

            // 5. Mật khẩu
            lblPassword = new Label
            {
                Text = "Mật khẩu (*):",
                Location = new Point(20, 195),
                Size = new Size(130, 20)
            };
            txtPassword = new TextBox
            {
                Location = new Point(155, 192),
                Size = new Size(280, 25),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };

            // 6. Nhập lại mật khẩu
            lblConfirmPassword = new Label
            {
                Text = "Nhập lại mật khẩu (*):",
                Location = new Point(20, 235),
                Size = new Size(130, 20)
            };
            txtConfirmPassword = new TextBox
            {
                Location = new Point(155, 232),
                Size = new Size(280, 25),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };
            txtConfirmPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    btnDangKy.PerformClick();
                }
            };

            // 7. Nhãn thông báo lỗi
            lblLoiDangKy = new Label
            {
                Text = string.Empty,
                Location = new Point(20, 275),
                Size = new Size(415, 60),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            // 8. Nút Đăng ký & Hủy
            btnDangKy = new Button
            {
                Text = "ĐĂNG KÝ",
                Location = new Point(20, 350),
                Size = new Size(205, 42),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDangKy.FlatAppearance.BorderSize = 0;
            btnDangKy.Click += async (s, e) => await XuLyDangKyAsync();

            btnHuy = new Button
            {
                Text = "HỦY BỎ",
                Location = new Point(235, 350),
                Size = new Size(200, 42),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            grpThongTin.Controls.AddRange(new Control[]
            {
                lblUsername, txtUsername,
                lblFullName, txtFullName,
                lblEmail, txtEmail,
                lblBirthday, dtpBirthday,
                lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                lblLoiDangKy,
                btnDangKy, btnHuy
            });

            this.Controls.AddRange(new Control[]
            {
                lblTieuDe,
                lblPhuDe,
                grpThongTin
            });
        }

        private void DatTrangThaiDangXuLy(bool dangXuLy)
        {
            btnDangKy.Enabled = !dangXuLy;
            btnHuy.Enabled = !dangXuLy;
            txtUsername.Enabled = !dangXuLy;
            txtFullName.Enabled = !dangXuLy;
            txtEmail.Enabled = !dangXuLy;
            dtpBirthday.Enabled = !dangXuLy;
            txtPassword.Enabled = !dangXuLy;
            txtConfirmPassword.Enabled = !dangXuLy;
            this.Cursor = dangXuLy ? Cursors.WaitCursor : Cursors.Default;
        }

        private async Task XuLyDangKyAsync()
        {
            // Kiểm tra dữ liệu hợp lệ trên Form trước khi gửi (Validation)
            string username = txtUsername.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                lblLoiDangKy.Text = "Vui lòng nhập tên đăng nhập.";
                txtUsername.Focus();
                return;
            }

            if (username.Length < 3 || username.Contains(' '))
            {
                lblLoiDangKy.Text = "Tên đăng nhập phải từ 3 ký tự trở lên và không chứa dấu cách.";
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                lblLoiDangKy.Text = "Vui lòng nhập họ và tên của bạn.";
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                lblLoiDangKy.Text = "Vui lòng nhập địa chỉ Email.";
                txtEmail.Focus();
                return;
            }

            // Kiểm tra định dạng Email chuẩn bằng biểu thức chính quy (Regex)
            string patternEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, patternEmail))
            {
                lblLoiDangKy.Text = "Định dạng Email không hợp lệ (ví dụ: nguyenvana@gmail.com).";
                txtEmail.Focus();
                return;
            }

            if (dtpBirthday.Value.Date > DateTime.Today)
            {
                lblLoiDangKy.Text = "Ngày sinh không được vượt quá ngày hiện tại.";
                dtpBirthday.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                lblLoiDangKy.Text = "Vui lòng nhập mật khẩu.";
                txtPassword.Focus();
                return;
            }

            if (password.Length < 4)
            {
                lblLoiDangKy.Text = "Mật khẩu quá ngắn, vui lòng nhập ít nhất 4 ký tự.";
                txtPassword.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                lblLoiDangKy.Text = "Mật khẩu xác nhận không trùng khớp. Vui lòng nhập lại.";
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return;
            }

            lblLoiDangKy.Text = string.Empty;
            DatTrangThaiDangXuLy(true);

            try
            {
                // Kiểm tra kết nối TCP, nếu chưa kết nối thì kết nối tới Server
                if (!_tcpService.IsConnected)
                {
                    if (int.TryParse(_serverPort, out int port))
                    {
                        var (ok, msg) = await _tcpService.ConnectAsync(_serverIp, port);
                        if (!ok)
                        {
                            lblLoiDangKy.Text = "Không thể kết nối đến máy chủ. Vui lòng kiểm tra lại trạng thái Server.";
                            return;
                        }
                    }
                    else
                    {
                        lblLoiDangKy.Text = "Cổng kết nối máy chủ không hợp lệ.";
                        return;
                    }
                }

                // Đóng gói RequestPacket theo đúng yêu cầu đề bài:
                // Command = ProtocolConstants.CMD_DANG_KY
                // Mật khẩu gửi thô để Server tự băm salt/hash (không tự băm trước ở Client)
                var req = new RequestPacket
                {
                    Command = ProtocolConstants.CMD_DANG_KY,
                    Username = username,
                    Password = password,
                    FullName = fullName,
                    Email = email,
                    Birthday = dtpBirthday.Value.Date
                };

                var res = await _tcpService.SendRequestAsync(req);
                if (res.Success)
                {
                    RegisteredUsername = username;
                    MessageBox.Show(
                        "Đăng ký tài khoản thành công!\nBạn có thể đăng nhập ngay bây giờ.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblLoiDangKy.Text = string.IsNullOrWhiteSpace(res.Message)
                        ? "Đăng ký không thành công. Vui lòng thử lại."
                        : res.Message;

                    if (res.Code == ProtocolConstants.ERR_USER_EXISTS)
                    {
                        txtUsername.SelectAll();
                        txtUsername.Focus();
                    }
                }
            }
            catch
            {
                lblLoiDangKy.Text = "Đã xảy ra sự cố trong quá trình gửi yêu cầu đăng ký. Vui lòng thử lại.";
            }
            finally
            {
                DatTrangThaiDangXuLy(false);
            }
        }
    }
}
