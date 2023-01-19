using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Field
    {
        private int[,] field;
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int this[int row, int column]
        {
            get
            {
                return field[row, column];
            }
            set
            {
                field[row, column] = value;
            }
        }

        public Field(int row, int column)
        {
            Rows = row;
            Columns = column;
            field = new int[row, column];
        }

        public Field Copy()
        {
            Field fnew = new Field(8, 8);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    fnew[i, j] = field[i, j];
                }
            }
            return fnew;
        }

    public void Draw()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Console.Write(field[i, j] + "   ");
            }
            Console.WriteLine();
        }
        Console.WriteLine('*' * 10);
    }

    // находится ли клетка внутри поля
    public bool IsInside(int row, int column)
    {
        if (row >= 0 && row < Rows
                && column >= 0 && column < Columns)
            return true;
        else return false;
    }

    // пустая ли клетка
    public bool IsEmpty(int row, int column)
    {
        if (IsInside(row, column) && field[row, column] == 0)
            return true;
        else return false;
    }

    public bool IsEmptyWave(int row, int column)
    {
        if (IsInside(row, column) && field[row, column] >= 0)
            return true;
        else return false;
    }

    // пустая ли строка
    public bool IsRowEmpty(int row)
    {
        for (int c = 0; c < Columns; c++)
        {
            if (field[row, c] != 0)
            {
                return false;
            }
        }
        return true;
    }
}
}
