using System.Security.Cryptography;
using System.Text;

namespace PasswordGenerator;

internal class Program
{
   static void Main(string[] args)
   {
      //Console.ReadLine();
      var password = "123qwe";
      byte[] salt = new byte[16];
      using (var rng = new RNGCryptoServiceProvider())
      {
         rng.GetBytes(salt);
      }
      Console.ReadLine();
      var hashedPassworded = GeneratePasswordHashUsingSalt(password, salt);
      //Console.ReadLine();
      var hashedPasswordedV2 = GeneratePasswordHashUsingSaltV2(password, salt);
      //Console.ReadLine();
      var hashedPasswordedV3 = GeneratePasswordHashUsingSaltV3(password, salt);
      Console.ReadLine();
      //Console.WriteLine(hashedPassworded);
      //Console.WriteLine(hashedPasswordedV2);
      //Console.WriteLine(hashedPasswordedV3);

      //Console.ReadLine();
   }
   public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
   {
      var iterate = 100000;
      var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
      byte[] hash = pbkdf2.GetBytes(20);

      byte[] hashBytes = new byte[36];
      Array.Copy(salt, 0, hashBytes, 0, 16);
      Array.Copy(hash, 0, hashBytes, 16, 20);

      var passwordHash = Convert.ToBase64String(hashBytes);

      return passwordHash;
   }

   public static string GeneratePasswordHashUsingSaltV2(string passwordText, byte[] salt)
   {
      var iterate = 100000;
      var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
      byte[] hash = pbkdf2.GetBytes(20);

      byte[] hashBytes = new byte[36];

      hashBytes = salt.Concat(hash).ToArray();

      var passwordHash = Convert.ToBase64String(hashBytes);

      return passwordHash;
   }

   public static string GeneratePasswordHashUsingSaltV3(string passwordText, byte[] salt)
   {
      var iterate = 100000;
      using (var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate))
      {
         byte[] hash = pbkdf2.GetBytes(20);

         var hashBytes = salt.Concat(hash).ToArray();

         var passwordHash = Convert.ToBase64String(hashBytes);

         return passwordHash;
      }
   }

}
