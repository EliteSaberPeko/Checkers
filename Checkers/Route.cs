using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Route
    {
        private Man? _enemysMan;
        public Man PlayersMan { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Route(Man playersMan, Man? enemysMan, int row, int column)
        {
            PlayersMan = playersMan;
            _enemysMan = enemysMan;
            Row = row;
            Column = column;
        }
        public Route(Man playersMan, int row, int column) : this(playersMan, null, row, column)
        {

        }
        public bool TryGetEnemysMan(out Man? enemysMan)
        {
            enemysMan = _enemysMan;
            return enemysMan != null;
        }
    }
}
