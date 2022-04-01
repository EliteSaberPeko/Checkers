using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public static class ConsoleView
    {
        public static void PrintField(string[][] field)
        {
            int[] range = new int[field.Length];
            for (int i = 0; i < field.Length; i++)
                range[i] = i;
            Console.WriteLine($" |{string.Join("|", range)}|");
            for (int x = 0; x < field.Length; x++)
                Console.WriteLine($"{x}|{string.Join("|", field[x])}|");
        }
    }
}
