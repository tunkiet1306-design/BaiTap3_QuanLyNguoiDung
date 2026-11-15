using System;
using System.Windows.Forms;

namespace Client
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi nhập chính cho ứng dụng Client Windows Forms.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormDangNhap());
        }
    }
}