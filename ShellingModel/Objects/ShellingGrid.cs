﻿using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

using System.Text;

namespace ShellingModel.Objects
{
    internal class ShellingGrid
    {
        internal short GridWidth;
        internal short GridHeight;
        internal ShellingObject[,] Grid;

        public ShellingGrid(short GridWidth, short GridHeight)
        {
            this.GridWidth = GridWidth;
            this.GridHeight = GridHeight;
            Grid = new ShellingObject[GridHeight, GridWidth];
        }

        public int Count(TypeEnum type)
        {
            short x = 0;
            short y = 0;
            short count = 0;

            while (y < GridHeight)
            {
                while (x < GridWidth)
                {
                    if (Grid[y, x].Type == type)
                    {
                        count++;
                    }

                    x++;
                }
                x = 0;
                y++;
            }

            return count;
        }

        internal string[] StringifyGrid()
        {
            short w = 0;
            short h = 0;
            List<string> outputString = new List<string>();

            while (h < GridHeight)
            {
                StringBuilder sb = new StringBuilder();

                while (w < GridWidth)
                {
                    sb.Append(Grid[h, w].DisplayValue);
                    w++;
                }
                outputString.Add(sb.ToString());
                w = 0;
                h++;
            }

            return outputString.ToArray();
        }

        internal void WriteGrid()
        {
            short w = 0;
            short h = 0;

            while (h < GridHeight)
            {
                while (w < GridWidth)
                {
                    if (!Grid[h, w].HappyHere())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Grid[h, w]?.DisplayValue);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Grid[h, w]?.DisplayValue);
                        Console.ResetColor();
                    }

                    w++;
                }
                Console.WriteLine();
                w = 0;
                h++;
            }
        }
    }
}
