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
       
        public Player player1;
        public Player player2;
        int motion = 1;

        public DynamicField()
        {
            GameField = new Field(8, 8);
            player1 = new Player(Color.White, ref GameField);
            player2 = new Player(Color.Black, ref GameField);
        }

        public void Strategy1()
        {
            while (!IsGameOver())
            {
                player1.StrategySimple(motion);
                player2.StrategySimple(motion);
                motion++;
            }
        }

        public bool IsGameOver()
        {
            if ((GameField[0, 4] == -3) || (GameField[7, 4] == -1))
                return true;
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

        public bool TwoWave(Field copy)
        {
            Field cMap = Function.CreateWave(0, 4, 7, 4, true, copy);
            int result = cMap[7, 4];
            if (result == -6)
                return false;
            Function.Search(7, 4, result, ref cMap, true);
            cMap = Function.CreateWave(0, 4, 7, 4, true, cMap);
            result = cMap[7, 4];
            if (result == -6)
                return false;
            return true;
        }

        // проверка: клетка - не стена
        

        
        


        

        

        // пат
        public bool Stalemate(int x_king, int y_king, int x_queen, int y_queen,
            int x_king_opponent, int y_king_opponent, Player player,
            int x_queen_next, int y_queen_next, bool flag)
        {
            if (OpportunityToMakeMoveKing(x_king, y_king, x_king_opponent, y_king_opponent, player, motion) || 
                OpportunityToMakeMoveQueen(x_queen, y_queen, x_queen_next, y_queen_next, motion, flag))
                return false;
            else return true;
        }

        

       

        
    }
}
