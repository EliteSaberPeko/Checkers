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
        private bool _isGameOver = false;
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
        public void Start(Player player1, Player player2)
        {
            _field.GameOverNotify += IsGameOver;
            while (!_isGameOver)
            {
                Step(player1);
                if (_isGameOver)
                    break;
                Step(player2);
            }
        }
        private void Step(Player player)
        {
            Console.Clear();
            bool isSuccess = false;
            bool isContinue = false;
            Man? man = null;
            while (!isSuccess)
            {
                if (!isContinue)
                    man = ChooseMan(player);
                Console.WriteLine("Ходит: " + player.Name);
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
                isSuccess = _field.Step(man, x, y, out isContinue);
                if (!isSuccess)
                {
                    Console.Clear();
                    Console.WriteLine("Невозможно сделать ход!");
                }
                if (isContinue)
                {
                    isSuccess = false;
                    player.TryGetMan(x, y, out man);
                }
            }

            //isGameOver = GameOver(player, endOfGame);
            PrintField();
        }
        private Man ChooseMan(Player player)
        {
            Man man = new Man();
            bool wrongMan = true;
            while (wrongMan)
            {
                Console.WriteLine("Ходит: " + player.Name);
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
                wrongMan = !player.TryGetMan(x, y, out man);
                if (wrongMan)
                    Console.WriteLine("Шашка не найдена!");
            }
            return man;
        }
        private bool GameOver(Player player, GameStatus endOfGame)
        {
            bool isGameOver = true;
            switch(endOfGame)
            {
                case GameStatus.Victory:
                    Console.WriteLine($"Победил {player.Name}!");
                    break;
                case GameStatus.Draw:
                    Console.WriteLine("Ничья!");
                    break;
                default:
                    isGameOver = false;
                    break;
            }
            return isGameOver;
        }
        private void IsGameOver(string message)
        {
            Console.WriteLine(message);
            _isGameOver = true;
        }
    }
}
