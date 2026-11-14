using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public static class ProtocolConstants
    {
        public const int DEFAULT_PORT = 9000;

        // 4 mã lệnh bắt buộc theo Mục III.2 + lệnh PING để kiểm tra đường truyền ở Bước 4
        public const string CMD_PING = "PING";
        public const string CMD_DANG_KY = "DANGKY";
        public const string CMD_DANG_NHAP = "DANGNHAP";
        public const string CMD_LAY_THONG_TIN = "LAYTHONGTIN";
        public const string CMD_DANG_XUAT = "DANGXUAT";

        // Bảng mã phản hồi & mã lỗi
        public const string STATUS_OK = "OK";
        public const string ERR_USER_EXISTS = "ERR_USER_EXISTS";
        public const string ERR_AUTH_FAILED = "ERR_AUTH_FAILED";
        public const string ERR_TOKEN_EXPIRED = "ERR_TOKEN_EXPIRED";
        public const string ERR_INVALID_DATA = "ERR_INVALID_DATA";
        public const string ERR_SERVER_ERROR = "ERR_SERVER_ERROR";
    }
}
