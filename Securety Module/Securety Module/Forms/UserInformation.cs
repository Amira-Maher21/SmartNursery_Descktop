using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using SecuretyModule.DB;

namespace SecuretyModule
{
    public static class UserInformation
    {
        public static string UserName { get; set; }
        public static string SecConnctionStrin { get; set; }
        public static object Useres()
        {
            DataClassesSecurityMouleDataContext  dbcon = new DataClassesSecurityMouleDataContext();
            var users = from u in dbcon.Users select u;
            return users;
        }
        public static string CompHash(string input)
        {
            HashAlgorithm sha = SHA256Cng.Create();
            byte[] hashData = sha.ComputeHash(Encoding.Default.GetBytes(input));
            return Convert.ToBase64String(hashData);
        }
        public static bool CheckAction(string action)
        {
            DataClassesSecurityMouleDataContext dbcon = new DataClassesSecurityMouleDataContext();
            return Convert.ToBoolean(dbcon.CheckAction(UserName, action));
        }
        
    }
}
