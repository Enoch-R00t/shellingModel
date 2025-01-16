//using GAF;
//using GAF.Operators;
//using GAF.Threading;

//using ShellingModel.AbstractClasses;

//using System.Reflection.Metadata.Ecma335;

//namespace ShellingModel_GA.Operators
//{
//    internal class MutateWithShellingObjectSwap : IGeneticOperator
//    {
//        private double MutationProbability;

//        public MutateWithShellingObjectSwap(double mutationProbability)
//        // : base(mutationProbability)
//        {
//            // base.RequiresEvaluatedPopulation = false;
//            MutationProbability = mutationProbability;
//        }

//        public bool Enabled
//        {
//            get => true;
//            set => throw new NotImplementedException();
//        }
//        public bool RequiresEvaluatedPopulation { get; set; }

//        public int GetOperatorInvokedEvaluations()
//        {
//            return 0;
//        }

//        public void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnesFunctionDelegate)
//        {
//            if (currentPopulation.Solutions == null || currentPopulation.Solutions.Count == 0)
//            {
//                throw new ArgumentException("There are no Solutions in the current Population.");
//            }

//            if (newPopulation == null)
//            {
//                newPopulation = currentPopulation.CreateEmptyCopy();
//            }

//            //base.CurrentPopulation = currentPopulation;
//            //base.NewPopulation = newPopulation;
//            //base.FitnessFunction = fitnessFunctionDelegate;
//            //if (!base.Enabled)
//            //{
//            //    return;
//            //}

//            newPopulation.Solutions.Clear();
//            newPopulation.Solutions.AddRange(currentPopulation.Solutions);
//            foreach (Chromosome nonElite in newPopulation.GetNonElites())
//            {
//                if (nonElite == null || nonElite.Genes == null)
//                {
//                    throw new ArgumentException("The Chromosome is either null or the Chromosome's Genes are null.");
//                }

//                Mutate(nonElite);
//            }
//        }

//        protected void Mutate(Chromosome child)
//        {
//            if (RandomProvider.GetThreadRandom().NextDouble() <= MutationProbability)
//            {
//                List<int> swapPoints = GetSwapPoints(child);
//                Mutate(child, swapPoints[0], swapPoints[1]);
//            }
//        }

//        internal void Mutate(Chromosome chromosome, int first, int second)
//        {
//            Gene value = chromosome.Genes[first];
//            chromosome.Genes[first] = chromosome.Genes[second];
//            chromosome.Genes[second] = value;


//            var grid = ((ShellingObject)chromosome.Genes[first].ObjectValue).shellingGrid;

//            ShellingObject objVal = (ShellingObject)chromosome.Genes[first].ObjectValue;
//            int gridYVal = objVal.yLoc;
//            int gridXVal = objVal.xLoc;


//            chromosome.Genes[first].ObjectValue = chromosome.Genes[second].ObjectValue;



//            chromosome.Genes[second].ObjectValue = objVal;




//            chromosome.ClearFitness();
//            //chromosome.Fitness = 0.0;
//            //chromosome.FitnessNormalised = 0.0;
//        }

//        internal List<int> GetSwapPoints(Chromosome chromosome)
//        {
//            List<int> list = new List<int>();
//            int num = RandomProvider.GetThreadRandom().Next(chromosome.Genes.Count);
//            int num2 = 0;
//            while (num == num2 || num2 == 0)
//            {
//                num2 = RandomProvider.GetThreadRandom().Next(chromosome.Genes.Count);
//            }

//            list.Add(num);
//            list.Add(num2);
//            return list;
//        }
//    }
//}