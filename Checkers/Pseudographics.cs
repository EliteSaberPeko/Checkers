using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Pseudographics
    {
        private readonly int _boardCenter = (Console.WindowWidth - 35) / 2;
        private Field _field;
        private string _gameOverNotification = "";
        public Pseudographics(Field field)
        {
            _field = field;
        }
        public void PrintField(int row, int col, Man? man = null)
        {
            var field = _field.Get;
            int[] range = new int[field.Length];
            for (int i = 0; i < field.Length; i++)
                range[i] = i;

            Console.CursorLeft = _boardCenter;
            Console.Write("  ");
            for (int i = 0; i < field[0].Length; i++)
            {
                char c = (char)('A' + i);
                Console.Write("  " + c + " ");
            }
            Console.WriteLine();
            for (int x = 0; x < field.Length; x++)
            {
                Console.CursorLeft = _boardCenter;

                for (int i = -1; i < field[x].Length; i++)
                {
                    if (i == -1)
                        Console.Write("  ");
                    Console.Write("\u253C");
                    if(i + 1 < field[x].Length)
                        Console.Write("\u2500\u2500\u2500");
                }
                Console.WriteLine();
                Console.CursorLeft = _boardCenter;
                Console.Write(x + " ");
                for (int i = 0; i < field[x].Length; i++)
                {

                    Console.Write("\u2502");
                    if(man != null && x == man.Row && i == man.Column)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (x == row && i == col)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write($" {field[x][i]} ");
                    Console.ResetColor();
                }
                Console.Write("\u2502");
                Console.WriteLine();
            }
            Console.CursorLeft = _boardCenter;
            for (int i = -1; i < field[0].Length; i++)
            {
                if (i == -1)
                    Console.Write("  ");
                Console.Write("\u253C");
                if (i + 1 < field[0].Length)
                    Console.Write("\u2500\u2500\u2500");
            }
            Console.WriteLine();
        }
        public void Start(Player player1, Player player2)
        {
            _field.GameOverNotify += IsGameOver;
            while (!_gameOverNotification.Any())
            {
                Step(player1);
                if (_gameOverNotification.Any())
                    break;
                Step(player2);
            }
        }
        private void Step(Player player)
        {
            Console.Clear();
            int max = _field.Get.Length;
            bool isSuccess = false;
            bool isContinue = false;
            Man man = ChooseMan(player);
            int row = man.Row, col = man.Column;
            Console.Clear();
            Console.WriteLine("Продолжает ходить: " + player.Name);
            PrintField(row, col, man);
            while (!isSuccess)
            {
                string errorMessage = "";
                ConsoleKey inputKey = Console.ReadKey().Key;
                switch (inputKey)
                {
                    case ConsoleKey.Enter:
                        isSuccess = _field.Step(man, row, col, out isContinue);
                        if (!isSuccess)
                            errorMessage = "Невозможно сделать ход!";
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        man = ChooseMan(player);
                        break;
                    default:
                        WASDKeys(inputKey, max, ref row, ref col);
                        break;
                }
                Console.Clear();
                if (isContinue)
                {
                    isSuccess = false;
                    player.TryGetMan(row, col, out man);
                }
                if (errorMessage.Any())
                    Console.WriteLine(errorMessage);

                if (_gameOverNotification.Any())
                    Console.WriteLine(_gameOverNotification);
                else
                    Console.WriteLine("Продолжает ходить: " + player.Name);
                PrintField(row, col, man);
            }
        }
        private Man ChooseMan(Player player)
        {
            Man man = new Man();
            int max = _field.Get.Length;
            bool next = true;
            int row = -1, col = -1;
            var z = player.Men.FirstOrDefault();
            Console.Clear();
            Console.WriteLine("Ходит: " + player.Name);
            if (z != null)
            {
                row = z.Row;
                col = z.Column;
            }
            PrintField(row, col);
            while (next)
            {
                ConsoleKey inputKey = Console.ReadKey().Key;
                switch (inputKey)
                {
                    case ConsoleKey.Enter:
                        if (player.TryGetMan(row, col, out man))
                            next = false;
                        break;
                    default:
                        WASDKeys(inputKey, max, ref row, ref col);
                        break;
                }
                Console.Clear();
                Console.WriteLine("Ходит: " + player.Name);
                PrintField(row, col);
            }
            return man;
        }

        private void WASDKeys(ConsoleKey inputKey, int max, ref int row, ref int col)
        {
            switch (inputKey)
            {
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    col++;
                    if (col >= max)
                        col = 0;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    col--;
                    if (col < 0)
                        col = max - 1;
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    row--;
                    if (row < 0)
                        row = max - 1;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    row++;
                    if (row >= max)
                        row = 0;
                    break;
            }
        }
        private void IsGameOver(string message) => _gameOverNotification = message;
    }
}
