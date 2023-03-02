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
        public Player Сompetitor { get; set; }
        public bool Pat = false;
        public int motionColor = 0;
        public Position posEnd;

        public Player(Color color, ref Field GameField)
        {
            this.Color = color;
            this.GameField = GameField;
            king = new FigureKing(ref GameField, color);
            queen = new FigureQueen(ref GameField, color);
            if (color == Color.White)
            {
                posEnd = new Position(7, 4);
            }
            else
            {
                posEnd = new Position(0, 4);
            }


        }

        public int Wave(int startX, int startY, int finishX, int finishY, FigureKing figure, int motion, int strategy)
        {
            int result, fx, fy, x, y;
            while (true)
            {
                Field cMap = Function.CreateWave(startX, startY, finishX, finishY, false, GameField);
                result = cMap[finishX, finishY];

                x = finishX;
                y = finishY;
                cMap.Draw();
                (fx, fy) = Function.Search(x, y, result, ref cMap, false);

                if (fx != -100 && checkXodKing(fx, fy, motion))
                {
                    figure.MoveBlock(fx, fy);
                    break;
                }
                else
                {
                    if (fx == -100 || (fx, fy) == (0, 4) || (fx, fy) == (7, 4))
                    {
                        List<Position> list = new List<Position>();
                        list.Add(new Position(0, 1));
                        list.Add(new Position(0, -1));
                        list.Add(new Position(1, 0));
                        list.Add(new Position(1, 1));
                        list.Add(new Position(1, -1));
                        list.Add(new Position(-1, 0));
                        list.Add(new Position(-1, 1));
                        list.Add(new Position(-1, -1));
                        while (list.Any())
                        {
                            Position pos = king.RandomXodKing(list);
                            if (GameField.IsInside(king.offset.Row + pos.Row, king.offset.Column + pos.Column) &&
                                checkXodKing(king.offset.Row + pos.Row, king.offset.Column + pos.Column, motion))
                            {
                                figure.MoveBlock(king.offset.Row + pos.Row, king.offset.Column + pos.Column);
                                fx = pos.Row;
                                break;
                            }
                            else
                                list.Remove(pos);
                        }
                        if (list.Count == 0)
                            fx = -100;
                        break;
                    }
                    GameField[fx, fy] = -7;
                    //queen.ObstaclesQueenCheck();
                }

                cMap.Draw();
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (GameField[i, j] == -7)
                    {
                        GameField[i, j] = 0;
                    }
                }
            }
            return fx;
        }
        public bool checkXodKing(int x, int y, int motion)
        {
            if (!Сompetitor.queen.CheckQueenAttack(Сompetitor.queen.offset.Row, Сompetitor.queen.offset.Column, x, y) ||
               king.AdjacentPosition(x, y, Сompetitor.king.offset.Row, Сompetitor.king.offset.Column) ||
               king.LeaveSquare(x, y, motion) ||
               !GameField.IsEmptyWave(x, y))
                return false;
            else
                return true;
        }

        public void StrategySimple(int motion)
        {
            Console.WriteLine("Ходит {0} ", Color);
            if (motion % 6 == 0)
            {
                bool check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column);
                if (check == false)
                {

                    if (king.offset.Row == posEnd.Row && king.offset.Column == posEnd.Column)
                        Console.WriteLine("Finish. Win is {0} ", Color);
                    else
                    {
                        if (-100 == Wave(king.offset.Row, king.offset.Column, posEnd.Row, posEnd.Column, king, motion, 1))
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                        }
                    }
                }
            }
            else
            {
                if (king.offset.Row == posEnd.Row && king.offset.Column == posEnd.Column)
                    Console.WriteLine("Finish. {0} is win", Color);
                else
                {
                    if (-100 == Wave(king.offset.Row, king.offset.Column, posEnd.Row, posEnd.Column, king, motion, 1))
                    {
                        bool check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column);
                        if (check == false)
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                        }
                    }
                }
            }
        }


        public void StrategyCapture(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);
            if (motion % 2 == 0)
            {
                if (king.offset.Row == posEnd.Row && king.offset.Column == posEnd.Column)
                {
                    Console.WriteLine("Finish. Win is {0}", Color);
                    return;
                }
                else
                {
                    if (-100 == Wave(king.offset.Row, king.offset.Column, posEnd.Row, posEnd.Column, king, motion, 2))
                    {
                        bool check = queen.ObstacleMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Color);
                        if (check)
                        {
                            motionColor = 0;
                            return;
                        }
                        check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column);
                        if (check == false)
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                            return;
                        };
                        motionColor = 0;
                    }
                }
            }
            else
            {

                bool check = queen.ObstacleMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Color);

                if (check == true)
                {
                    motionColor = 0;
                    return;
                }
                if (motionColor >= 6)
                {
                    check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column);
                    if (check == false)
                    {
                        if (-100 == Wave(king.offset.Row, king.offset.Column, posEnd.Row, posEnd.Column, king, motion, 1))
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                            return;
                        }
                    }
                }

                if (king.offset.Row == posEnd.Row && king.offset.Column == posEnd.Column)
                {
                    Console.WriteLine("Finish. Win is {0}", Color);
                    return;
                }
                else
                {
                    if (-100 == Wave(king.offset.Row, king.offset.Column, posEnd.Row, posEnd.Column, king, motion, 2))
                    {
                        check = queen.ObstacleMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Color);
                        if (check)
                        {
                            motionColor = 0;
                            return;
                        }
                        check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column);
                        if (check == false)
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                            return;
                        };
                        motionColor = 0;
                    }
                }

            }
        }
    }
}