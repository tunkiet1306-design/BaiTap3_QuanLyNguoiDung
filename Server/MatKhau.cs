using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Server
{
    public static class MatKhau
    {
        public const int VONG_LAP = 600_000;

        public static (string Salt, string Hash) TaoBam(string matKhauTho)
        {
            if (string.IsNullOrEmpty(matKhauTho))
                throw new ArgumentException("Mật khẩu không được để trống.", nameof(matKhauTho));

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                matKhauTho, salt, VONG_LAP, HashAlgorithmName.SHA256, 32);

            return (Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool KiemTra(string matKhauTho, string saltBase64, string hashBase64)
        {
            if (string.IsNullOrWhiteSpace(matKhauTho) ||
                string.IsNullOrWhiteSpace(saltBase64) ||
                string.IsNullOrWhiteSpace(hashBase64))
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(saltBase64);
                byte[] hashLuu = Convert.FromBase64String(hashBase64);
                byte[] hashMoi = Rfc2898DeriveBytes.Pbkdf2(
                    matKhauTho, salt, VONG_LAP, HashAlgorithmName.SHA256, 32);

                return CryptographicOperations.FixedTimeEquals(hashLuu, hashMoi);
            }
            catch (FormatException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}