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

            /*ConsoleView view = new ConsoleView(field);
            view.Start(player1, player2);*/

            Pseudographics pseudographics = new Pseudographics(field);
            pseudographics.Start(player1, player2);
        }
    }
}