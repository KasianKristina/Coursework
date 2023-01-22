using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class FigureKing : Figure
    {
        public FigureKing(ref Field GameField)
        {
            this.GameField = GameField;
            GameField[0, 4] = this.Id;
        }
        public override int Id
        {
            get { return -1; }
        }
        public override Color Color { get { return Color.White; } }
        protected override Position StartOffset { get { return new Position(0, 4); } }
    }
}
