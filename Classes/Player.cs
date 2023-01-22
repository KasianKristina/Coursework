using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Player
    {
        public Figure king;
        protected Figure queen;
        protected Color Color;
        public Field GameField;

        public Player(Color color, ref Field GameField)
        {
            this.Color = color;
            this.GameField = GameField;
            if (color == Color.White)
            {
                king = new FigureKing(ref GameField);
                queen = new FigureQueen(ref GameField);
            }
            else
            {
                king = new FigureKingBlack(ref GameField);
                queen = new FigureQueenBlack(ref GameField);
            }
        }

        public void Wave(int startX, int startY, int finishX, int finishY)
        {
            int result, fx, fy, x, y;
            Field cMap = Function.CreateWave(startX, startY, finishX, finishY, false, GameField);

            result = cMap[finishX, finishY];
            x = finishX;
            y = finishY;
            cMap.Draw();
            (fx, fy) = Function.Search(x, y, result, ref cMap, false);
            //startKing.MoveBlock(fx, fy);

            cMap.Draw();
        }

        public void StrategySimple(int motion)
        {
            if (motion % 6 == 0)
            {
                queen.MoveDetailLeft
            }
        }
    }
}
