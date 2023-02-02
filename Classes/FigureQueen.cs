using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class FigureQueen : Figure
    {
        public FigureQueen(ref Field GameField, Color color)
        {
            if (color != Color.Black || color != Color.White)
                throw new Exception("color black or white");
            this.GameField = GameField;
            if (color == Color.White)
            {
                this.Id = -2;
                GameField[0, 3] = this.Id;
            }
            else
            {
                this.Id = -4;
                GameField[7, 3] = this.Id;
            }
            this.Color = color;
        }

        // проверка, что ферзь может сходить
        public bool OpportunityToMakeMoveQueen(int x, int y, int xNext, int yNext, int motion, bool flag)
        {
            if (QueenOnHorizontal(x, y, xNext, yNext, motion, flag) &&
                GameField[x, y] != -5 &&
                GameField[x, y] != -7)
                return true;
            else return false;
        }

        // проверка: ферзь не может находиться на одной горизонтали дольше 5 ходов, если он не заблокирован (иначе поражение)
        public bool QueenOnHorizontal(int x, int y, int xNext, int yNext, int motion, bool flag)
        {
            if (QueenNotHorizontal(x, xNext, motion, flag))
                return true;
            else if (QueenIsNotLocked(x, y))
            {
                GameOver = true;
                return false;
            }
            else return true;
        }

        // ферзь не находится на одной горизонтали дольше 5 ходов
        public bool QueenNotHorizontal(int x, int xNext, int motion, bool flag)
        {
            if (motion > 5 && flag && x == xNext)
                return false;
            else return true;
        }

        // ферзь заблокирован
        public bool QueenIsNotLocked(int x, int y)
        {
            if (GameField[x + 1, y] < 0 && GameField[x + 1, y + 1] < 0 &&
                GameField[x, y + 1] < 0 && GameField[x - 1, y + 1] < 0 &&
                GameField[x - 1, y] < 0 && GameField[x - 1, y - 1] < 0 &&
                GameField[x, y - 1] < 0 && GameField[x + 1, y - 1] < 0)
                return false;
            else return true;
        }
    }
}
