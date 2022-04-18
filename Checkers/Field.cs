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
        public bool Step(Man man, int row, int column)
        {
            if (!CanMove(man, ref row, ref column))
                return false;
            man.Step(row, column);
            Rewrite();
            return true;
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
        private bool CanMove(Man man, ref int row, ref int column)
        {
            if (!IsAvailableCells(row, column))
                return false;

            Player player, enemy;
            if (PlayerOne.Mark == man.Mark)
            {
                player = PlayerOne;
                enemy = PlayerTwo;
            }
            else
            {
                player = PlayerTwo;
                enemy = PlayerOne;
            }

            if (player.TryGetMan(row, column, out _))
                return false;

            if (man.IsKing)
            {
                //тут дамки
            }
            else
            {
                if (IsStepBack(man.Row, row, player.IsBeginner))
                    return false;

                int stepRow = Math.Abs(man.Row - row);
                int stepColumn = Math.Abs(man.Column - column);
                bool isAttack = stepRow == 2 && stepColumn == 2;
                bool isCorrectStep = isAttack || (stepRow == 1 && stepColumn == 1);
                if (!isCorrectStep)
                    return false;

                if (isAttack)
                {
                    stepRow = man.Row + ((row - man.Row) / 2);
                    stepColumn = man.Column + ((column - man.Column) / 2);

                    if (player.IsManExist(stepRow, stepColumn))
                        return false;
                    Man manFromDestinationCell;
                    if (enemy.TryGetMan(stepRow, stepColumn, out manFromDestinationCell))
                    {
                        if (enemy.IsManExist(row, column) || player.IsManExist(row, column))
                            return false;
                        enemy.KillMan(manFromDestinationCell);
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return !(enemy.IsManExist(row, column) || player.IsManExist(row, column));
                    //manFromDestinationCell = enemy.GetMan(row, column);
                    //if (manFromDestinationCell == null)
                    //    return true;
                    //else
                    //{
                    //    stepRow = man.Row + ((row - man.Row) * 2);
                    //    stepColumn = man.Column + ((column - man.Column) * 2);
                    //    if (enemy.GetMan(stepRow, stepColumn) != null || player.GetMan(stepRow, stepColumn) != null)
                    //    {
                    //        return false;
                    //    }
                    //    enemy.KillMan(manFromDestinationCell);
                    //    row = stepRow;
                    //    column = stepColumn;
                    //    return true;
                    //}
                }
            }

            return false;//пока без дамок
        }
        private bool IsStepBack(int row, int rowDestination, bool isBeginner) => isBeginner ? row < rowDestination : row > rowDestination;
    }
}
