using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.Core
{
    public static class ConsoleUtility
    {
        public static void Trace(string msg, [CallerMemberName] string methodName = "A method without a name :(")
        {
            WriteLine("TRACE", ConsoleColor.Black, ConsoleColor.White, $"{methodName} - {msg}");
        }

        public static void Info(string msg)
        {
            WriteLine("INFO", ConsoleColor.Blue, ConsoleColor.Black, msg);
        }

        public static void Error(string msg, Exception ex)
        {
            WriteSeperator(ConsoleColor.Red, ConsoleColor.Black);
            WriteLine("ERROR", ConsoleColor.Red, ConsoleColor.Black, $"{msg}: Exception: {ex.Message}");
            WriteSeperator(ConsoleColor.Red, ConsoleColor.Black);
        }

        public static void Warning(string msg)
        {
            WriteLine("WARNING", ConsoleColor.Yellow, ConsoleColor.Black, msg);
        }

        private static void WriteSeperator(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine("--------------------------------------------------------------------------------------------------------");
        }

        private static void WriteLine(string tag, ConsoleColor foregroundColor, ConsoleColor backgroundColor, string msg)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            var time = DateTime.Now.ToString("o");

            Console.WriteLine($"{time}-[{tag}]: {msg}");
        }

        
    }
}
