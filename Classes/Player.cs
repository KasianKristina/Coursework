using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Player
    {
        public FigureKing king;
        public FigureQueen queen;
        public Color Color;
        public Field GameField;

        public Player(Color color, ref Field GameField)
        {
            this.Color = color;
            this.GameField = GameField;
            if (color == Color.White)
            {
                king = new FigureKing(ref GameField, color);
                queen = new FigureQueen(ref GameField, color);
            }
            else
            {
                king = new FigureKing(ref GameField, color);
                queen = new FigureQueen(ref GameField, color);
            }
        }

        public void Wave(int startX, int startY, int finishX, int finishY, FigureKing figure)
        {
            int result, fx, fy, x, y;
            Field cMap = Function.CreateWave(startX, startY, finishX, finishY, false, GameField);

            result = cMap[finishX, finishY];
            x = finishX;
            y = finishY;
            cMap.Draw();
            (fx, fy) = Function.Search(x, y, result, ref cMap, false);
            figure.MoveFigureKing(fx, fy);

            cMap.Draw();
        }

        public void StrategySimple(int motion)
        {
            if (motion % 6 == 0)
            {
                //queen.MoveDetailLeft
            }
            else
            {
                if (Color == Color.White)
                {
                    Wave(king.offset.Row, king.offset.Column, 7, 4, king);
                }
                else
                {
                    Wave(king.offset.Row, king.offset.Column, 0, 4, king);
                }
            }
        }
    }
}
