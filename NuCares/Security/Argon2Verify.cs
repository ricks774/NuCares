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
    }
}