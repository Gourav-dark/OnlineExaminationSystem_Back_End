using System.Security.Cryptography;
using System.Text;

namespace OnlineExaminationSystem_Back_End_DAL.Contains.Functions
{
    public class CommanFunctions
    {
        public static string EncriptPassword(string password)
        {
            string salt = "QWERTYUIOPASDFGHJKLMNBVCXZ1234567890";
            password += salt;
            var sha = SHA512.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var encriptpassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(encriptpassword);
        }
        public static bool VerifyPassword(string currpassword, string databasepassword)
        {
            if(EncriptPassword(currpassword).Equals(databasepassword)) 
            {
                return true;
            }
            return false;
        }
    }
}
