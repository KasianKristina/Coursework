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
            }
        }

        public Field GameField;
        public Player player1;
        public Player player2;
        int motion = 1;

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
            while (!IsGameOver())
            {
                player1.StrategySimple(motion);
                if (IsGameOver())
                    break;
                player2.StrategySimple(motion);
                motion++;
                Draw();
            }
            Console.WriteLine("Конец игры");
        }

        public void Strategy2()
        {
            while (!IsGameOver())
            {
                player1.StrategyCapture(motion);
                if (IsGameOver())
                    break;
                Draw();
                player2.StrategyCapture(motion);
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
            Field cMap = Function.CreateWave(0, 4, 7, 4, true, copy);
            Console.WriteLine("Первый непересекающийся путь");
            int result = cMap[7, 4];
            cMap.Draw();
            if (result == -6)
                return false;
            
            Function.Search(7, 4, result, ref cMap, true);
            cMap.Draw();
            cMap = Function.CreateWave(0, 4, 7, 4, true, cMap);

            Console.WriteLine("Второй непересекающийся путь");
            cMap.Draw();
            result = cMap[7, 4];
            if (result == -6)
                return false;
            return true;
        }
    }
}
