using System;
using System.Text.RegularExpressions;

namespace PasswordEncrypt
{
    internal class Program
    {
        // requires at least one letter and at least one digit
        static Regex regex = new("/^(?:[0-9]+[a-z]|[a-z]+[0-9])[a-z0-9]*$/i");//, RegexOptions.IgnoreCase);

        private static bool CheckPasswordStrengthRegex(string input) // does not work normally
        {
            // and not less than 8 chars
            return input.Length > 7 && regex.IsMatch(input);
        }

        private static bool CheckPasswordStrength(string? pass)
        {
            bool containsDigit = false;
            bool containsLetter = false;
            if (pass is null ||
                pass.Length < 8) return false;
            for (int i = 0; i < pass.Length; i++)
                if (pass[i] >= '0' && pass[i] <= '9')
                    containsDigit = true;
            for (int i = 0; i < pass.Length; i++)
                if (pass[i] >= 'A' && pass[i] <= 'Z' ||
                    pass[i] >= 'a' && pass[i] <= 'z')
                    containsLetter = true;
            return containsDigit && containsLetter;
        }
        
        private static string EncryptOrDecryptPassword(string password)
        // in this case, encryption and decryption use the same operation :)
        {
            string result = "";
            for (int i = 0; i < password.Length; i++)
                result += (char)(password[i] ^ Int16.MaxValue);   // XOR with 1111111111111111
            return result;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("Enter password");
            string s = Console.ReadLine();
            Console.WriteLine("Encrypted password: ");
            string es = EncryptOrDecryptPassword(s);
            Console.WriteLine(es + "\r\nDecrypted password: ");
            string des = EncryptOrDecryptPassword(es);
            Console.WriteLine(des);
            Console.WriteLine("The password is " + (CheckPasswordStrength(s) ? "strong!" : "weak¡"));
        }
    }
}
