using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    class Function
    {

        public static void PortimGameField(ref Field GameField, Player competitor)
        {
            BlocksWay(competitor.queen.offset.Row, competitor.queen.offset.Column, ref GameField);
            GameField.Draw();
            BlocksByKing(competitor.king.offset.Row, competitor.king.offset.Column, ref GameField);
        }
        public static bool NoWall(int x, int y, ref Field GameField)
        {
            if (GameField[x, y] < 0)
                return false;
            else return true;
        }

        // преграды ферзем/королем

        public static void BlocksByKing(int x, int y, ref Field GameField)
        {
            // Рисует битые поля короля

            if (GameField.IsEmptyWave(x, y - 1))
            {
                GameField[x, y - 1] = -7;
            }
            if (GameField.IsEmptyWave(x, y + 1) )
            {
                GameField[x, y + 1] = -7;
            }
            if (GameField.IsEmptyWave(x + 1, y) )
            {
                GameField[x + 1, y] = -7;
            }
            if (GameField.IsEmptyWave(x - 1, y))
            {
                GameField[x - 1, y] = -7;
            }

            if (GameField.IsEmptyWave(x - 1, y - 1))
            {
                GameField[x - 1, y - 1] = -7;
            }
            if (GameField.IsEmptyWave(x - 1, y + 1))
            {
                GameField[x - 1, y + 1] = -7;
            }
            if (GameField.IsEmptyWave(x + 1, y - 1))
            {
                GameField[x + 1, y - 1] = -7;
            }
            if (GameField.IsEmptyWave(x + 1, y + 1))
            {
                GameField[x + 1, y + 1] = -7;
            }
        }
        public static void BlocksWay(int x, int y, ref Field GameField)
        {
            // Рисует битые поля королевы
            int mark = -7;
            
           
            for (int i = 0; i < 8; i++)
            {
                if (NoWall(i, y, ref GameField))
                    GameField[i, y] = mark;
                else break;
            }
            for (int i = 0; i < 8; i++)
            {
                if (NoWall(x, i, ref GameField))
                    GameField[x, i] = mark;
                else break;
            }
            DiagonalMarks(x, y, mark, ref GameField);
        }

        // пометки диагоналей
        public static void DiagonalMarks(int x, int y, int mark1, ref Field GameField)
        {
            int i = x, j = y;
            if (GameField.IsEmpty(i + 1, j + 1))
            {
                i = x + 1;
                j = y + 1;
            }
            else
            {
                i = x;
                j = y;
            }
            while (i < 7 && j < 7)
            {
                if (NoWall(i, j, ref GameField))
                {
                    GameField[i, j] = mark1;
                    i++;
                    j++;
                }
                else break;
            }

            if (GameField.IsEmpty(i - 1, j - 1))
            {
                i = x - 1;
                j = y - 1;
            }
            else
            {
                i = x;
                j = y;
            }
            while (i > 0 && j > 0)
            {
                if (NoWall(i, j, ref GameField))
                {
                    GameField[i, j] = mark1;
                    i--;
                    j--;
                }
                else break;
            }

            if (GameField.IsEmpty(i + 1, j - 1))
            {
                i = x + 1;
                j = y - 1;
            }
            else
            {
                i = x;
                j = y;
            }
            while (j > 0 && i < 7)
            {
                if (NoWall(i, j, ref GameField))
                {
                    GameField[i, j] = mark1;
                    i++;
                    j--;
                }
                else break;
            }

            if (GameField.IsEmpty(i - 1, j + 1))
            {
                i = x - 1;
                j = y + 1;
            }
            else
            {
                i = x;
                j = y;
            }
            while (i > 0 && j < 7)
            {
                if (NoWall(i, j, ref GameField))
                {
                    GameField[i, j] = mark1;
                    i--;
                    j++;
                }
                else break;
            }
        }

        public static (int fx, int fy) Search(int x, int y, int result, ref Field cMap, bool wall)
        {
            while (result != 1)
            {
                if (cMap.IsEmptyWave(x, y - 1) && cMap[x, y - 1] == result - 1)
                {
                    result = cMap[x, y - 1];
                    y = y - 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x, y + 1) && cMap[x, y + 1] == result - 1)
                {
                    result = cMap[x, y + 1];
                    y = y + 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x + 1, y) && cMap[x + 1, y] == result - 1)
                {
                    result = cMap[x + 1, y];
                    x = x + 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x - 1, y) && cMap[x - 1, y] == result - 1)
                {
                    result = cMap[x - 1, y];
                    x = x - 1;
                    if (wall)
                        cMap[x, y] = -5;

                }
                else
                {
                    Console.WriteLine("путь не найден");
                    return (-100, -100);
                }
            }
            return (x, y);
        }
        // пат
/*        public bool Stalemate(int x_king, int y_king, int x_queen, int y_queen,
            int x_king_opponent, int y_king_opponent, Player player,
            int x_queen_next, int y_queen_next, bool flag)
        {
            if (OpportunityToMakeMoveKing(x_king, y_king, x_king_opponent, y_king_opponent, player, motion) ||
                OpportunityToMakeMoveQueen(x_queen, y_queen, x_queen_next, y_queen_next, motion, flag))
                return false;
            else return true;
        }
*/

        public static Field CreateWave(int startX, int startY, int finishX, int finishY, bool wall, Field field)
        {
            bool add = true;
            int MapX = 8;
            int MapY = 8;
            Field cMap = new Field(8, 8);
            int x, y, step = 0, count = 0;

            for (x = 0; x < MapX; x++)
            {
                for (y = 0; y < MapY; y++)
                {
                    if (field[x, y] != -6 && field[x, y] < 0)
                        cMap[x, y] = -5;//индикатор стены
                    else
                        cMap[x, y] = -6;//индикатор еще не ступали сюда

                }
            }
            cMap[7, 4] = -6;
            cMap[0, 4] = -6;
            cMap[startX, startY] = 0;
            while (add == true)
            {
                count++;
                for (x = 0; x < MapX; x++)
                {
                    for (y = 0; y < MapY; y++)
                    {
                        if (cMap[x, y] == step)
                        {
                            //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                            if (y - 1 >= 0 && cMap[x, y - 1] != -5 && cMap[x, y - 1] == -6)
                                cMap[x, y - 1] = step + 1;
                            if (x - 1 >= 0 && cMap[x - 1, y] != -5 && cMap[x - 1, y] == -6)
                                cMap[x - 1, y] = step + 1;
                            if (y + 1 < MapX && cMap[x, y + 1] != -5 && cMap[x, y + 1] == -6)
                                cMap[x, y + 1] = step + 1;
                            if (x + 1 < MapY && cMap[x + 1, y] != -5 && cMap[x + 1, y] == -6)
                                cMap[x + 1, y] = step + 1;
                        }

                        if (cMap[finishX, finishY] != -6 && cMap[finishX, finishY] != -5)//решение найдено
                        {
                            add = false;
                            break;
                        }
                    }
                }
                step++;
                if (count == 164)
                    add = false;
            }
            return cMap;
        }
    }
}
