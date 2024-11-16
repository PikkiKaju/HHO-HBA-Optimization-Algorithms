using System;
using System.Collections.Generic;

class HoneyBadgerAlgorithm : IOptimizationAlgorithm
{
    public string Name { get; set; } = "HoneyBadgerAlgorithm";

    public int PopulationSize { get; set; } = 30;
    public int MaxIterations { get; set; } = 100;
    public int Dimension { get; set; } = 30;

    // HBO algorithm's beta hyperparameter
    public double beta { get; set; } = 6;

    // HBO algorithm's C hyperparameter
    public double C { get; set; } = 2;

    // Property for the best individual
    public double[] XBest { get; set; }

    // Property for the best individual's value of the fitness function 
    public double FBest { get; set; }

    // Property for keeping track of the number of fitness function calls
    public int NumberOfEvaluationFitnessFunction { get; set; }

    private static Random random = new Random();

    // Individual Prey class
    public class Prey
    {
        public double[] Position;
        public double Fitness;

        public Prey(int dimension)
        {
            Position = new double[dimension];
        }
    }

    // Class constructor
    public HoneyBadgerAlgorithm(int populationSize = 30, int maxIterations = 100, int dimension = 30, double beta = 6, double C = 2)
    {
        this.PopulationSize = populationSize;
        this.MaxIterations = maxIterations;
        this.Dimension = dimension;
        this.beta = beta;
        this.C = C;
    }

    // HoneyBadgerAlgorithm algorithm
    public static OptimizationResult HBO(Func<double[], double> fitnessFunction, int populationSize = 30, int maxIterations = 100, int dimension = 30, double beta = 6, double C = 2)
    {
        List<Prey> population = new List<Prey>();
        int numberOfEvaluationFitnessFunction = 0;

        Prey bestPrey = new Prey(dimension);

        // Population inicialization
        for (int i = 0; i < populationSize; i++)
        {
            Prey prey = new Prey(dimension);
            for (int d = 0; d < dimension; d++)
                prey.Position[d] = random.NextDouble();
            prey.Fitness = fitnessFunction(prey.Position);
            population.Add(prey);

            if (prey.Fitness < double.MaxValue)
            {
                bestPrey = prey;
            }
        }

        for (int t = 0; t <= maxIterations; t++)
        {
            double alpha = HBOCalculation.UpdateDecreasingFactor(t, maxIterations);


            for (int i = 0; i < populationSize; i++)
            {
                // Calculate intensity vector for each dimension
                double[] I = HBOCalculation.CalculateIntensity(population[i].Position, bestPrey.Position, beta);

                // Create new position
                double[] xnew = new double[dimension];
                for (int d = 0; d < dimension; d++)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        xnew[d] = population[i].Position[d] + alpha * I[d] + C * (2 * random.NextDouble() - 1);
                    }
                    else
                    {
                        xnew[d] = bestPrey.Position[d] + alpha * I[d] + C * (2 * random.NextDouble() - 1);
                    }
                }

                // Evaluate the new position
                double fnew = fitnessFunction(xnew);
                numberOfEvaluationFitnessFunction++;

                // Update the prey's position and fitness if better
                if (fnew <= population[i].Fitness)
                {
                    population[i].Position = xnew;
                    population[i].Fitness = fnew;
                }

                // Update the global best position if this is the best seen so far
                if (fnew <= bestPrey.Fitness)
                {
                    bestPrey.Position = (double[])xnew.Clone();
                    bestPrey.Fitness = fnew;
                }
            }
        }

        // Return the best result
        return new OptimizationResult
        {
            xBest = bestPrey.Position,
            fBest = bestPrey.Fitness,
            numberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunction
        };
    }

    public double Solve(Func<double[], double> fitnessFunction, int populationSize = 30, int maxIterations = 100, int dimension = 30)
    {
        OptimizationResult result = HBO(fitnessFunction, populationSize, maxIterations, dimension, beta, C);

        // Assign the value of the optimization result to the globacl class properties
        XBest = result.xBest;
        FBest = result.fBest;
        NumberOfEvaluationFitnessFunction = result.numberOfEvaluationFitnessFunction;

        return FBest;
    }

    public void Print()
    {
        Console.WriteLine("Najlepsza pozycja królika:\n");
        foreach (var coord in XBest)
        {
            Console.WriteLine(coord);
        }
        Console.WriteLine($"\nWartość funkcji celu: {FBest}");
        Console.WriteLine($"\nIlość wywołań funkcji dopasowania: {NumberOfEvaluationFitnessFunction}");
    }
}

public class HBOCalculation
{
    public static double UpdateDecreasingFactor(int t, int maxIterations)
    {
        return 1.0 - (double)t / maxIterations;
    }
    public static double CalculateIntensity(double xi, double xprey, double beta)
    {
        return beta * Math.Abs(xi - xprey);
    }

    public static double[] CalculateIntensity(double[] xi, double[] xprey, double beta)
    {
        int dimension = xi.Length;
        double[] intensity = new double[dimension];

        for (int d = 0; d < dimension; d++)
        {
            intensity[d] = beta * Math.Abs(xi[d] - xprey[d]);
        }

        return intensity;
    }
}