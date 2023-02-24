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
        
        public Player(Color color, ref Field GameField)
        {
            this.Color = color;
            this.GameField = GameField;
            king = new FigureKing(ref GameField, color);
            queen = new FigureQueen(ref GameField, color);
          
        }

        public void Wave(int startX, int startY, int finishX, int finishY, FigureKing figure, int motion)
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
                        List<Position> list = new List<Position> { 
                            new Position(0, 1), 
                            new Position(0, -1), 
                            new Position(1, 0),
                            new Position(1, 1), 
                            new Position(1, -1), 
                            new Position(-1, 0), 
                            new Position(-1, 1), 
                            new Position(-1, 1)
                        };
                        while(list.Count != 0)
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

            if (fx == -100)
            {
                bool check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column);
                if (check == false)
                {
                    Console.WriteLine("пат");
                    Pat = true;
                }
            }
        }
        public bool checkXodKing(int x, int y, int motion)
        {
            // TODO добавить другие условия
            if (!Сompetitor.queen.CheckQueenAttack(Сompetitor.queen.offset.Row, Сompetitor.queen.offset.Column, x, y) ||
               king.AdjacentPosition(x, y,Сompetitor.king.offset.Row, Сompetitor.king.offset.Column))
               return false;
            else
               return true;   
        }
        // проверка: ферзь не может делать шах королю
        public bool CheckKingAttack(int KingComRow, int KingComCol, int kingRow, int kingCol)
        {
            
            
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
                    if (Color == Color.White)
                    {
                        if (king.offset.Row == 7 && king.offset.Column == 4)
                            Console.WriteLine("Finish. White is win");
                        else
                            Wave(king.offset.Row, king.offset.Column, 7, 4, king, motion);
                    }
                    else
                    {
                        if (king.offset.Row == 0 && king.offset.Column == 4)
                            Console.WriteLine("Finish. Black is win");
                        else
                            Wave(king.offset.Row, king.offset.Column, 0, 4, king, motion);
                    }
                }
            }
            else
            {
                if (Color == Color.White)
                {
                    if (king.offset.Row == 7 && king.offset.Column == 4)
                        Console.WriteLine("Finish. White is win");
                    else 
                        Wave(king.offset.Row, king.offset.Column, 7, 4, king, motion);
                }
                else
                {
                    if (king.offset.Row == 0 && king.offset.Column == 4)
                        Console.WriteLine("Finish. Black is win");
                    else 
                        Wave(king.offset.Row, king.offset.Column, 0, 4, king, motion);
                }
            }
        }
    }
}
