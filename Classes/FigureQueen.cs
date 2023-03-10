using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
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
                Id = -2;
                GameField[0, 3] = Id;
                offset = new Position(0, 3);
                endingPosition = new Position(7, 3);
            }
            else
            {
                Id = -4;
                GameField[7, 3] = Id;
                offset = new Position(7, 3);
                endingPosition = new Position(0, 3);
            }
            Color = color;
        }

        public bool RandomMove(int kingRow, int kingCol, int motion, Dictionary<int, (int, Position)> history, int motionColor)
        {
            int position;
            List<Position> list = getAllPosition(offset.Row, offset.Column, kingRow, kingCol, motionColor);
            Random random = new Random();

            if (list.Count == 0)
                return false;
            position = random.Next(list.Count);

            MoveBlock(list[position].Row, list[position].Column);
            history.Add(motion, (Id, new Position(list[position].Row, list[position].Column)));
            return true;
        }

        public bool PositionsEquel(int firstRow, int firstCol, int secondRow, int secondCol)
        {
            if (firstRow == secondRow && firstCol == secondCol)
                return true;
            return false;
        }

        public bool getPosFerz(int kingRow, int kingCol, Color color)
        {
            int k;
            int pos;
            if (color == Color.Black)
            {
                k = -1;
                pos = -4;
            }
            else
            {
                k = 1;
                pos = -2;
            }
            // вправо
            for (int i = kingCol + 1; i < 8; i++)
            {
                if (GameField.IsInside(kingRow - k, i))
                    if (GameField[kingRow - k, i] == pos)
                        return true;
            }
            // влево
            for (int i = kingCol - 1; i >= 0; i--)
            {
                if (GameField.IsInside(kingRow - k, i))
                    if (GameField[kingRow - k, i] == pos)
                        return true;
            }

            return false;
        }

        public bool ObstacleMove(int kingRow, int kingCol, Color color, int motionColor, Dictionary<int, (int, Position)> history, int motion)
        {
            List<Position> listObstacles = getObstaclesPosition(kingRow, kingCol, color);
            List<Position> listAll = getAllPosition(offset.Row, offset.Column, kingRow, kingCol, motionColor);
            for (int i = 0; i < listObstacles.Count; i++)
            {
                for (int j = 0; j < listAll.Count; j++)
                {
                    if (PositionsEquel(listAll[j].Row, listAll[j].Column, listObstacles[i].Row, listObstacles[i].Column) &&
                        history.Count > 1 &&
                        listObstacles[i].Row != history[motion - 1].Item2.Row &&
                        !getPosFerz(kingRow, kingCol, color))
                    {
                        MoveBlock(listObstacles[i].Row, listObstacles[i].Column);
                        history.Add(motion, (Id, new Position(listObstacles[i].Row, listObstacles[i].Column)));
                        return true;
                    }
                }
            }
            return false;
        }

        // 5 2 вынести в класс
        public bool ObstacleOrNearbyMove(int kingRow, int kingCol, Color color, int motionColor, Dictionary<int, (int, Position)> history, int motion)
        {
            int row = 2;
            if (color == Color.Black)
                row = 5;
            if ((color == Color.Black && kingRow >= row) || (color == Color.White && kingRow <= row))
            {
                bool check = ObstacleMove(kingRow, kingCol, color, motionColor, history, motion);
                    return check;
            }
            else if (motionColor >= 6 && ((color == Color.Black && kingRow < row) || (color == Color.White && kingRow > row)))
            {
                bool check = NearbyMove(kingRow, kingCol, motion, history);
                return check;
            }
            else
                return false;
        }

        public bool HorizontalMove(int kingRow, int kingCol, Dictionary<int, (int, Position)> history, int motion)
        {
            int position;
            List<Position> list = getHorizontalPositions(offset.Row, offset.Column, kingRow, kingCol);
            Random random = new Random();

            if (list.Count == 0)
                return false;
            position = random.Next(list.Count);

            MoveBlock(list[position].Row, list[position].Column);
            history.Add(motion, (Id, new Position(list[position].Row, list[position].Column)));
            return true;
        }

        public List<Position> getHorizontalPositions(int x, int y, int kingRow, int kingCol)
        {
            List<Position> list = new List<Position>();

            // иду вправо
            for (int i = y + 1; i < 8; i++)
            {
                if (GameField[x, i] == 0)
                {
                    if (CheckQueenAttack(x, i, kingRow, kingCol))
                        list.Add(new Position(x, i));
                }
                else break;
            }
            // иду влево
            for (int i = y - 1; i >= 0; i--)
            {
                if (GameField[x, i] == 0)
                {
                    if (CheckQueenAttack(x, i, kingRow, kingCol))
                        list.Add(new Position(x, i));
                }
                else break;
            }

            return list;
        }
        // TODO избавиться от дублирования
        // возможные позиции королевы
        public List<Position> getAllPosition(int x, int y, int kingRow, int kingCol, int motion)
        {
            List<Position> list = new List<Position>();
            if (motion < 6)
            {
                // иду вправо
                for (int i = y + 1; i < 8; i++)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(x, i, kingRow, kingCol))
                            list.Add(new Position(x, i));
                    }
                    else break;
                }
                // иду влево
                for (int i = y - 1; i >= 0; i--)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(x, i, kingRow, kingCol))
                            list.Add(new Position(x, i));
                    }
                    else break;
                }
            }
            // иду вниз
            for (int i = x + 1; i < 8; i++)
            {
                if (GameField[i, y] == 0)
                {
                    if (CheckQueenAttack(i, y, kingRow, kingCol))
                        list.Add(new Position(i, y));
                }
                else break;
            }
            // иду вверх
            for (int i = x - 1; i >= 0; i--)
            {
                if (GameField[i, y] == 0)
                {
                    if (CheckQueenAttack(i, y, kingRow, kingCol))
                        list.Add(new Position(i, y));
                }
                else break;
            }
            // иду в правый нижний угол
            int rowStep = 1;
            int columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в левый нижний угол
            rowStep = 1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в правый верхний угол
            rowStep = -1;
            columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в левый верхний угол
            rowStep = -1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, kingRow, kingCol))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            return list;
        }

        public bool NearbyMove(int kingRow, int kingCol, int motion, Dictionary<int, (int, Position)> history)
        {
            List<Position> list = new List<Position>() {
                            new Position(1, 0),
                            new Position(1, 1),
                            new Position(1, -1),
                            new Position(-1, 0),
                            new Position(-1, 1),
                            new Position(-1, -1),
                        };
            Random random = new Random();
            while (list.Any())
            {
                int position = random.Next(list.Count);
                if (GameField.IsEmpty(offset.Row + list[position].Row, offset.Column + list[position].Column)
                    && CheckQueenAttack(offset.Row + list[position].Row, offset.Column + list[position].Column, kingRow, kingCol))
                {
                    MoveBlock(offset.Row + list[position].Row, offset.Column + list[position].Column);
                    history.Add(motion, (Id, new Position(offset.Row + list[position].Row, offset.Column + list[position].Column)));
                    return true;
                }
                else list.RemoveAt(position);
            }
            return false;
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
    }
}
