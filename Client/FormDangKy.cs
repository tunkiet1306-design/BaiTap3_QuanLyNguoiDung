using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class FormDangKy : Form
    {
        private readonly TcpClientService _tcpService;
        public FormDangKy(TcpClientService tcpService)
        {
            _tcpService = tcpService;
            this.Text = "Đăng ký tài khoản mới";
            this.Width = 500;
            this.Height = 650;
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
