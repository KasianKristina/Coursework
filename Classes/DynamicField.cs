using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                // метод сброса вызывается для установки 
                // правильного начального положения и поворота
                currentDetail.Reset();
            }
        }

        public Field GameField;
        public bool GameOver { get; set; }
        public int Message { get; private set; }
        public FigureQueen startQueen { get; set; }
        public FigureKing startKing { get; set; }
        public FigureKingBlack startKingBlack { get; set; }
        public DynamicField()
        {
            GameField = new Field(8, 8);
            startQueen = new FigureQueen(ref GameField);
            startKing = new FigureKing(ref GameField);
            startKingBlack = new FigureKingBlack(ref GameField);
            GameField[0, 3] = startQueen.Id;
            GameField[0, 4] = startKing.Id;
            GameField[7, 4] = startKingBlack.Id;
            //startQueen.MoveDetailRight();
            //startQueen.MoveDetailLeft();
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
            GameField = copy;
        }

        public bool TwoWave(Field copy)
        {
           
            Field cMap = CreateWave(0, 4, 7, 4, true, copy);
            //cMap.Draw();
            int result = cMap[7, 4];
            if (result == -6)
                return false;
            Search(7, 4, result, ref cMap, true);
            cMap = CreateWave(0, 4, 7, 4, true, cMap);
            //cMap.Draw();
            result = cMap[7, 4];
            if (result == -6)
                return false;
            return true;
        }

        public Field CreateWave(int startX, int startY, int finishX, int finishY, bool wall, Field field)
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
        public void Wave(int startX, int startY, int finishX, int finishY)
        {
            int result, fx, fy, x, y;
            Field cMap = CreateWave(startX, startY, finishX, finishY, false, GameField);

            result = cMap[finishX, finishY];
            x = finishX;
            y = finishY;
            cMap.Draw();
            (fx, fy) = Search(x, y, result, ref cMap, false);
            //startKing.MoveBlock(fx, fy);
            startKing.MoveDetailDown();
            cMap.Draw();
        }

        public (int fx, int fy) Search(int x, int y, int result, ref Field cMap, bool wall)
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

        public void FindWave(int startX, int startY, int targetX, int targetY)
        {
            bool add = true;
            int MapHeight = 8;
            int MapWidht = 8;
            int[,] cMap = new int[MapHeight, MapWidht];
            int x, y, step = 0;
            for (y = 0; y < MapHeight; y++)
                for (x = 0; x < MapWidht; x++)
                {
                    if (GameField[y, x] != 0)
                        cMap[y, x] = -5;//индикатор стены
                    else
                        cMap[y, x] = 1;//индикатор еще не ступали сюда
                }
            cMap[targetY, targetX] = 0;//Начинаем с финиша
            while (add == true)
            {
                add = false;
                for (y = 0; y < MapWidht; y++)
                    for (x = 0; x < MapHeight; x++)
                    {
                        if (cMap[x, y] == step)
                        {
                            //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                            if (y - 1 >= 0 && cMap[x, y - 1] != -5 && cMap[x, y - 1] == 1)
                                cMap[x, y - 1] = step + 1;
                            if (x - 1 >= 0 && cMap[x - 1, y] != -5 && cMap[x - 1, y] == 1)
                                cMap[x - 1, y] = step + 1;
                            if (y + 1 < MapWidht && cMap[x, y + 1] != -5 && cMap[x, y + 1] == 1)
                                cMap[x, y + 1] = step + 1;
                            if (x + 1 < MapHeight && cMap[x + 1, y] != -5 && cMap[x + 1, y] == 1)
                                cMap[x + 1, y] = step + 1;
                        }
                    }
                step++;

                add = true;
                if (cMap[startY, startX] != -1)//решение найдено
                    add = true;
                if (step > MapWidht * MapHeight)//решение не найдено
                    add = false;
            }
        }


        private bool IsGameOver()
        {
            return !(GameField.IsRowEmpty(0) && GameField.IsRowEmpty(1) && GameField.IsRowEmpty(2));
        }

        public bool dataInit2()
        {
            int[][] ar = new int[8][]; //кол-во строк

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ar[i][j] != GameField[i, j])
                    {
                        return false;
                    };
                }
            }
            return true;
        }
    }
}
