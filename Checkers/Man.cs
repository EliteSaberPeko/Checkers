using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Man
    {
        public Coordinate Coordinate { get; private set; }
        public string Mark { get; private set; }
        public bool IsKing { get; private set; }
        public Man(Coordinate coordinate, string mark)
        {
            Coordinate = coordinate;
            Mark = mark;
            IsKing = false;
        }
        public void Step(Coordinate coordinate) => Coordinate = coordinate;
        public void Upgrade()
        {
            Mark = Mark.ToUpper();
            IsKing = true;
        }
    }
}
