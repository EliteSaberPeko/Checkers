using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class ConsoleView
    {
        private Field _field;
        public ConsoleView(Field field)
        {
            _field = field;
        }
        public void PrintField()
        {
            var field = _field.Get;
            int[] range = new int[field.Length];
            for (int i = 0; i < field.Length; i++)
                range[i] = i;
            Console.WriteLine($" |{string.Join("|", range)}|");
            for (int x = 0; x < field.Length; x++)
                Console.WriteLine($"{x}|{string.Join("|", field[x])}|");
        }
        private Man? ChooseMan(Player player)
        {
            Man? man;
            do
            {
                PrintField();
                int x, y;
                do
                {
                    Console.Write("Введите номер строки шашки: ");
                } while (!int.TryParse(Console.ReadLine(), out x));
                do
                {
                    Console.Write("Введите номер столбца шашки: ");
                } while (!int.TryParse(Console.ReadLine(), out y));
                Console.Clear();
                man = player.GetMan(x, y);
                if (man == null)
                    Console.WriteLine("Шашка не найдена!");
            } while (man == null);
            return man;
        }
        public void Step(Player player)
        {
            var man = ChooseMan(player);
            PrintField();
            int x, y;
            do
            {
                Console.Write("Введите номер строки назначения шашки: ");
            } while (!int.TryParse(Console.ReadLine(), out x));
            do
            {
                Console.Write("Введите номер столбца назначения шашки: ");
            } while (!int.TryParse(Console.ReadLine(), out y));
            Console.Clear();
            _field.Step(man, x, y);
            PrintField();
        }
    }
}
