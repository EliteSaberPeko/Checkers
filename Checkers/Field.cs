using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Field
    {
        private string[][] _field = new string[8][];
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
        public bool Step(Man man, int row, int column, out bool isContinue)
        {
            if (CanMove(man, row, column, out isContinue))
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
        private bool CanMove(Man man, int row, int column, out bool isContinue)
        {
            isContinue = false;
            int rowForUpgrade;
            if (!IsAvailableCells(row, column))
                return false;

            bool result = false;

            Player player, enemy;
            if (PlayerOne.Mark == man.Mark)
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
                }
            }

            if (result && row == rowForUpgrade)
                man.Upgrade();

            return result;//пока без дамок
        }
        private bool IsStepBack(int row, int rowDestination, bool isBeginner) => isBeginner ? row < rowDestination : row > rowDestination;
        private void CheckAvailableMoves(Man man, Player player, Player enemy, ref List<Route> priorityMoves, ref List<Route> possibleMoves)
        {
            if (man.IsKing)
            {
                //тут дамки
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
                    else if (!player.IsManExist(stepRow, stepColumn))
                    {
                        possibleMoves.Add(new Route(man, stepRow, stepColumn));
                    }
                }
            }
        }
    }
}
