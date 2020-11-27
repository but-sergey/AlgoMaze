using System;
using System.Collections.Generic;

namespace MapGeneration
{
    struct genCell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public bool Visited { get; set; }
        public int Value { get; set; }
    }

    class Program
    {
        static public int Rows = 19;
        static public int Cols = 49;
        static public Random rand = new Random();

        static public genCell[,] map = new genCell[Rows, Cols];

        static public void PrintMap(genCell[,] M)
        {
            for(var i = 0; i < M.GetLength(0); i++)
            {
                for(var j = 0; j < M.GetLength(1); j++)
                {
                    string s = String.Empty;
                    switch(M[i, j].Value)
                    {
                        case 0: s = " "; break;
                        default: s = "█"; break;
                    }
                    Console.Write($"{s}");
                }
                Console.WriteLine();
            }
        }

        static public void ClearMap(ref genCell[,] M)
        {
            for(var i = 0; i < M.GetLength(0); i++)
            {
                for(var j = 0; j<M.GetLength(1); j++)
                {
                    if((i % 2 != 0 && j % 2 != 0) && (i < Rows - 1))
                    {
                        M[i, j].Value = 0;
                    }
                    else
                    {
                        M[i, j].Value = -1;
                    }
                    M[i, j].Row = i;
                    M[i, j].Col = j;
                    M[i, j].Visited = false;
                }
            }
        }

        static public void RemoveWall(ref genCell[,] M)
        {
            genCell current = M[1, 1];
            current.Visited = true;

            Stack<genCell> stack = new Stack<genCell>();

            do
            {
                List<genCell> cells = new List<genCell>();

                int row = current.Row;
                int col = current.Col;

                if (row - 1 > 0 && !M[row - 2, col].Visited) cells.Add(M[row - 2, col]);
                if (col - 1 > 0 && !M[row, col - 2].Visited) cells.Add(M[row, col - 2]);

                if (row < Rows - 3 && !M[row + 2, col].Visited) cells.Add(M[row + 2, col]);
                if (col < Cols - 3 && !M[row, col + 2].Visited) cells.Add(M[row, col + 2]);

                if(cells.Count > 0)
                {
                    genCell selected = cells[rand.Next(cells.Count)];
                    RemoveCurrentWall(ref M, current, selected);

                    selected.Visited = true;
                    M[selected.Row, selected.Col].Visited = true;
                    stack.Push(selected);
                    current = selected;
                }
                else
                {
                    current = stack.Pop();
                }

            } while (stack.Count > 0);
        }

        static public void RemoveCurrentWall(ref genCell[,] M, genCell current, genCell selected)
        {
            if(current.Row == selected.Row)
            {
                if (current.Col > selected.Col)
                {
                    M[current.Row, current.Col - 1].Value = 0;
                }
                else
                {
                    M[selected.Row, selected.Col - 1].Value = 0;
                }
            }
            else
            {
                if (current.Row > selected.Row)
                {
                    M[current.Row - 1, current.Col].Value = 0;
                }
                else
                {
                    M[selected.Row - 1, selected.Col].Value = 0;
                }
            }
        }

        static void Main(string[] args)
        {
            ClearMap(ref map);
            RemoveWall(ref map);
            PrintMap(map);

            Console.ReadLine();
        }
    }
}
