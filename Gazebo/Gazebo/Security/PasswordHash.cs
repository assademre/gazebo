using System.Security.Cryptography;
using System.Text;

namespace Gazebo.Security
{
    public interface IHashPassword
    {
        string HashPassword(string password);
    }
    public class PasswordHash : IHashPassword
    {
        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
