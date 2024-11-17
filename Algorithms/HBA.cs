using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using static HoneyBadgerAlgorithm;

class HoneyBadgerAlgorithm : IOptimizationAlgorithm
{
    public string Name { get; set; } = "HoneyBadgerAlgorithm";

    // HBO algorithm's beta hyperparameter
    public double beta { get; set; } = 6;

    // HBO algorithm's C hyperparameter
    public double C { get; set; } = 2;

    // Property for the best individual
    public double[] XBest { get; set; } = new double[1];

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

        public Prey(int dimensions)
        {
            Position = new double[dimensions];
        }
    }

    // HoneyBadgerAlgorithm algorithm
    public static OptimizationResult HBA(
        FitnessFunction fitnessFunction, 
        int populationSize, 
        int maxIterations, 
        int dimensions,
        double[] lowerBounds,
        double[] upperBounds,
        double beta = 6,
        double C = 2
    )
    {
        List<Prey> population = new List<Prey>();
        int numberOfEvaluationFitnessFunction = 0;

        Prey bestPrey = new Prey(dimensions);

        // Population inicialization
        for (int i = 0; i < populationSize; i++)
        {
            Prey prey = new Prey(dimensions);
            for (int d = 0; d < dimensions; d++)
                prey.Position[d] = lowerBounds[d] + random.NextDouble() * (upperBounds[d] - lowerBounds[d]);
            prey.Fitness = fitnessFunction.Function(prey.Position);
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
                double[] xnew = new double[dimensions];
                for (int d = 0; d < dimensions; d++)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        xnew[d] = population[i].Position[d] + alpha * I[d] + C * (2 * random.NextDouble() - 1);
                    }
                    else
                    {
                        xnew[d] = bestPrey.Position[d] + alpha * I[d] + C * (2 * random.NextDouble() - 1);
                    }
                    // Ensure the new position is within the bounds
                    xnew[d] = Math.Max(lowerBounds[d], Math.Min(upperBounds[d], xnew[d]));
                }

                // Evaluate the new position
                double fnew = fitnessFunction.Function(xnew);
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

    public double Solve(FitnessFunction fitnessFunction, int populationSize = 30, int maxIterations = 100, int dimensions = 1)
    {
        int maxDimensions;
        if (fitnessFunction.MaxDimensions == 0) maxDimensions = dimensions;
        else maxDimensions = (dimensions <= fitnessFunction.MaxDimensions) ? dimensions : fitnessFunction.MaxDimensions;
        double[] lowerBounds = new double[maxDimensions]; // Lower bounds
        double[] upperBounds = new double[maxDimensions]; // Upper bounds

        for (int i = 0; i < maxDimensions; i++)
        {
            if (fitnessFunction.MaxDimensions == 0)
            {
                lowerBounds[i] = fitnessFunction.MinDomain[0];
                upperBounds[i] = fitnessFunction.MaxDomain[0];
            }
            else
            {
                lowerBounds[i] = fitnessFunction.MinDomain[i];
                upperBounds[i] = fitnessFunction.MaxDomain[i];
            }
        }

        OptimizationResult result = HBA(fitnessFunction, populationSize, maxIterations, maxDimensions, lowerBounds, upperBounds, beta, C);

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