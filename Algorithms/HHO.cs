using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Drawing;

public class HarrisHawksOptimization : IOptimizationAlgorithm
{
    public string Name { get; set; }  = "HarrisHawksOptimization";

    // Property for the best individual
    public double[] XBest { get; set; } = new double[1];

    // Property for the best individual's value of the fitness function 
    public double FBest { get; set; }

    // Property for keeping track of the number of fitness function calls
    public int NumberOfEvaluationFitnessFunction { get; set; }

    private static Random random = new Random();

    // Individual Hawk class
    public class Hawk
    {
        public double[] Position;
        public double Fitness;

        public Hawk(int dimensions)
        {
            Position = new double[dimensions];
        }
    }

    // HarrisHawksOptimization algorithm
    public static OptimizationResult HHO(
        FitnessFunction fitnessFunction, 
        int populationSize, 
        int maxIterations,
        int dimensions, 
        double[] lowerBounds, 
        double[] upperBounds
    )
    {
        List<Hawk> population = new List<Hawk>();
        int numberOfEvaluationFitnessFunction = 0;

        Hawk bestHawk = new Hawk(dimensions);

        Func<double[], double> F = fitnessFunction.Function;

        // Population inicialization
        for (int i = 0; i < populationSize; i++)
        {
            Hawk hawk = new Hawk(dimensions);
            for (int d = 0; d < dimensions; d++)
                hawk.Position[d] = lowerBounds[d] + random.NextDouble() * (upperBounds[d] - lowerBounds[d]);
            hawk.Fitness = F(hawk.Position);
            population.Add(hawk);

            if (hawk.Fitness < double.MaxValue)
            {
                bestHawk = hawk;
            }
        }


        for (int t = 0; t < maxIterations; t++)
        {
            // Find the individual with the best position 
            foreach (var hawk in population)
            {
                if (hawk.Fitness < bestHawk.Fitness)
                    bestHawk = hawk;
            }

            foreach (var hawk in population)
            {
                double E0 = 2 * random.NextDouble() - 1;  // Initial hawk's energy
                double J = 2 * (1 - random.NextDouble());  // Jump Rabbit's jump strength
                double E = 2 * E0 * (1 - (double)t / maxIterations);  // Linear decrease in energy amount
                double q = random.NextDouble(); // Random number to decide which formula is going to be used

                // Exploration:
                //  X(t +1) =
                //    Xrand(t) − r1 | Xrand(t) − 2r2X(t) |        , q ≥ 0.5
                //    (Xrabbit(t) − Xm(t)) − r3(LB + r4(UB − LB)) , q < 0.5

                if (Math.Abs(E) >= 1)
                {
                    double[] Xrand = population[random.Next(population.Count)].Position;

                    if (q >= 0.5)
                    {
                        for (int d = 0; d < dimensions; d++)
                        {
                            double r1 = random.NextDouble();
                            double r2 = random.NextDouble();
                            hawk.Position[d] = Xrand[d] - r1 * Math.Abs(Xrand[d] - 2 * r2 * hawk.Position[d]);
                        }
                    }
                    else
                    {
                        for (int d = 0; d < dimensions; d++)
                        {
                            double r3 = random.NextDouble();
                            double r4 = random.NextDouble();
                            hawk.Position[d] = bestHawk.Position[d] - (bestHawk.Position[d] - hawk.Position[d]) - r3 * (lowerBounds[d] + r4 * (upperBounds[d] - lowerBounds[d]));
                        }
                    }
                }

                // Exploitation
                else
                {
                    double r = random.NextDouble(); // Random number to decide which formula is going to be used
                    double[] deltaX = new double[dimensions]; // Location vector ∆X(t)
                    for (int d = 0; d < dimensions; d++)
                    {
                        // ∆X(t) = Xrabbit(t) − X(t)
                        deltaX[d] = Math.Abs(bestHawk.Position[d] - hawk.Position[d]);
                    }

                    if (r >= 0.5 && Math.Abs(E) >= 0.5)
                    {
                        // Soft besiege
                        // X(t +1) = ∆X(t)−E|JXrabbit(t) − X(t)|
                        for (int d = 0; d < dimensions; d++)
                        {
                            hawk.Position[d] = deltaX[d] - E * Math.Abs(J * bestHawk.Position[d] - hawk.Position[d]);
                        }
                    }
                    else if (r >= 0.5 && Math.Abs(E) < 0.5)
                    {
                        // Hard besiege
                        // X(t +1) = Xrabbit(t) − E |∆X(t)|
                        for (int d = 0; d < dimensions; d++)
                        {
                            hawk.Position[d] = bestHawk.Position[d] - E * Math.Abs(deltaX[d]);
                        }
                    }
                    else if (r < 0.5 && Math.Abs(E) >= 0.5)
                    {
                        //  Y = Xrabbit(t) − E |JXrabbit(t) − X(t)|
                        double[] Y = HHOCalculation.CalculateY(bestHawk.Position, hawk.Position, E, J, dimensions);

                        //  Z = Y + S × LF(D)
                        double[] Z = HHOCalculation.CalculateZ(Y, dimensions);

                        // Soft besiege with progressive rapid dives
                        // X(t + 1) =
                        //    Y if F(Y) < F(X(t))
                        //    Z if F(Z) < F(X(t))
                        hawk.Position = F(Y) < F(hawk.Position) ? Y : Z;
                    }
                    else if (r < 0.5 && Math.Abs(E) < 0.5)
                    {   
                        //  Y = Xrabbit(t) − E |JXrabbit(t) − X(t)|
                        double[] Y = HHOCalculation.CalculateY(bestHawk.Position, hawk.Position, E, J, dimensions);

                        //  Z = Y + S × LF(D)
                        double[] Z = HHOCalculation.CalculateZ(Y, dimensions);

                        // Hard besiege with progressive rapid dives
                        // X(t + 1) =
                        //    Y if F(Y) < F(X(t))
                        //    Z if F(Z) < F(X(t))
                        hawk.Position = F(Y) < F(hawk.Position) ? Y : Z;
                    }
                }
                numberOfEvaluationFitnessFunction++;
                hawk.Fitness = F(hawk.Position); // Update the value of fitness function fot the best individual
            }
        }

        // Return the best result
        return new OptimizationResult
        {
            xBest = bestHawk.Position,
            fBest = bestHawk.Fitness,
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

        OptimizationResult result = HHO(fitnessFunction, populationSize, maxIterations, maxDimensions, lowerBounds, upperBounds);

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

public class HHOCalculation
{
    private static Random random = new Random();

    public static double[] CalculateY(double[] bestHawkPosition, double[] hawkPosition, double E, double J, int dimension)
    {
        double[] Y = new double[dimension];

        for (int d = 0; d < dimension; d++)
        {
            //  Y = Xrabbit(t) − E |JXrabbit(t) − X(t)|
            Y[d] = bestHawkPosition[d] - E * Math.Abs(J * bestHawkPosition[d] - hawkPosition[d]);
        }

        return Y;
    }

    public static double[] CalculateZ(double[] Y, int D)
    {
        double beta = 1.5;
        double[] S = GenerateRandomVector(D);
        double[] levyFlight = LevyFlight(D, beta);
        double[] Z = new double[D];

        // Z = Y + S×LF(D)
        for (int i = 0; i < D; i++)
        {
            Z[i] = Y[i] + S[i] * levyFlight[i];
        }

        return Z;
    }

    private static double[] GenerateRandomVector(int size)
    {
        //  wektor S z losowymi wartościami z zakresu (0, 1)
        double[] S = new double[size];
        for (int i = 0; i < size; i++)
        {
            S[i] = random.NextDouble();
        }
        return S;
    }

    private static double[] LevyFlight(int dimension, double beta)
    {
        double[] step = new double[dimension];
        // Instalacja pakietu MathNet.Numerics 

        //       ( Γ(1 + β) × sin( πβ / 2 )         )  ^  1
        //  σ =  ( -------------------------------  )     -
        //       ( Γ(1 + β / 2) × β × 2 (β - 1) / 2 )     β

        double sigma = Math.Pow(
            (MathNet.Numerics.SpecialFunctions.Gamma(1 + beta) * Math.Sin(Math.PI * beta / 2)) /
            (MathNet.Numerics.SpecialFunctions.Gamma((1 + beta) / 2) * beta * Math.Pow(2, (beta - 1) / 2)),
            1 / beta
        );

        for (int i = 0; i < dimension; i++)
        {
            // LF(x) = 0.01 × u × σ / |v| ^ (1 / β)
            double u = random.NextDouble() * sigma;
            double v = random.NextDouble();
            step[i] = 0.01 * (u / Math.Pow(Math.Abs(v), 1 / beta));
        }

        return step;
    }
}

