using System;
using System.Collections.Generic;

class TestFunctions {

    // Class for storring function info
    public class FunctionInfo
    {
        public string Name { get; set; }
        public double MinX { get; set; }
        public double MaxX { get; set; }
        public int GlobalMin { get; set; }
        public Func<double[], double> Function { get; set; }
    }

    // A list of test functions' info and the functions themselves
    public static readonly List<FunctionInfo> FunctionInfos = new List<FunctionInfo>
    {
        new FunctionInfo
        {
            Name = Rastrigin.Name,
            MinX = Rastrigin.MinX,
            MaxX = Rastrigin.MaxX,
            GlobalMin = Rastrigin.GlobalMin,
            Function = Rastrigin.func
        },
        new FunctionInfo
        {
            Name = Rosenbrock.Name,
            MinX = Rosenbrock.MinX,
            MaxX = Rosenbrock.MaxX,
            GlobalMin = Rosenbrock.GlobalMin,
            Function = Rosenbrock.func
        },
        new FunctionInfo
        {
            Name = Sphere.Name,
            MinX = Sphere.MinX,
            MaxX = Sphere.MaxX,
            GlobalMin = Sphere.GlobalMin,
            Function = Sphere.func
        },
        new FunctionInfo
        {
            Name = Beale.Name,
            MinX = Beale.MinX,
            MaxX = Beale.MaxX,
            GlobalMin = Beale.GlobalMin,
            Function = Beale.func
        },
        new FunctionInfo
        {
            Name = Bukin.Name,
            MinX = Bukin.MinX,
            MaxX = Bukin.MaxX,
            GlobalMin = Bukin.GlobalMin,
            Function = Bukin.func
        },
        new FunctionInfo
        {
            Name = Himmelblau.Name,
            MinX = Himmelblau.MinX,
            MaxX = Himmelblau.MaxX,
            GlobalMin = Himmelblau.GlobalMin,
            Function = Himmelblau.func
        },
    };

    // A list of test functions
    public static readonly List<Func<double[], double>> Functions = new List<Func<double[], double>>
    {
        Rastrigin.func,
        Rosenbrock.func,
        Sphere.func,
        Beale.func,
        Bukin.func,
        Himmelblau.func
    };

    public static class Rastrigin
    {
        public static string Name = "Rastrigin";
        public static double MinX = -5.12;
        public static double MaxX = 5.12;
        public static int GlobalMin = 0;

        public static double func(double[] x)
        {
            double A = 10.0;
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i] * x[i] - A * Math.Cos(2 * Math.PI * x[i]);
            }
            return A * x.Length + sum;
        }
    }

    public static class Rosenbrock
    {
        public static string Name = "Rosenbrock";
        public static double MinX = double.MinValue;
        public static double MaxX = double.MaxValue;
        public static int GlobalMin = 0;
        public static double func(double[] x)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length - 1; i++)
            {
                sum += 100 * Math.Pow(x[i + 1] - x[i] * x[i], 2) + Math.Pow(1 - x[i], 2);
            }
            return sum;
        }
    }

    public static class Sphere
    {
        public static string Name = "Sphere";
        public static double MinX = double.MinValue;
        public static double MaxX = double.MaxValue;
        public static int GlobalMin = 0;

        public static double func(double[] x)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i] * x[i];
            }
            return sum;
        }
    }

    public static class Beale
    {
        public static string Name = "Beale";
        public static double MinX = double.MinValue;
        public static double MaxX = double.MaxValue;
        public static int GlobalMin = 0;

        public static double func(double[] x)
        {
            if (x.Length != 2) return double.MaxValue;
            return Math.Pow(1.5 - x[0] + x[0] * x[1], 2) + Math.Pow(2.25 - x[0] + x[0] * x[1] * x[1], 2) + Math.Pow(2.625 - x[0] + x[0] * x[1] * x[1] * x[1], 2);
        }
    }
    
    public static class Bukin
    {
        public static string Name = "Bukin";
        public static double MinX = double.MinValue;
        public static double MaxX = double.MaxValue;
        public static int GlobalMin = 0;

        public static double func(double[] x)
        {
            if (x.Length != 2) return double.MaxValue;
            return 100 * Math.Sqrt(Math.Abs(x[1] - 0.01 * x[0] * x[0])) + 0.01 * Math.Abs(x[0] + 10);
        }
    }
    
    public static class Himmelblau
    {
        public static string Name = "Himmelblau";
        public static double MinX = double.MinValue;
        public static double MaxX = double.MaxValue;
        public static int GlobalMin = 0;
        public static double func(double[] x)
        {
            if (x.Length != 2) return double.MaxValue;
            return Math.Pow(x[0] * x[0] + x[1] - 11, 2) + Math.Pow(x[0] + x[1] * x[1] - 7, 2);
        }
    }
}