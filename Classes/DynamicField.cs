using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Classes
{
    public class DynamicField
    {
        private Figure currentDetail;

        public Figure CurrentDetail
        {
            get
            {
                return currentDetail;
            }
            set
            {
                currentDetail = value;
            }
        }

        public Field GameField;
        public Player player1;
        public Player player2;


        public DynamicField()
        {
            GameField = new Field(8, 8);
            player1 = new Player(Color.White, ref GameField);
            player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
        }

        public void Strategy1()
        {
            int motion = 1;
            while (!IsGameOver())
            {
                player1.StrategySimple(motion);
                if (IsGameOver())
                    break;
                Draw();
                player2.StrategySimple(motion);
                motion++;
                Draw();
            }
            Console.WriteLine("Конец игры");
        }

        public void Strategy2()
        {
            int motion = 1;
            while (!IsGameOver())
            {
                player1.Str(motion);
                if (IsGameOver())
                    break;
                Draw();
                player2.Str(motion);
                motion++;
                Draw();
            }
            Console.WriteLine("Конец игры");
        }

        public void Strategy3()
        {
            int motion = 1;
            while (!IsGameOver())
            {
                player1.StrategySecurity(motion);
                if (IsGameOver())
                    break;
                Draw();
                player2.StrategySecurity(motion);
                motion++;
                Draw();
            }
            Console.WriteLine("Конец игры");
        }

        public bool IsGameOver()
        {
            if ((GameField[0, 4] == -3) || (GameField[7, 4] == -1))
            {
                if (GameField[0, 4] == -3)
                    Console.WriteLine("Победили черные фигуры");
                else Console.WriteLine("Победили белые фигуры");
                return true;
            }
            if (player1.Pat || player2.Pat)
            {
                Console.WriteLine("Ничья");
                return true;
            }
            return false;
        }

        public void Draw()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(Math.Abs(GameField[i, j]));
                }
                Console.WriteLine();
            }
            Console.WriteLine("********");
        }

        public void Walls()
        {
            Field copy;
            Random random = new Random();
            while (true)
            {
                copy = GameField.Copy();
                int i = 1;
                copy.Draw();
                int number = random.Next(1, 31);
                Console.WriteLine(number);
                while (i <= number)
                {
                    int x = random.Next(0, 8);
                    int y = random.Next(0, 8);
                    if (copy[x, y] == 0)
                    {
                        copy[x, y] = -5;
                        i++;
                    }
                }
                copy.Draw();
                if (TwoWave(copy))
                    break;
            }
            GameField.Clone(copy);
        }

        // поиск двух непересекающихся путей
        // первый найденный путь отмечаем -5 (стены)
        public bool TwoWave(Field copy)
        {
            Field cMap = CreateWave(0, 4, 7, 4, true, copy);
            Console.WriteLine("Первый непересекающийся путь");
            int result = cMap[7, 4];
            cMap.Draw();
            if (result == -6)
                return false;

            Search(7, 4, result, ref cMap, true);
            cMap.Draw();
            cMap = CreateWave(0, 4, 7, 4, true, cMap);

            Console.WriteLine("Второй непересекающийся путь");
            cMap.Draw();
            result = cMap[7, 4];
            if (result == -6)
                return false;
            return true;
        }

        //public static bool SearchFirst(int result, int x, int y, ref Field cMap, bool wall)
        //{
        //    if (cMap.IsEmptyWave(x, y) && cMap[x, y] == result - 1)
        //    {
        //        result = cMap[x, y - 1];
        //        y = y - 1;
        //        if (wall)
        //            cMap[x, y] = -5;
        //    }
        //    List<Position> list = new List<Position>() {
        //                    new Position(0, 1),
        //                    new Position(0, -1),
        //                    new Position(1, 0),
        //                    new Position(1, 1),
        //                    new Position(1, -1),
        //                    new Position(-1, 0),
        //                    new Position(-1, 1),
        //                    new Position(-1, -1),
        //                };
        //    foreach (Position pos in list)
        //    {
        //        if (GameField.IsInside(king.offset.Row + pos.Row, king.offset.Column + pos.Column) &&
        //                        checkXodKing(king.offset.Row + pos.Row, king.offset.Column + pos.Column, motion))
        //    }
        //    return true;
        //}

        // TODO убрать дублирование
        // поиск позиции, на которую нужно идти королю
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
                else if (cMap.IsEmptyWave(x + 1, y + 1) && cMap[x + 1, y + 1] == result - 1)
                {
                    result = cMap[x + 1, y + 1];
                    x = x + 1;
                    y = y + 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x + 1, y - 1) && cMap[x + 1, y - 1] == result - 1)
                {
                    result = cMap[x + 1, y - 1];
                    x = x + 1;
                    y = y - 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x - 1, y - 1) && cMap[x - 1, y - 1] == result - 1)
                {
                    result = cMap[x - 1, y - 1];
                    x = x - 1;
                    y = y - 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x - 1, y + 1) && cMap[x - 1, y + 1] == result - 1)
                {
                    result = cMap[x - 1, y + 1];
                    x = x - 1;
                    y = y + 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else
                {
                    Console.WriteLine("оптимальный путь не найден");
                    return (-100, -100);
                }
            }
            return (x, y);
        }

        // TODO убрать дублирование
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
                        cMap[x, y] = -5; // индикатор стены
                    else
                        cMap[x, y] = -6; // индикатор: еще не были здесь
                }
            }

            cMap[finishX, finishY] = -6;

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
                            if (y - 1 >= 0 && cMap[x, y - 1] == -6)
                                cMap[x, y - 1] = step + 1;
                            if (x - 1 >= 0 && cMap[x - 1, y] == -6)
                                cMap[x - 1, y] = step + 1;
                            if (y + 1 < MapX && cMap[x, y + 1] == -6)
                                cMap[x, y + 1] = step + 1;
                            if (x + 1 < MapY && cMap[x + 1, y] == -6)
                                cMap[x + 1, y] = step + 1;
                            if (x + 1 < MapY && y + 1 < MapY && cMap[x + 1, y + 1] == -6)
                                cMap[x + 1, y + 1] = step + 1;
                            if (x + 1 < MapY && y - 1 >= 0 && cMap[x + 1, y - 1] == -6)
                                cMap[x + 1, y - 1] = step + 1;
                            if (x - 1 >= 0 && y - 1 >= 0 && cMap[x - 1, y - 1] == -6)
                                cMap[x - 1, y - 1] = step + 1;
                            if (x - 1 >= 0 && y + 1 < MapY && cMap[x - 1, y + 1] == -6)
                                cMap[x - 1, y + 1] = step + 1;
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
