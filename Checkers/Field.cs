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
        public void Step(Man man, int row, int column)
        {
            man.Step(row, column);
            Rewrite();
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
    }
}
