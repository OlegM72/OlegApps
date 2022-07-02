using System;
using System.IO;

namespace PartialCopy
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Partial File Copy. (c) 2022 Oleg V. Melnikov.\r\n" +
                                  "Parameters: <source file name> <destination file name> <desired size (<= original size)>");
                return -1;
            }
            try
            {
                using (FileStream reader = File.OpenRead(args[0]))
                using (FileStream writer = File.OpenWrite(args[1]))
                {
                    if (!Int32.TryParse(args[2], out int desiredSize))
                        throw new ArgumentException("Wrong desired size");
                    if (desiredSize > reader.Length)
                        throw new ArgumentException("Desired size is larger than the original size");
                    if (File.Exists(args[1]))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("The destination file exists. Do you want to overwrite it? (Y/N) ");
                        Console.ResetColor();
                        if (Console.ReadLine().ToUpper() != "Y")
                            return -2;
                    }
                    long read = 0; // counter of total bytes read
                    int readCurrBlock = 0; // current block read size
                    byte[] buffer = new byte[100000000];
                    bool stop = false;
                    while (!stop && (readCurrBlock = reader.Read(buffer)) != 0)
                    {
                        read += (long)readCurrBlock;
                        if (read >= desiredSize)
                        {
                            readCurrBlock -= (int)(read - desiredSize); // decrease the buffer if we need less size
                            stop = true;
                        }
                        writer.Write(buffer, 0, readCurrBlock);
                        if (stop)
                        {
                            writer.Flush();
                            Console.WriteLine($"{desiredSize} bytes copied.");
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                int error = ex.GetHashCode();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error {error}, {ex.Message}");
                Console.ResetColor();
                return error;
            }
        }
    }
}
