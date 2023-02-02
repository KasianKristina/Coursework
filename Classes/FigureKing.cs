using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class FigureKing : Figure
    {
        public FigureKing(ref Field GameField, Color color)
        {
            if (color != Color.Black || color != Color.White)
                throw new Exception("color black or white");
            this.GameField = GameField;
            if (color == Color.White)
            {
                this.Id = -1;
                GameField[0, 4] = this.Id;
            }
            else
            {
                this.Id = -3;
                GameField[7, 4] = this.Id;
            }
            this.Color = color;
        }

        public void MoveFigureKing(int x, int y)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (GameField[i, j] == -7)
                        GameField[i, j] = 0;
                }
            }
        }

        // проверка, что король может сходить
        public bool OpportunityToMakeMoveKing(int x, int y, int xOpponent, int yOpponent, int motion)
        {
            if ((motion <= 16 || (motion > 16 && LeaveSquare(x, y))) &&
                AdjacentPosition(x, y, xOpponent, yOpponent) &&
                LeaveSquare(x, y) &&
                GameField[x, y] != -5 &&
                GameField[x, y] != -7)
                return true;
            else return false;
        }

        // проверка: король должен покинуть квадрат за 16 ходов и больше не возвращаться в него
        // x, y - позиция короля, которую он займет при передвижении
        public bool LeaveSquare(int x, int y)
        {
            int Row = 0;
            int iterator = 1;
            if (this.Color == Color.Black)
            {
                Row += 7;
                iterator = -1;
            }
            if ((x, y) == (Row, 3) || (x, y) == (Row + iterator * 2, 3) || (x, y) == (Row + iterator * 3, 3) ||
                 (x, y) == (Row, 4) || (x, y) == (Row + iterator * 2, 4) || (x, y) == (Row + iterator * 3, 4) ||
                 (x, y) == (Row, 5) || (x, y) == (Row + iterator * 2, 5) || (x, y) == (Row + iterator * 3, 5))
                return false;
            return true;
        }

        // проверка: король находится в смежной позиции с королем противника
        // x, y - координаты короля; xOpponent, yOpponent - координаты короля противника
        public bool AdjacentPosition(int x, int y, int xOpponent, int yOpponent)
        {
            if (GameField[x, y] == GameField[xOpponent - 1, yOpponent] ||
                GameField[x, y] == GameField[xOpponent - 1, yOpponent - 1] ||
                GameField[x, y] == GameField[xOpponent, yOpponent - 1] ||
                GameField[x, y] == GameField[xOpponent + 1, yOpponent - 1] ||
                GameField[x, y] == GameField[xOpponent + 1, yOpponent] ||
                GameField[x, y] == GameField[xOpponent + 1, yOpponent + 1] ||
                GameField[x, y] == GameField[xOpponent, yOpponent + 1] ||
                GameField[x, y] == GameField[xOpponent - 1, yOpponent + 1])
                return false;
            else return true;
        }

    }
}
