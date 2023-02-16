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
            if (color != Color.Black && color != Color.White)
                throw new Exception("color black or white");
            this.GameField = GameField;
            if (color == Color.White)
            {
                this.Id = -2;
                GameField[0, 3] = this.Id;
                offset = new Position(0, 3);
            }
            else
            {
                this.Id = -4;
                GameField[7, 3] = this.Id;
                offset = new Position(7, 3);
            }
            this.Color = color;
        }

        public void RandomMove(int kingRow, int kingCol)
        {
            //if (GameField.IsEmpty(offset.Row - 1, offset.Column + 1) && !CheckQueenAttack(offset.Row - 1, offset.Column + 1, kingRow, kingCol))
            //{
            //    this.MoveBlock(offset.Row - 1, offset.Column + 1);
            //}
            //else 
            //if (GameField.IsEmpty(offset.Row, offset.Column + 1) && !CheckQueenAttack(offset.Row, offset.Column + 1, kingRow, kingCol))
            //{
            //    this.MoveBlock(offset.Row, offset.Column + 1);
            //}
            //else
            //if (GameField.IsEmpty(offset.Row + 1, offset.Column + 1) && !CheckQueenAttack(offset.Row + 1, offset.Column + 1, kingRow, kingCol))
            //{
            //    this.MoveBlock(offset.Row + 1, offset.Column + 1);
            //}
            Random random = new Random();
            int x, w, y;
            while (true)
            {
                x = random.Next(0, 8);
                if (x != offset.Row)
                    break;
            }
            w = Math.Abs(offset.Row - offset.Column);
            y = Math.Abs(w - x);
            this.MoveBlock(x, y);
        }

        // true - если бьет ферзь и false - не бьет
        public bool CheckQueenAttack(int queenRow, int queenCol, int kingRow, int kingCol)
        {
            // int queenRow = offset.Row;
            // int queenCol = offset.Column;
            // Row - x Col - y
            // Check if king is in the same row or column as queen
            if (queenRow == kingRow)
                if (queenCol > kingCol)
                {
                    for (int i = queenCol - 1; i > kingCol; i--)
                    {
                        if (GameField[queenRow, i] < 0 & GameField[queenRow, i] >= -5)
                            return true;
                    }
                    return false;
                }
                else
                {
                    for (int i = queenCol + 1; i < kingCol; i++)
                    {
                        if (GameField[queenRow, i] < 0 & GameField[queenRow, i] >= -5)
                            return true;
                    }
                    return false;
                }


            if (queenCol == kingCol)
                if (queenRow > kingRow)
                {
                    for (int i = queenRow - 1; i > kingRow; i--)
                    {
                        if (GameField[i, queenCol] < 0 & GameField[i, queenCol] >= -5)
                            return true;
                    }
                    return false;
                }
                else
                {
                    for (int i = queenRow + 1; i < kingRow; i++)
                    {
                        if (GameField[i, queenCol] < 0 & GameField[i, queenCol] >= -5)
                            return true;
                    }
                    return false;
                }
            // Check if king is in the same diagonal as queen
            int rowDiff = Math.Abs(queenRow - kingRow);
            int colDiff = Math.Abs(queenCol - kingCol);

            if (rowDiff == colDiff)
            {
                // Check for obstacles
                if (queenRow > kingRow)
                {
                    int rowStep = -1;
                    int columnStep = 1;
                    if (queenCol > kingCol)
                        columnStep = -1;
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (GameField[queenRow + i * rowStep, queenCol + i * columnStep] < 0 &
                            GameField[queenRow + i * rowStep, queenCol + i * columnStep] >= -5)
                            return true;
                    }
                }
                else
                {
                    int rowStep = 1;
                    int columnStep = 1;
                    if (queenCol > kingCol)
                        columnStep = -1;
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (GameField[queenRow + i * rowStep, queenCol + i * columnStep] < 0 &
                            GameField[queenRow + i * rowStep, queenCol + i * columnStep] >= -5)
                            return true;
                    }
                }
                return false;
            }
            return true;
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
                //GameOver = true;
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
