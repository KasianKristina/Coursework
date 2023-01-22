using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    class FigureQueenBlack : Figure
    {
        public FigureQueenBlack(ref Field GameField)
        {
            this.GameField = GameField;
            GameField[7, 3] = this.Id;
        }
        public override int Id
        {
            get { return -4; }
        }

        public override Color Color { get { return Color.Black; } }

        protected override Position StartOffset { get { return new Position(7, 3); } }
    }
}
