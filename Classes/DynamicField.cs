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

        // проверка: король не может переместиться на поле, которое бьёт ферзь соперника
        // (xk, yk) - координаты клетки, на которую собирается вступить король
        // (xf, yf) - координаты клетки, на которой находится ферзь соперника
        public bool CheckQueenBlocks(int xk, int yk, int xf, int yf)
        {
            
            return false;
        }

        // ход ферзя
        public void QueenMove(int x, int y)
        {

        }

        // ход короля
        public void KingMove(int x, int y)
        {

        }

        // проверка: клетка - не стена
        public bool NoWall(int x, int y)
        {
            if (GameField[x, y] < 0)
                return false;
            else return true;
        }

        // преграды ферзем/королем
        public void BlocksWay(int x, int y)
        {
            for (int i = 0; i < 8; i++)
            {
                if (NoWall(i, y))
                    GameField[i, y] = -7;
                else break;
            }
            for (int i = 0; i < 8; i++)
            {
                if (NoWall(x, i))
                    GameField[x, i] = -7;
                else break;
            }
            DiagonalMarks(x, y, -7, -7);
        }

        // проверка: король находится в смежной позиции с королем противника
        // x, y - координаты короля; xOpponent, yOpponent - координаты короля противника
        public bool AdjacentPosition(int x, int y, int xOpponent, int yOpponent)
        {
            if (GameField[x, y] == GameField[xOpponent - 1, yOpponent] ||
                GameField[x, y] == GameField[xOpponent - 1, yOpponent - 1] ||
                GameField[x, y] == GameField[xOpponent, yOpponent - 1] ||
                GameField[x, y] == GameField[xOpponent + 1, yOpponent - 1] ||
                GameField[x, y] == GameField[xOpponent + 1, yOpponent] ||
                GameField[x, y] == GameField[xOpponent + 1, yOpponent + 1] ||
                GameField[x, y] == GameField[xOpponent, yOpponent + 1] ||
                GameField[x, y] == GameField[xOpponent - 1, yOpponent + 1])
                return false;
            else return true;
        }

        // проверка: король должен покинуть квадрат за 16 ходов и больше не возвращаться в него
        // x, y - позиция короля, которую он займет при передвижении
        public bool LeaveSquare(int x, int y, Player player)
        {
            int Row = 0;
            int iterator = 1;
            if (player.Color == Color.Black)
            {
                Row += 7;
                iterator = -1;
            }
            if ((x, y) == (Row, 3) || (x, y) == (Row + iterator * 2, 3) || (x, y) == (Row + iterator * 3, 3) ||
                 (x, y) == (Row, 4) || (x, y) == (Row + iterator * 2, 4) || (x, y) == (Row + iterator * 3, 4) ||
                 (x, y) == (Row, 5) || (x, y) == (Row + iterator * 2, 5) || (x, y) == (Row + iterator * 3, 5))
                return false;
            return true;
        }


        // проверка, что король может сходить
        public bool OpportunityToMakeMoveKing(int x, int y, int xOpponent, int yOpponent, Player player, int motion)
        {
            if ((motion <= 16 || (motion > 16 && LeaveSquare(x, y, player))) &&
                AdjacentPosition(x, y, xOpponent, yOpponent) && 
                LeaveSquare(x, y, player) && 
                GameField[x, y] != -5 && 
                GameField[x, y] != -7)
                return true;
            else return false;
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

        // проверка: ферзь не может находиться на одной горизонтали дольше 5 ходов, если он не заблокирован (иначе поражение)
        public bool QueenOnHorizontal(int x, int y, int xNext, int yNext, int motion, bool flag)
        {
            if (QueenNotHorizontal(x, xNext, motion, flag))
                return true;
            else if (QueenIsNotLocked(x, y))
            {
                GameOver = true;
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

        // пометки диагоналей
        public void DiagonalMarks(int x, int y, int mark1, int mark2)
        {
            int i = x, j = y;
            if (GameField.IsEmpty(i+1, j+1))
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
                if (NoWall(i, j))
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
                if (NoWall(i, j))
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
                if (NoWall(i, j))
                {
                    GameField[i, j] = mark2;
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
                if (NoWall(i, j))
                {
                    GameField[i, j] = mark2;
                    i--;
                    j++;
                }
                else break;
            }
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
