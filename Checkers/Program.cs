using System;

namespace Checkers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player1 = new Player("One", "X", true);
            Player player2 = new Player("Two", "O");
            Field field = new Field(player1, player2);
            ConsoleView.PrintField(field.Get);
            Console.ReadKey();
            Console.Clear();

            var man = player1.Men.First(x => x.Coordinate.X == 5 && x.Coordinate.Y == 2);
            field.Step(man, new Coordinate(4, 1));
            ConsoleView.PrintField(field.Get);
        }
    }
}