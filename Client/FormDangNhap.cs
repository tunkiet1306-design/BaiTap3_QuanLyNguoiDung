using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class FormDangNhap : Form
    {
        private readonly TcpClientService _tcpService = new();
        public FormDangNhap()
        {
            this.Text = "Đăng nhập hệ thống (TCP Client)";
            this.Width = 480;
            this.Height = 560;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
