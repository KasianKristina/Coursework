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

        public bool RandomMove(int kingRow, int kingCol)
        {
            int position;
            List<Position> list = getAllPosition(offset.Row, offset.Column, kingRow, kingCol);
            Random random = new Random();
            while (true)
            {
                if (list.Count == 0)
                    return false;
                else
                {
                    position = random.Next(list.Count);
                    if (CheckQueenAttack(list[position].Row, list[position].Column, kingRow, kingCol))
                        break;
                }
            }
            this.MoveBlock(list[position].Row, list[position].Column);
            return true;
        }

        public bool ObstacleMove(int kingRow, int kingCol, Color color)
        {
            List<Position> listObstacles = getObstaclesPosition(kingRow, kingCol, color);
            List<Position> listAll = getAllPosition(offset.Row, offset.Column, kingRow, kingCol);
            for (int i = 0; i < listObstacles.Count; i++)
            {
                for (int j = 0; j < listAll.Count; j++)
                {
                    if (listAll[j].Row == listObstacles[i].Row &&
                        listAll[j].Column == listObstacles[i].Column)
                    {
                        this.MoveBlock(listObstacles[i].Row, listObstacles[i].Column);
                        return true;
                    }
                }
            }
            return  false;
        }

        // возможные позиции королевы
        public List<Position> getAllPosition(int x, int y, int kingRow, int kingCol)
        {
            List<Position> list = new List<Position>();
            // иду вниз
            for (int i = x + 1; i < 8; i++)
            {
                if (CheckQueenAttack(i, y, kingRow, kingCol))
                {
                    if (GameField[i, y] == 0)
                        list.Add(new Position(i, y));
                    else break;
                }
            }
            // иду вверх
            for (int i = x - 1; i > 0; i--)
            {
                if (CheckQueenAttack(i, y, kingRow, kingCol))
                {
                    if (GameField[i, y] == 0)
                        list.Add(new Position(i, y));
                    else break;
                }
            }
            // иду в правый нижний угол
            int rowStep = 1;
            int columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                {
                    if (GameField[x + i * rowStep, y + i * columnStep] == 0)
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                    else break;
                }
            }
            // иду в левый нижний угол
            rowStep = 1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) &&  CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                {
                    if (GameField[x + i * rowStep, y + i * columnStep] == 0)
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                    else break;
                }
            }
            // иду в правый верхний угол
            rowStep = -1;
            columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                {
                    if (GameField[x + i * rowStep, y + i * columnStep] == 0)
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                    else break;
                }
            }
            // иду в левый верхний угол
            rowStep = -1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                {
                    if (GameField[x + i * rowStep, y + i * columnStep] == 0)
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                    else break;
                }
            }
            return list;
        }

        // все позиции для блокировки короля соперника
        public List<Position> getObstaclesPosition(int kingRow, int kingCol, Color color)
        {
            List<Position> list = new List<Position>();
            int k;
            if (color == Color.Black)
                k = -1;
            else k = 1;
            // вправо
            for (int i = kingCol + 1; i < 8; i++)
            {
                if (GameField.IsWall(kingRow - k, i))
                    break;
                if (GameField.IsInside(kingRow - k, i) && GameField[kingRow - k, i] == 0)
                    list.Add(new Position(kingRow - k, i));
            }
            // влево
            for (int i = kingCol - 1; i >= 0; i--)
            {
                if (GameField.IsWall(kingRow - k, i))
                    break;
                if (GameField.IsInside(kingRow - k, i) && GameField[kingRow - k, i] == 0)
                    list.Add(new Position(kingRow - k, i));
            }
            return list;
        }

        // true - не бьет, false - бьет
        public bool CheckQueenAttack(int queenRow, int queenCol, int kingRow, int kingCol)
        {
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
