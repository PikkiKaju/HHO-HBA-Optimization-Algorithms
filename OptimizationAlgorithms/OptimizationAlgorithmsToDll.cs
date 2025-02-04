using MetaheuristicOptimizer.Calculations.Algorithms;
using MetaheuristicOptimizer.Calculations.HelperClasses;

namespace OptimizationAlgorithms
{
    public class OptimizationAlgorithmsToDll : IOptimizationAlgorithm
    {
        public string Name { get; set; } = "Honey Badger Algorithm";
        public double[] XBest { get; set; } = new double[1];
        public double FBest { get; set; }
        public int NumberOfEvaluationFitnessFunction { get; set; }

        private static Random random = new Random();

        static double[,] Initial(int pop, int dim, double[] ub, double[] lb)
        {
            double[,] X = new double[pop, dim];
            for (int i = 0; i < pop; i++)
                for (int j = 0; j < dim; j++)
                    X[i, j] = random.NextDouble() * (ub[j] - lb[j]) + lb[j];
            return X;
        }

        static double CalculateFitness(double[] X, IFitnessFunction fun) => fun.Function(X);

        static (double[] fitness, int[] index) SortFitness(double[] Fit)
        {
            var sorted = Fit
                .Select((value, index) => new { Value = value, Index = index })
                .OrderBy(x => x.Value)
                .ToArray();

            return (sorted.Select(x => x.Value).ToArray(), sorted.Select(x => x.Index).ToArray());
        }

        static double[,] SortPosition(double[,] X, int[] index)
        {
            int rows = X.GetLength(0);
            int cols = X.GetLength(1);
            double[,] Xnew = new double[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    Xnew[i, j] = X[index[i], j];

            return Xnew;
        }

        public static double[] Intensity(int pop, double[] GbestPosition, double[,] X)
        {
            double epsilon = 1e-15;
            double[] di = new double[pop];
            double[] S = new double[pop];
            double[] I = new double[pop];

            for (int j = 0; j < pop; j++)
            {
                double[] diff = ElementWiseSubtract(Vectorize(X, j), GbestPosition);
                di[j] = L2Norm(diff) + epsilon;

                if (j < pop - 1)
                {
                    double[] diffNext = ElementWiseSubtract(Vectorize(X, j), Vectorize(X, j + 1));
                    S[j] = L2Norm(diffNext) + epsilon;
                }
                else
                {
                    double[] diffFirst = ElementWiseSubtract(Vectorize(X, j), Vectorize(X, 0));
                    S[j] = L2Norm(diffFirst) + epsilon;
                }

                di[j] = Math.Pow(di[j], 2);
                S[j] = Math.Pow(S[j], 2);

                double n = random.NextDouble();
                I[j] = n * S[j] / (4 * Math.PI * di[j]);
            }

            return I;
        }

        private static double[] Vectorize(double[,] matrix, int rowIndex)
        {
            return Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[rowIndex, x]).ToArray();
        }

        private static double[] ElementWiseSubtract(double[] a, double[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Arrays must have the same length");

            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] - b[i];
            }
            return result;
        }

        private static double L2Norm(double[] vector) => Math.Sqrt(vector.Sum(x => x * x));

        public double Solve(IFitnessFunction fun, int pop, int maxIter, int dim)
        {
            double[] lb = Enumerable.Repeat(-10.0, dim).ToArray();
            double[] ub = Enumerable.Repeat(10.0, dim).ToArray();

            double[,] X = Initial(pop, dim, ub, lb);
            double[] fitness = new double[pop];
            double[] Curve = new double[maxIter];

            NumberOfEvaluationFitnessFunction = 0;

            for (int i = 0; i < pop; i++)
            {
                fitness[i] = CalculateFitness(Vectorize(X, i), fun);
                NumberOfEvaluationFitnessFunction++;
            }

            var (sortedFitness, sortIndex) = SortFitness(fitness);
            X = SortPosition(X, sortIndex);

            FBest = sortedFitness[0];
            XBest = Vectorize(X, 0);

            double[,] Xnew = new double[pop, dim];
            double C = 2;
            double beta = 6;
            int[] vecFlag = { 1, -1 };

            for (int t = 0; t < maxIter; t++)
            {
                double alpha = C * Math.Exp(-t / (double)maxIter);
                double[] I = Intensity(pop, XBest, X);

                for (int i = 0; i < pop; i++)
                {
                    double Vs = random.NextDouble();
                    int F = vecFlag[random.Next(2)];

                    for (int j = 0; j < dim; j++)
                    {
                        double di = XBest[j] - X[i, j];

                        if (Vs < 0.5)
                        {
                            double r3 = random.NextDouble();
                            double r4 = random.NextDouble();
                            double r5 = random.NextDouble();

                            Xnew[i, j] = XBest[j] +
                                            F * beta * I[i] * XBest[j] +
                                            F * r3 * alpha * di *
                                            Math.Abs(Math.Cos(2 * Math.PI * r4) * (1 - Math.Cos(2 * Math.PI * r5)));
                        }
                        else
                        {
                            double r7 = random.NextDouble();
                            Xnew[i, j] = XBest[j] + F * r7 * alpha * di;
                        }
                    }

                    double tempFitness = CalculateFitness(Vectorize(Xnew, i), fun);
                    NumberOfEvaluationFitnessFunction++;

                    if (tempFitness < fitness[i])
                    {
                        fitness[i] = tempFitness;
                        for (int d = 0; d < dim; d++)
                            X[i, d] = Xnew[i, d];
                    }
                }

                var (newFitness, newIndex) = SortFitness(fitness);

                if (newFitness[0] < FBest)
                {
                    FBest = newFitness[0];
                    XBest = Vectorize(X, newIndex[0]);
                }

                Curve[t] = FBest;
            }

            return FBest;
        }
    }
}
