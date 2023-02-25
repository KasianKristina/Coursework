using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public abstract class Figure
    {
        protected Position StartOffset { get; }
        protected int Id { get; set; }
        public Position offset { get; set; }
        protected Field GameField { get; set; }
        protected Color Color { get; set; }

        // метод перемещения, который перемещает блок на заданное количество строк и столбцов
        public void Move(int rows, int columns)
        {
            GameField[offset.Row, offset.Column] = 0;
            offset.Row += rows;
            offset.Column += columns;
            GameField[offset.Row, offset.Column] = Id;
        }

        // метод премещения, который перемещает блок на заданную позицию
        public void MoveBlock(int rows, int columns)
        {
            if (GameField[rows, columns] >= 0)
            {
                GameField[offset.Row, offset.Column] = 0;
                offset.Row = rows;
                offset.Column = columns;
                GameField[offset.Row, offset.Column] = Id;
            }
        }
    }
}
