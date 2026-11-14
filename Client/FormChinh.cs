using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class FormChinh : Form
    {
        private readonly TcpClientService _tcpService;
        public FormChinh(TcpClientService tcpService)
        {
            _tcpService = tcpService;
            this.Text = "Thông tin người dùng";
            this.Width = 520;
            this.Height = 460;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
