using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Player
    {
        public string Name { get; private set; }
        public string Mark { get; private set; }
        public List<Man> Men { get; private set; }
        public bool CanMove { get; private set; }
        public Player(string name, string mark, bool canMove = false)
        {
            Name = name;
            Mark = mark.ToLower();
            CanMove = canMove;
            Men = new List<Man>();
            Initialization();
        }
        public bool KillMan(Man man) => Men.Remove(man);
        public Man? GetMan(int row, int column) => Men.FirstOrDefault(x => x.Row == row && x.Column == column);

        private void Initialization()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (CanMove)
                    {
                        if ((x == 5 || x == 7) && y % 2 == 0 || x == 6 && y % 2 != 0)
                            Men.Add(new Man(x, y, Mark));
                    }
                    else
                    {
                        if ((x == 0 || x == 2) && y % 2 != 0 || x == 1 && y % 2 == 0)
                            Men.Add(new Man(x, y, Mark));
                    }
                }
            }
        }
    }
}
