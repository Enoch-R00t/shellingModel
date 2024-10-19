using ShellingModel.AbstractClasses;
using ShellingModel.Enums;
using ShellingModel.Objects;

using System.Text.Json;

namespace ShellingModel
{
    // serialization classes
    public class Dimension
    {
        public string height { get; set; }
        public string width { get; set; }
    }

    public class Root
    {
        public List<Dimension> dimensions { get; set; }
        public List<string> variables { get; set; }
        public string values { get; set; }
    }
    // end serialization classes

    internal class Program
    {
        private static short gridWidth = 15;
        private static short gridHeight = 15;

        private static short currentMoves = 0;
        private static short totalMoves = 0;
        private static short gridIterations = 1;
        private static short randomizations = 0;

        internal static ShellingGrid shellingGridOrig;
        internal static ShellingGrid shellingGrid;

        private static Random rand;
        private static string primingString;
        private static List<string> variables;

        private static bool UseGeneralSolution = true;

        internal static readonly short affinity = 1;
        static void Main(string[] args)
        {
            GetSetupFromFile();

            //SetUp();

            //rand = new Random();

            shellingGrid = new ShellingGrid(gridWidth, gridHeight);
            shellingGridOrig = new ShellingGrid(gridWidth, gridHeight);

            PopulateGridFromFile();

            //PopulateGridRandom();

            //if (UseGeneralSolution)
            //{
            //    GeneralSolution();
            //}
            //else
            //{
                FindSolutionWithRandomization();
            //}

        }

        private static void GetSetupFromFile()
        {
            string filePath = "D:\\ga_inputFiles\\20b40.txt";

            var myJsonResponse = ReadInputFile<Root>(filePath);

            var height = myJsonResponse.dimensions[0].height;
            short.TryParse(height, out gridHeight);

            var width = myJsonResponse.dimensions[1].width;
            short.TryParse(width, out gridWidth);

            variables = myJsonResponse.variables;

            primingString = myJsonResponse.values;
        }

        public static T ReadInputFile<T>(string filePath)
        {
            string text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(text);
        }

        private static void FindSolutionWithRandomization()
        {
            currentMoves = IterateGrid();
            totalMoves = currentMoves;

            Console.Clear();
            shellingGridOrig.WriteGrid();

            Console.WriteLine();
            shellingGrid.WriteGrid();

            // make as many moves as we can with the current configured grid
            while (currentMoves > 0)
            {
                gridIterations += 1;

                currentMoves = IterateGrid();
                totalMoves += currentMoves;

                Thread.Sleep(1000);

                Console.Clear();
                shellingGridOrig.WriteGrid();

                Console.WriteLine();
                shellingGrid.WriteGrid();
            }

            // are we at an optimum solution? 
            while(!IsEverybodyHappy() && randomizations < 100)
            {
                // randomize
                RandomlySwapElements();

                currentMoves = IterateGrid();
                while (currentMoves > 0)
                {
                    gridIterations += 1;

                    currentMoves = IterateGrid();
                    totalMoves += currentMoves;

                    Thread.Sleep(1000);

                    Console.Clear();
                    shellingGridOrig.WriteGrid();

                    Console.WriteLine();
                    shellingGrid.WriteGrid();
                }

                // loop if needed
            }

            // Results and counts output
            //--------------------------------------------------- 
            Console.WriteLine();
            Console.WriteLine($"Total number of elements:{gridWidth * gridHeight}");

            Array values = Enum.GetValues(typeof(TypeEnum));
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{values.GetValue(i)}:{shellingGrid.Count((TypeEnum)values.GetValue(i))}");
            }

            Console.WriteLine($"Total swaps :{totalMoves}");
            Console.WriteLine($"Grid iterations:{gridIterations - 1}");
            Console.WriteLine($"Randomizations:{randomizations}");

            WriteOutputToFile();


            Console.Read();
        }

