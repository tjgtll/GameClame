using System;
using System.Collections.Generic;
using System.Text;

namespace GameClame.Model
{
    class TetrisGrid
    {
        private readonly int[,] grid;   

        public int Rows { get; }

        public int Columns { get; }

        public int this[int r,int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public TetrisGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public bool IsInside(int r,int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool IsEmpty(int r,int c)
        {
            return IsInside(r, c) && grid[r, c] == 0;
        }

        public bool IsRowFull(int r)
        {
            for (int i = 0; i < Columns; i++)
            {
                if (grid[r,i] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int i = 0; i < Columns; i++)
            {
                if (grid[r, i] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearRow(int r)
        {
            for (int i = 0; i < Columns; i++)
            {
                grid[r, i] = 0;
            }
        }

        private void MoveRowDown(int r,int numRows)
        {
            for (int i = 0; i < Columns; i++)
            {
                grid[r + numRows, i] = grid[r, i];
                grid[r, i] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int i = Rows - 1; i >= 0; i--)
            {
                if (IsRowFull(i))
                {
                    ClearRow(i);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(i, cleared);
                }
            }

            return cleared;
        }

    }
}
