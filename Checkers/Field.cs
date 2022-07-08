using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Field
    {
        public delegate void GameOverHandler(string message);
        public event GameOverHandler? GameOverNotify;

        private readonly string[][] _field = new string[8][];
        public Player PlayerOne { get; private set; }
        public Player PlayerTwo { get; private set; }
        public string EmptyMark { get; private set; }
        public string[][] Get => _field;
        public Field(Player player1, Player player2, string empty = " ")
        {
            PlayerOne = player1;
            PlayerTwo = player2;
            EmptyMark = empty;
            Initialization();
        }
        public bool Step(Man man, int row, int column, out bool isContinue/*, out GameStatus endOfGame*/)
        {
            if (CanMove(man, row, column, out isContinue/*, out endOfGame*/))
            {
                Rewrite();
                return true;
            }
            return false;
        }

        private void Rewrite()
        {
            for (int i = 0; i < _field.Length; i++)
                for (int j = 0; j < _field[i].Length; j++)
                    _field[i][j] = EmptyMark;

            foreach (var man in PlayerOne.Men)
                _field[man.Row][man.Column] = man.Mark;

            foreach (var man in PlayerTwo.Men)
                _field[man.Row][man.Column] = man.Mark;
        }
        private void Initialization()
        {
            for (int i = 0; i < _field.Length; i++)
                _field[i] = new string[_field.Length];
            Rewrite();
        }
        private bool IsAvailableCells(int row, int column)
        {
            bool even = false;
            for (int i = 0; i < _field.Length; i++)
            {
                for (int j = even ? 0 : 1; j < _field[i].Length; j += 2)
                    if (i == row && j == column)
                        return true;
                even = !even;
            }
            return false;
        }
        private bool CanMove(Man man, int row, int column, out bool isContinue/*, out GameStatus endOfGame*/)
        {
            isContinue = false;
            //endOfGame = GameStatus.Continue;
            int rowForUpgrade;

            bool result = false;

            Player player, enemy;
            if (PlayerOne.Mark == man.Mark.ToLower())
            {
                player = PlayerOne;
                enemy = PlayerTwo;
                rowForUpgrade = 0;
            }
            else
            {
                player = PlayerTwo;
                enemy = PlayerOne;
                rowForUpgrade = _field.Length - 1;
            }

            List<Route> priorityMoves = new List<Route>();
            List<Route> possibleMoves = new List<Route>();
            foreach (Man m in player.Men)
                CheckAvailableMoves(m, player, enemy, ref priorityMoves, ref possibleMoves);

            if(priorityMoves.Any())
            {
                var route = priorityMoves.FirstOrDefault(x => x.PlayersMan == man && x.Row == row && x.Column == column);
                if(route != null && route.TryGetEnemysMan(out var enemysMan))
                {
                    enemy.KillMan(enemysMan);
                    result = true;

                    man.Step(row, column);

                    priorityMoves.Clear();
                    possibleMoves.Clear();

                    Upgrade(result, row, rowForUpgrade, ref man);

                    CheckAvailableMoves(man, player, enemy, ref priorityMoves, ref possibleMoves);
                    if (priorityMoves.Any())
                        isContinue = true;
                }
            }
            else
            {
                var route = possibleMoves.FirstOrDefault(x => x.PlayersMan == man && x.Row == row && x.Column == column);
                if (route != null)
                {
                    result = true;
                    man.Step(row, column);
                    Upgrade(result, row, rowForUpgrade, ref man);
                }
            }

            //if (result && row == rowForUpgrade)
            //    man.Upgrade();

            priorityMoves.Clear();
            possibleMoves.Clear();
            foreach (Man m in player.Men)
                CheckAvailableMoves(m, player, enemy, ref priorityMoves, ref possibleMoves);
            foreach (Man m in enemy.Men)
                CheckAvailableMoves(m, enemy, player, ref priorityMoves, ref possibleMoves);

            if (!(priorityMoves.Any() || possibleMoves.Any()))
                //endOfGame = GameStatus.Draw;
                GameOverNotify?.Invoke("Ничья!");

            if (!enemy.Men.Any())
                //endOfGame = GameStatus.Victory;
                GameOverNotify?.Invoke($"Победил {player.Name}!");

            return result;
        }
        private static void Upgrade(bool result, int row, int rowForUpgrade, ref Man man)
        {
            if (result && row == rowForUpgrade)
                man.Upgrade();
        }
        private bool IsStepBack(int row, int rowDestination, bool isBeginner) => isBeginner ? row < rowDestination : row > rowDestination;
        private void CheckAvailableMoves(Man man, Player player, Player enemy, ref List<Route> priorityMoves, ref List<Route> possibleMoves)
        {
            if (man.IsKing)
            {
                int[] rowDirections = new int[2] { -1, 1 };  //up   | down
                int[] columnDirections = new int[2] { -1, 1 };  //left | right
                foreach (int rowDirection in rowDirections)
                {
                    foreach (int columnDirection in columnDirections)
                    {
                        int stepRow = man.Row;
                        int stepColumn = man.Column;
                        while (stepRow < _field.Length && stepRow >= 0 && stepColumn < _field.Length && stepColumn >= 0)
                        {
                            stepRow += rowDirection;
                            stepColumn += columnDirection;
                            if (enemy.TryGetMan(stepRow, stepColumn, out var enemyMan))
                            {
                                int afterEnemyRow = stepRow,
                                    afterEnemyColumn = stepColumn;
                                while (afterEnemyRow < _field.Length && afterEnemyRow >= 0 && afterEnemyColumn < _field.Length && afterEnemyColumn >= 0)
                                {
                                    afterEnemyRow += rowDirection;
                                    afterEnemyColumn += columnDirection;
                                    if (IsAvailableCells(afterEnemyRow, afterEnemyColumn) && !enemy.IsManExist(afterEnemyRow, afterEnemyColumn) && !player.IsManExist(afterEnemyRow, afterEnemyColumn))
                                        priorityMoves.Add(new Route(man, enemyMan, afterEnemyRow, afterEnemyColumn));
                                }
                            }
                            else if (IsAvailableCells(stepRow, stepColumn) && !player.IsManExist(stepRow, stepColumn))
                            {
                                possibleMoves.Add(new Route(man, stepRow, stepColumn));
                            }
                        }
                    }
                }
            }
            else
            {
                int rowDirection = player.IsBeginner ? -1 : 1;  //up   | down
                int[] columnDirections = new int[2] { -1, 1 };  //left | right

                
                foreach (int columnDirection in columnDirections)
                {
                    int stepRow = man.Row + rowDirection;
                    int stepColumn = man.Column + columnDirection;
                    if (enemy.TryGetMan(stepRow, stepColumn, out var enemyMan))
                    {
                        stepRow += rowDirection;
                        stepColumn += columnDirection;
                        if (IsAvailableCells(stepRow, stepColumn) && !enemy.IsManExist(stepRow, stepColumn) && !player.IsManExist(stepRow, stepColumn) )
                            priorityMoves.Add(new Route(man, enemyMan, stepRow, stepColumn));
                    }
                    else if (IsAvailableCells(stepRow, stepColumn) && !player.IsManExist(stepRow, stepColumn))
                    {
                        possibleMoves.Add(new Route(man, stepRow, stepColumn));
                    }
                }
            }
        }
    }
}
