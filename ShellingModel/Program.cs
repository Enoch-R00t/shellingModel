using System.Runtime.CompilerServices;

namespace ShellingModel
{
    internal class Program
    {
        private static short gridWidth = 15;
        private static short gridHeight = 15;

        private static short currentMoves = 0;
        private static short totalMoves = 0;
        private static short gridIterations = 1;

        internal static ShellingGrid shellingGridOrig;
        internal static ShellingGrid shellingGrid;

        internal static readonly short affinity = 1;
        static void Main(string[] args)
        {
            SetUp();

            shellingGrid = new ShellingGrid(gridWidth, gridHeight);
            shellingGridOrig = new ShellingGrid(gridWidth, gridHeight);
            PopulateGrid();

            currentMoves = IterateGrid();
            totalMoves = currentMoves;

            Console.Clear();
            shellingGridOrig.WriteGrid();

            Console.WriteLine();
            shellingGrid.WriteGrid();

            while (currentMoves > 0)
            {
                gridIterations += 1;

                currentMoves = IterateGrid();
                totalMoves += currentMoves;

                Thread.Sleep(2000);

                Console.Clear();
                shellingGridOrig.WriteGrid();

                Console.WriteLine();
                shellingGrid.WriteGrid();
            }
            

           // Console.WriteLine();
            //Console.WriteLine($"Orig X:{shellingGridOrig.Count(TypeEnum.X)}");
            //Console.WriteLine($"Orig O:{shellingGridOrig.Count(TypeEnum.O)}");
            //Console.WriteLine($"Orig Blank:{shellingGridOrig.Count(TypeEnum.Blank)}");

           
       

            Console.WriteLine();
            Console.WriteLine($"Total number of elements:{gridWidth * gridHeight}");
            Console.WriteLine($"X:{shellingGrid.Count(TypeEnum.X)}");
            Console.WriteLine($"Y:{shellingGrid.Count(TypeEnum.O)}");
            Console.WriteLine($"Blank:{shellingGrid.Count(TypeEnum.Blank)}");
            Console.WriteLine($"Total swaps :{totalMoves}");
            Console.WriteLine($"Grid iterations:{gridIterations-1}");
            Console.Read();
        }

        static void SetUp()
        {
            Console.WriteLine("What grid dimensions? eg... 6,6");
            var gridDimensions = Console.ReadLine();
            var inputVal = gridDimensions.Split(',');
            short.TryParse(inputVal[0], out gridHeight);
            short.TryParse(inputVal[1], out gridWidth);
        }

        static short IterateGrid()
        {
            short x = 0;
            short y = 0;
            short moves = 0;

            while (y < shellingGrid.GridHeight)
            {
                while (x < shellingGrid.GridWidth)
                {
                    //Console.WriteLine($"Checking if we need to move: y:{y}, x:{x}");

                    if (shellingGrid.Grid[y, x].Type != TypeEnum.Blank)
                    {
                        if (!shellingGrid.Grid[y, x].HappyHere())
                        {
                            // Console.WriteLine($"Attempting to move y:{y},x:{x}: {shellingGrid.Grid[y, x]}");

                            if(shellingGrid.Grid[y, x].TryMove())
                            {
                                moves++;
                            }
                            
                        }
                        else
                        {
                            // Console.WriteLine("Im happy here thank you.");
                        }
                    }
                    x++;
                }
                x = 0;
                y++;
            }

            return moves;
        }



        static void PopulateGrid()
        {
            Array values = Enum.GetValues(typeof(TypeEnum));
            Random random = new Random();
            TypeEnum randomType;
            ShellingObject shellingObject;

            short w = 0;
            short h = 0;
            while (h < gridHeight)
            {
                while (w < gridWidth)
                {
                    randomType = (TypeEnum)values.GetValue(random.Next(values.Length));

                    switch (randomType)
                    {
                        case TypeEnum.X:
                            shellingObject = new X(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new X(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                        //case TypeEnum.Y:
                        //    shellingObject = new Y(affinity, w, h, ref shellingGrid);
                        //    shellingGrid.Grid[h, w] = shellingObject;
                        //    break;
                        case TypeEnum.O:
                            shellingObject = new O(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new O(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                        case TypeEnum.T:
                            shellingObject = new T(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new T(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                        case TypeEnum.Blank:
                            shellingObject = new Blank(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new Blank(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                    }

                    w++;
                }
                w = 0;
                h++;
            }
        }

    }


}
