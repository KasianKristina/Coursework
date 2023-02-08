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

            //Function.PortimGameField(ref GameField, Сompetitor);
            //GameField.Draw();
            while (true)
            {
                Field cMap = Function.CreateWave(startX, startY, finishX, finishY, false, GameField);
                result = cMap[finishX, finishY];

                x = finishX;
                y = finishY;
                cMap.Draw();
                (fx, fy) = Function.Search(x, y, result, ref cMap, false);

                if (fx == -100)
                    break;

                if (checkXodKing(fx, fy, motion))
                {
                    figure.MoveBlock(fx, fy);

                    break;
                }
                else
                {
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
                queen.MoveDetailLeft();
        }
        public bool checkXodKing(int x, int y, int motion)
        {
            // TODO добавить другие условия
            if (CheckQueenAttack(Сompetitor.queen.offset.Row, Сompetitor.queen.offset.Column,x,y) &&
               king.AdjacentPosition(x, y,Сompetitor.king.offset.Row, Сompetitor.king.offset.Column))
                return true;  
            else
                 return false;   
        }
        // проверка: ферзь не может делать шах королю
        public bool CheckKingAttack(int KingComRow, int KingComCol, int kingRow, int kingCol)
        {
            
            
            return true;

        }

        // true - если бьет ферзь и false - не бьет
        public bool CheckQueenAttack(int queenRow, int queenCol, int kingRow, int kingCol)
        {
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
                            GameField[queenRow + i * rowStep, queenCol + i * columnStep] >= -5 )
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

        public void StrategySimple(int motion)
        {
            Console.WriteLine("Ходит {0} ", Color);
            if (motion % 6 == 0)
            {
                queen.MoveDetailLeft();
            }
            else
            {
                if (Color == Color.White)
                {
                    if (king.offset.Row == 7 && king.offset.Column == 4)
                        Console.WriteLine("Finish. White is win");
                    else 
                        Wave(king.offset.Row, king.offset.Column, 7, 4, king,motion);
                }
                else
                {
                    if (king.offset.Row == 0 && king.offset.Column == 4)
                        Console.WriteLine("Finish. Black is win");
                    else 
                        Wave(king.offset.Row, king.offset.Column, 0, 4, king,motion);
                }
            }
        }
    }
}
