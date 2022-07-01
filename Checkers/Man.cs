using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Man
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public string Mark { get; private set; }
        public bool IsKing { get; private set; }
        public Man(int row, int column, string mark)
        {
            Row = row;
            Column = column;
            Mark = mark;
            IsKing = false;
        }
        public Man() : this(-1, -1, "")
        {

        }
        public void Step(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public void Upgrade()
        {
            if (!IsKing)
            {
                Mark = Mark.ToUpper();
                IsKing = true;
            }
        }
    }
}
