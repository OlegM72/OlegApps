using System;

namespace PasswordEncrypt
{
    internal class Program
    {
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
        }
    }
}
