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
        public static string Grade(float percentage)
        {
            string grade;
            if (percentage >= 90 && percentage <= 100)
            {
                grade = "AA";
            }
            else if (percentage >= 75 && percentage < 90)
            {
                grade = "A+";
            }
            else if (percentage >= 60 && percentage < 75)
            {
                grade = "A";
            }
            else if (percentage >= 50 && percentage < 60)
            {
                grade = "B+";
            }
            else if (percentage >= 40 && percentage < 50)
            {
                grade = "B";
            }
            else if (percentage >= 30 && percentage < 40)
            {
                grade = "C";
            }
            else
            {
                grade = "D";
            }
            return grade;
        }
    }
}
