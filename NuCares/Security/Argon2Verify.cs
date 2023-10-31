using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace NuCares.Security
{
    public class Argon2Verify
    {
        #region "產生salt功能"

        public byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        #endregion "產生salt功能"

        #region "Hash處理加鹽的密碼功能"

        public byte[] HashPassword(string password, byte[] salt)
        {
            // 建立Argon2id實例，並使用UTF - 8編碼將輸入的密碼轉換為位元組陣列
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            // 設定的值會影響計算時間，驗證時也要使用相同的設定
            argon2.Salt = salt;  // 設定了Argon2計算中使用的鹽（Salt），是一個隨機值
            argon2.DegreeOfParallelism = 8;  // 計算密碼散列時同時使用的核心數，用8個核心並行處理
            argon2.Iterations = 4;  //  設定了計算的迭代次數
            argon2.MemorySize = 1024 * 1024;    // 記憶體的使用量，設定為1 GB
            return argon2.GetBytes(16);
        }

        #endregion "Hash處理加鹽的密碼功能"

        #region "驗證加密密碼"

        private bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash); // 比較兩個位元組數組（hash 和 newHash）是否相等
        }

        #endregion "驗證加密密碼"

        #region "登入驗證"

        public bool Argon2_Login(string Salt, string Password, string dbPassword)
        {
            byte[] salt = Convert.FromBase64String(Salt);
            byte[] hash = Convert.FromBase64String(dbPassword);

            // 驗證密碼
            bool success = VerifyHash(Password, salt, hash);

            return success;
        }

        #endregion "登入驗證"

        #region "不透過Argon2加密密碼"

        public (string salt, string hashPassword) PasswordHash(string password)
        {
            // 將密碼使用 SHA256 雜湊運算(不可逆)
            //string salt = email.Substring(0, 1).ToLower(); //使用帳號前一碼當作密碼鹽

            // 生成隨機的密碼鹽
            byte[] saltBytes = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(saltBytes);
            }

            string salt = Convert.ToBase64String(saltBytes);

            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(salt + password); //將密碼鹽及原密碼組合
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder newPassword = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                newPassword.Append(hash[i].ToString("X2"));
            }
            string hashPassword = newPassword.ToString(); // 雜湊運算後密碼
            return (salt, hashPassword);
        }

        #endregion "不透過Argon2加密密碼"

        #region "驗證密碼"

        public bool VerifyPassword(string userInputPassword, string storedSalt, string storedHashPassword)
        {
            // 將輸入密碼和儲存的鹽組合
            //byte[] saltBytes = Convert.FromBase64String(storedSalt);  // 用途不明?
            byte[] inputBytes = Encoding.UTF8.GetBytes(storedSalt + userInputPassword);

            // 使用 SHA-256 哈希函數對組合的密碼進行雜湊
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(inputBytes);
            StringBuilder newPassword = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                newPassword.Append(hash[i].ToString("X2"));
            }
            string hashedInputPassword = newPassword.ToString();

            // 比較雜湊後的密碼是否與儲存的密碼相符
            return hashedInputPassword == storedHashPassword;
        }

        #endregion "驗證密碼"
    }
}