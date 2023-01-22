using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public abstract class Figure
    {
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }
        private Position offset { get; set; }
        protected Field GameField { get; set; }
        public abstract Color Color { get; }
        public Figure()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
            //this.GameField = GameField;
        }

        // метод перемещения, который перемещает блок на заданное количество строк и столбцов
        public void Move(int rows, int columns)
        {
            GameField[offset.Row, offset.Column] = 0;
            offset.Row += rows;
            offset.Column += columns;
            GameField[offset.Row, offset.Column] = Id;
        }

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

        private bool IsDetailFit(int row, int column)
        {
            if (!GameField.IsEmpty(this.offset.Row, this.offset.Column))
            {
                return false;
            }
            return true;
        }

        public void MoveDetailLeft()
        {
            //this.Move(0, -1);
            if (GameField.IsEmpty(this.offset.Row, this.offset.Column - 1))
            {
                this.Move(0, -1);
            }
        }

        public void MoveDetailRight()
        {
            //this.Move(0, 1);

            if (GameField.IsEmpty(this.offset.Row, this.offset.Column + 1))
            {
                this.Move(0, 1);
            }
        }

        public void MoveDetailDown()
        {
            if (GameField.IsEmpty(this.offset.Row + 1, this.offset.Column))
            {
                this.Move(1, 0);
                GameField.Draw();
            }
        }

        public void MoveDetailUp()
        {
            if (GameField.IsEmpty(this.offset.Row - 1, this.offset.Column))
            {
                this.Move(-1, 0);
            }
        }

        // метод сброса, который сбрасывает вращение и положение
        public void Reset()
        {
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
