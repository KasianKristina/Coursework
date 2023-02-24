using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Test
    {
        public static void checkfun()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            
             player1.king.MoveBlock(7, 1);
            player1.queen.MoveBlock(5, 1);
            player2.queen.MoveBlock(3, 1);
            GameField.Draw();
            bool check = player1.Сompetitor.queen.CheckQueenAttack(player2.queen.offset.Row, player2.queen.offset.Column, player2.Сompetitor.king.offset.Row, player2.Сompetitor.king.offset.Column);
            Console.WriteLine(check);
        }
    }
}
