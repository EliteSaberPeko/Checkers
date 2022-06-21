using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Pseudographics
    {
        private const string BORDER_CROSS = "\u253C";
        private const string BORDER_HORIZONTAL = "\u2500\u2500\u2500";
        private const string BORDER_VERTICAL = "\u2502";
        private readonly int _boardCenter = (Console.WindowWidth - 35) / 2;
        private Field _field;
        private string _gameOverNotification = "";
        public Pseudographics(Field field)
        {
            _field = field;
        }
        public void PrintField(int row, int col, string? messages = null, Man? man = null)
        {
            var field = _field.Get;
            int[] range = new int[field.Length];
            for (int i = 0; i < field.Length; i++)
                range[i] = i;

            Console.Clear();
            if (messages != null)
                Console.WriteLine(messages);
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
                    Console.Write(BORDER_CROSS);
                    if (i + 1 < field[x].Length)
                        Console.Write(BORDER_HORIZONTAL);
                }
                Console.WriteLine();
                Console.CursorLeft = _boardCenter;
                Console.Write(x + " ");
                for (int i = 0; i < field[x].Length; i++)
                {

                    Console.Write(BORDER_VERTICAL);
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
                Console.Write(BORDER_VERTICAL);
                Console.WriteLine();
            }
            Console.CursorLeft = _boardCenter;
            for (int i = -1; i < field[0].Length; i++)
            {
                if (i == -1)
                    Console.Write("  ");
                Console.Write(BORDER_CROSS);
                if (i + 1 < field[0].Length)
                    Console.Write(BORDER_HORIZONTAL);
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
            int max = _field.Get.Length;
            bool isSuccess = false;
            bool isContinue = false;
            Man man = ChooseMan(player);
            int row = man.Row, col = man.Column;
            string message = "Продолжает ходить: " + player.Name + Environment.NewLine;
            string errorMessage = "";
            PrintField(row, col, message, man);
            while (!isSuccess)
            {
                ConsoleKey inputKey = Console.ReadKey(true).Key;
                switch (inputKey)
                {
                    case ConsoleKey.Enter:
                        isSuccess = _field.Step(man, row, col, out isContinue);
                        errorMessage = !isSuccess ? "Невозможно сделать ход!" + Environment.NewLine : "";
                        break;
                    case ConsoleKey.Escape:
                        if (!isContinue)
                        {
                            errorMessage = "";
                            man = ChooseMan(player);
                            row = man.Row;
                            col = man.Column;
                        }
                        break;
                    default:
                        WASDKeys(inputKey, max, ref row, ref col);
                        break;
                }
                if (isContinue && isSuccess)
                {
                    isSuccess = false;
                    player.TryGetMan(row, col, out man);
                }

                if (_gameOverNotification.Any())
                    message = _gameOverNotification;
                PrintField(row, col, errorMessage + message, man);
            }
        }
        private Man ChooseMan(Player player)
        {
            Man man = new Man();
            int max = _field.Get.Length;
            bool next = true;
            int row = -1, col = -1;
            var z = player.IsBeginner ?
                player.Men.OrderBy(x => x.Column).ThenBy(x => x.Row).FirstOrDefault() :
                player.Men.OrderBy(x => x.Column).ThenByDescending(x => x.Row).FirstOrDefault();
            
            string messages = "Ходит: " + player.Name + Environment.NewLine;
            if (z != null)
            {
                row = z.Row;
                col = z.Column;
            }
            PrintField(row, col, messages);
            while (next)
            {
                ConsoleKey inputKey = Console.ReadKey(true).Key;
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
                PrintField(row, col, messages);
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
