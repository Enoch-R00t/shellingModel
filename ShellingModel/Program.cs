using GAF;
using GAF.Extensions;
using GAF.Operators;

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
        private static short gridHeight = 15;
        private static short gridWidth = 15;
       
        internal static ShellingGrid primingShellingGrid;

        private static Random rand;

        private static double currentFitness = 0;

        internal static readonly short affinity = 1;

        static void Main(string[] args)
        {
            const double crossoverProbability = 0.85;
            const double mutationProbability = 0.08;
            const int elitismPercentage = 5;

            SetUp();

            rand = new Random();

            primingShellingGrid = new ShellingGrid(gridHeight,gridWidth);
            PopulateGrid(primingShellingGrid);

            var population = new Population();

            // iterate over the grid
            for(var p = 0; p < 100 /*gridWidth * gridWidth*/; p++)
            {

                // create a new chromosome containing
                // each indexed object from the grid.
                var chromosome = new Chromosome();
                int y=0, x = 0;

                while(y < gridHeight)
                {
                    while(x < gridWidth)
                    {                       
                        chromosome.Genes.Add(new Gene(primingShellingGrid.Grid[y, x].Clone()));
                        x++;
                    }
                    x = 0;
                    y++;
                }

                // randmize the genes and add to solutions collection
                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(5);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
                //, 
                //AllowDuplicates = false
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.2);
 
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;
            

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(Terminate);
        }

        private static double CalculateFitness( Chromosome chromosome)
        {
            return CalculateFitnessPercentage(ref chromosome);
        }


        public static bool Terminate(Population population,
           int currentGeneration, long currentEvaluation)
        {
            //return currentGeneration > 1000;
            return currentFitness >= 99;
        }

        static void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];

            currentFitness = fittest.Fitness;

            Console.WriteLine("Generation: {0}, Fitness: {1}", e.Generation, fittest.Fitness);

            // write the grid
            DisplayGrid(fittest);
            
            Console.WriteLine("Complete");
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            if (e.Generation % 100 == 0)
            {
                var fittest = e.Population.GetTop(1)[0];
                Console.Clear();
                DisplayGrid(fittest);
                Console.WriteLine("Generation: {0}, Fitness: {1}", e.Generation, string.Format("{0:0.0000000000}",fittest.Fitness));
            }                     
        }

      
        static void SetUp()
        {
            Console.WriteLine("What grid dimensions? eg... <height>,<width>");
            var gridDimensions = Console.ReadLine();
            var inputVal = gridDimensions.Split(',');
            short.TryParse(inputVal[0], out gridHeight);
            
            short.TryParse(inputVal[1], out gridWidth);

            //Console.WriteLine("Use the general solution? y/n");
            //var useGeneralSolution = Console.ReadLine();
            //UseGeneralSolution = useGeneralSolution == "y" ? true : false;            
        }

        
        static double CalculateFitnessPercentage(ref Chromosome chromosome)
        {
            double EveryOneIsHappy = gridHeight * gridWidth;
            double unHappy = 0;

            // we need to build a grid based on the straight list of genes
            int x = 0; int y = 0; int index = 0;

            var grid = new ShellingObject[gridHeight, gridWidth];
            
            while(y < gridHeight)
            {
                while(x < gridWidth)
                {
                    grid[y, x] = (ShellingObject)chromosome.Genes[index].ObjectValue;
                    x++;
                    index++;
                }
                x = 0;
                y++;
            }

            // now we can check each element and let the 
            // objects decide if they like where they are
             x = 0;
             y = 0;

            while (y < gridHeight)
            {
                while (x < gridWidth)
                {
                    //Console.WriteLine($"Checking if we need to move: y:{y}, x:{x}");

                    if (grid[y, x].Type != TypeEnum.Blank)
                    {
                        if (!grid[y, x].HappyHere(y, x, gridHeight, gridWidth, ref grid))
                        {
                            unHappy++;
                        }
                    }
                    x++;
                }
                x = 0;
                y++;
            }
           
            grid = null;

            return 1 - unHappy / EveryOneIsHappy;
        }

        static void DisplayGrid(Chromosome chromosome)
        {
            short w = 0;
            short h = 0;

            short y = 0, x = 0;

            int index =0;

            var grid = new ShellingObject[gridHeight, gridWidth];
            // build grid first
            while (y < gridHeight)
            {
                while (x < gridWidth)
                {
                    grid[y, x] = (ShellingObject)chromosome.Genes[index].ObjectValue;
                    x++;
                    index++;
                }
                x = 0;
                y++;
            }

            while (h < gridHeight)
            {
                while (w < gridWidth)
                {
                    if (!grid[h, w].HappyHere(h,w,gridHeight,gridWidth,ref grid))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(grid[h, w]?.DisplayValue);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(grid[h, w]?.DisplayValue);
                        Console.ResetColor();
                    }

                    w++;
                    index++;
                }
                Console.WriteLine();
                w = 0;
                h++;
            }
        }


        static void PopulateGrid(ShellingGrid primingShellingGrid)
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
                            shellingObject = new X(affinity);
                            primingShellingGrid.Grid[h, w] = shellingObject;
                            break;
                        case TypeEnum.Y:
                            //shellingObject = new Y(affinity);
                            //primingShellingGrid.Grid[h, w] = shellingObject;
                            //break;
                        case TypeEnum.O:
                            shellingObject = new O(affinity);
                            primingShellingGrid.Grid[h, w] = shellingObject;
                            break;
                        case TypeEnum.T:
                            shellingObject = new T(affinity);
                            primingShellingGrid.Grid[h, w] = shellingObject;
                            break;
                        case TypeEnum.Blank:
                            shellingObject = new Blank(affinity);
                            primingShellingGrid.Grid[h, w] = shellingObject;
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
