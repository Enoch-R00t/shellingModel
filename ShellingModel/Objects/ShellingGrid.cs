using ShellingModel.AbstractClasses;
using ShellingModel.Enums;

namespace ShellingModel.Objects
{

    internal class ShellingGrid //: ICloneable
    {
        internal short GridWidth;
        internal short GridHeight;
        internal ShellingObject[,] Grid;

        public ShellingGrid(short GridHeight, short GridWidth)
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

        internal void WriteGrid()
        {
            short w = 0;
            short h = 0;

            while (h < GridHeight)
            {
                while (w < GridWidth)
                {
                    //if (!Grid[h, w].HappyHere())
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Red;
                    //    Console.Write(Grid[h, w]?.DisplayValue);
                    //    Console.ResetColor();
                    //}
                    //else
                    //{
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Grid[h, w]?.DisplayValue);
                        Console.ResetColor();
                   // }

                    w++;
                }
                Console.WriteLine();
                w = 0;
                h++;
            }
        }

        //public object Clone()
        //{
        //    var clonedShellingGrid = new ShellingGrid(GridWidth, GridHeight);
        //    clonedShellingGrid.Grid = new ShellingObject[GridHeight, GridWidth];   
            
        //    short x = 0, y = 0; 

        //    while(y < GridHeight)
        //    {
        //        while(x < GridWidth)
        //        {
        //            ShellingObject tempObject;

        //            switch (Grid[y,x].Type)
        //            {
        //                case TypeEnum.X:
        //                    tempObject = new X(Grid[y,x].Discomfortability, x, y, ref clonedShellingGrid);
        //                    clonedShellingGrid.Grid[y, x] = tempObject;
        //                    break;
        //                case TypeEnum.Y:
        //                    tempObject = new Y(Grid[y, x].Discomfortability, x, y, ref clonedShellingGrid);
        //                    clonedShellingGrid.Grid[y, x] = tempObject;
        //                    break;
        //                case TypeEnum.O:
        //                    tempObject = new O(Grid[y, x].Discomfortability, x, y, ref clonedShellingGrid);
        //                    clonedShellingGrid.Grid[y, x] = tempObject;
        //                    break;
        //                case TypeEnum.T:
        //                    tempObject = new T(Grid[y, x].Discomfortability, x, y, ref clonedShellingGrid);
        //                    clonedShellingGrid.Grid[y, x] = tempObject;
        //                    break;
        //                case TypeEnum.Blank:
        //                    tempObject = new Blank(Grid[y, x].Discomfortability, x, y, ref clonedShellingGrid);
        //                    clonedShellingGrid.Grid[y, x] = tempObject;
        //                    break;
        //            }

        //            x++;
        //        }
        //        x = 0;
        //        y++;
        //    }

        //    return clonedShellingGrid;
        //}
    }
}
