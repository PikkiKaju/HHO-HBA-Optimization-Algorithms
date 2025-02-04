//namespace MetaheuristicOptimizer.Calculations.HelperClasses
//{
//    public static class FitnessFunctions
//    {
//        // A list of fitness functions' info and the functions themselves
//        public static readonly List<FitnessFunction> List = new List<FitnessFunction>
//    {
//        new FitnessFunction
//        {
//            Name = "Rastrigin",
//            MinDomain = [-5.12],
//            MaxDomain = [5.12],
//            MaxDimensions = 0,
//            GlobalMin = 0,
//            Function = delegate(double[] x)
//            {
//                double A = 10.0;
//                double sum = 0.0;
//                for (int i = 0; i < x.Length; i++)
//                {
//                    sum += x[i] * x[i] - A * Math.Cos(2 * Math.PI * x[i]);
//                }
//                return A * x.Length + sum;
//            }
//        },
//        new FitnessFunction
//        {
//            Name = "Rosenbrock",
//            MinDomain = [-10],
//            MaxDomain = [10],
//            MaxDimensions = 0,
//            GlobalMin = 0,
//            Function = delegate(double[] x)
//            {
//                double sum = 0.0;
//                for (int i = 0; i < x.Length - 1; i++)
//                {
//                    sum += 100 * Math.Pow(x[i + 1] - x[i] * x[i], 2) + Math.Pow(1 - x[i], 2);
//                }
//                return sum;
//            }
//        },
//        new FitnessFunction
//        {
//            Name = "Sphere",
//            MinDomain = [-10],
//            MaxDomain = [10],
//            MaxDimensions = 0,
//            GlobalMin = 0,
//            Function = delegate(double[] x)
//            {
//                double sum = 0.0;
//                for (int i = 0; i < x.Length; i++)
//                {
//                    sum += x[i] * x[i];
//                }
//                return sum;
//            }
//        },
//        new FitnessFunction
//        {
//            Name = "Beale",
//            MinDomain = [-4.5, -10],
//            MaxDomain = [10, 4.5],
//            MaxDimensions = 2,
//            GlobalMin = 0,
//            Function = delegate(double[] x)
//            {
//                if (x.Length != 2) return 999;
//                return Math.Pow(1.5 - x[0] + x[0] * x[1], 2) + Math.Pow(2.25 - x[0] + x[0] * x[1] * x[1], 2) + Math.Pow(2.625 - x[0] + x[0] * x[1] * x[1] * x[1], 2);
//            }
//        },
//        new FitnessFunction
//        {
//            Name = "Bukin",
//            MinDomain = [-5, -10],
//            MaxDomain = [10, 5],
//            MaxDimensions = 2,
//            GlobalMin = 0,
//            Function = delegate(double[] x)
//            {
//                if (x.Length != 2) return 999;
//                return 100 * Math.Sqrt(Math.Abs(x[1] - 0.01 * x[0] * x[0])) + 0.01 * Math.Abs(x[0] + 10);
//            }
//        },
//        new FitnessFunction
//        {
//            Name = "Himmelblau",
//            MinDomain = [-5, -10],
//            MaxDomain = [10, 5],
//            MaxDimensions = 2,
//            GlobalMin = 0,
//            Function = delegate(double[] x)
//            {
//                if (x.Length != 2) return 999;
//                return Math.Pow(x[0] * x[0] + x[1] - 11, 2) + Math.Pow(x[0] + x[1] * x[1] - 7, 2);
//            }
//        },
//    };
//    }
//}
