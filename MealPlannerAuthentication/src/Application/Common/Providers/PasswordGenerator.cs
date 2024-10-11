using System.Text;

namespace invensys.iserve.Application.Common.Providers;

public class PasswordGenerator
{
   private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
   private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
   private const string Digits = "0123456789";
   private const string SpecialCharacters = "!@#$%^&*()_-+=?";

   public static string GeneratePassword(
      int length = 12,
      bool includeUppercase = true,
      bool includeLowercase = true,
      bool includeDigits = true,
      bool includeSpecialCharacters = true)
   {
      if (length < 1) throw new ArgumentException("Password length should be greater than 0.");

      var characterSet = new StringBuilder();

      if (includeLowercase) characterSet.Append(Lowercase);
      if (includeUppercase) characterSet.Append(Uppercase);
      if (includeDigits) characterSet.Append(Digits);
      if (includeSpecialCharacters) characterSet.Append(SpecialCharacters);

      if (characterSet.Length == 0)
         throw new ArgumentException("At least one character set must be included.");

      var random = new Random();
      return new string(Enumerable.Repeat(characterSet.ToString(), length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
   }
}
