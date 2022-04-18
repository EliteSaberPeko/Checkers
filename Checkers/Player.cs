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
        public bool IsBeginner { get; private set; }
        public Player(string name, string mark, bool isBeginner = false)
        {
            Name = name;
            Mark = mark.ToLower();
            IsBeginner = isBeginner;
            Men = new List<Man>();
            Initialization();
        }
        public bool KillMan(Man man) => Men.Remove(man);
        //public Man? GetMan(int row, int column) => Men.FirstOrDefault(x => x.Row == row && x.Column == column);
        public bool TryGetMan(int row, int column, out Man man)
        {
            if (IsManExist(row, column))
            {
                man = Men.First(x => x.Row == row && x.Column == column);
                return true;
            }
            man = new Man();
            return false;
        }
        public bool IsManExist(int row, int column) => Men.Any(x => x.Row == row && x.Column == column);

        private void Initialization()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (IsBeginner)
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