        static void WriteOutputToFile()
        {
           
            // Set a variable to the Documents path.
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "output_20b40_ShellingGrid.txt"),true))
            {
              
                outputFile.WriteLine("Results run:{0}", DateTime.Now);
                outputFile.WriteLine();
                outputFile.WriteLine("Original grid:");
                var origGrid = shellingGridOrig.StringifyGrid();
                foreach (string line in origGrid)
                    outputFile.WriteLine(line);

                outputFile.WriteLine();
                outputFile.WriteLine("Resultant grid:");
                var finalGrid = shellingGrid.StringifyGrid();
                foreach (string line in finalGrid)
                    outputFile.WriteLine(line);

                outputFile.WriteLine();
                outputFile.WriteLine($"Total number of elements:{gridWidth * gridHeight}");

                Array values = Enum.GetValues(typeof(TypeEnum));
                for (int i = 0; i < values.Length; i++)
                {
                    outputFile.WriteLine($"{values.GetValue(i)}:{shellingGrid.Count((TypeEnum)values.GetValue(i))}");
                }

                outputFile.WriteLine($"Total swaps :{totalMoves}");
                outputFile.WriteLine($"Grid iterations:{gridIterations - 1}");
                outputFile.WriteLine($"Randomizations:{randomizations}");
            }
        }

        static void GeneralSolution()
        {
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


            // Results and counts output
            //--------------------------------------------------- 
            Console.WriteLine();
            Console.WriteLine($"Total number of elements:{gridWidth * gridHeight}");

            Array values = Enum.GetValues(typeof(TypeEnum));
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{values.GetValue(i)}:{shellingGrid.Count((TypeEnum)values.GetValue(i))}");
            }

            Console.WriteLine($"Total swaps :{totalMoves}");
            Console.WriteLine($"Grid iterations:{gridIterations - 1}");
            Console.Read();
        }

        static void SetUp()
        {
            Console.WriteLine("What grid dimensions? eg... 6,6");
            var gridDimensions = Console.ReadLine();
            var inputVal = gridDimensions.Split(',');
            short.TryParse(inputVal[0], out gridHeight);
            short.TryParse(inputVal[1], out gridWidth);

            Console.WriteLine("Use the general solution? y/n");
            var useGeneralSolution = Console.ReadLine();
            UseGeneralSolution = useGeneralSolution == "y" ? true : false;            
        }

        static void RandomlySwapElements()
        {
            short x = 0;
            short y = 0;
            int maxI = gridHeight * gridWidth;
            rand = new Random();

            while (y < shellingGrid.GridHeight)
            {
                while (x < shellingGrid.GridWidth)
                {
                    var left = shellingGrid.Grid[y, x];
                    if (!left.HappyHere())
                    {
                        // find random new element thats not happy either
                        var notHappyEither = false;                        
                        short otherX = 0;
                        short otherY = 0;
                        while (!notHappyEither)
                        {
                            otherY = (short)rand.Next(0, gridHeight - 1);
                            otherX = (short)rand.Next(0, gridWidth - 1);
                            notHappyEither = !shellingGrid.Grid[otherY, otherX].HappyHere();
                                //||
                                //shellingGrid.Grid[otherY, otherX].Type == TypeEnum.Blank;
                        }

                        var temp = left;
                        //temp.yLoc = y;
                        //temp.xLoc = x;

                        shellingGrid.Grid[y, x] = shellingGrid.Grid[otherY, otherX];
                        shellingGrid.Grid[y, x].yLoc = y;
                        shellingGrid.Grid[y, x].xLoc = x;

                        shellingGrid.Grid[otherY, otherX] = temp;
                        shellingGrid.Grid[otherY, otherX].yLoc = otherY;
                        shellingGrid.Grid[otherY, otherX].xLoc = otherX;
                    }

                        x++;
                }

                x = 0;
                y++;
            }

            randomizations++;
        }

        static bool IsEverybodyHappy()
        {
            short x = 0;
            short y = 0;
            bool everyoneIsHappy = true;

            while (y < shellingGrid.GridHeight)
            {
                while (x < shellingGrid.GridWidth)
                {
                    //Console.WriteLine($"Checking if we need to move: y:{y}, x:{x}");

                    if (shellingGrid.Grid[y, x].Type != TypeEnum.Blank)
                    {
                        if (!shellingGrid.Grid[y, x].HappyHere())
                        {
                            return false;
                        }
                    }
                    x++;
                }
                x = 0;
                y++;
            }
            return true;
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

        static void PopulateGridFromFile()
        {
            ShellingObject shellingObject;

            short w = 0;
            short h = 0;
            short index = 0;

            char randomType;

            while (h < gridHeight)
            {
                while (w < gridWidth)
                {
                    randomType = primingString[index];

                    switch (randomType)
                    {
                        case 'X':
                            shellingObject = new X(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new X(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                        case 'Y':
                            shellingObject = new Y(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new Y(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                        case 'O':
                            shellingObject = new O(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new O(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                        case 'T':
                            shellingObject = new T(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new T(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                        case '_':
                            shellingObject = new Blank(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new Blank(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
                    }
                    index++;
                   w++;
                }
                w = 0;
                h++;
            }
        }

        static void PopulateGridRandom()
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
                        case TypeEnum.Y:
                            shellingObject = new Y(affinity, w, h, ref shellingGrid);
                            shellingGrid.Grid[h, w] = shellingObject;
                            shellingObject = new Y(affinity, w, h, ref shellingGridOrig);
                            shellingGridOrig.Grid[h, w] = shellingObject;
                            break;
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
