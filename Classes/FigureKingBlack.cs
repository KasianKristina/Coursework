﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class FigureKingBlack : Figure
    {
        public FigureKingBlack(ref Field GameField)
        {
            this.GameField = GameField;
        }
        public override int Id
        {
            get { return -3; }
        }
        public override Color Color { get { return Color.White; } }
        protected override Position StartOffset { get { return new Position(7, 4); } }
    }
}
