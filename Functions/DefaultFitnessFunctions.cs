
public static class DefaultFitnessFunctions
{
    public static List<IFitnessFunction> List { get; } = new List<IFitnessFunction>
    {
        new SphereFunction(),
        new RosenbrockFunction(),
        new RastriginFunction(),
        new BealeFunction(),
        new BukinFunction(),
        new HimmelblauFunction()
    };
}


public class SphereFunction : IFitnessFunction
{
    public string Name { get; set; } = "Sphere";
    public double[] MinDomain { get; set; } = { -5.12, -5.12, -5.12 };
    public double[] MaxDomain { get; set; } = { 5.12, 5.12, 5.12 };
    public int MaxDimensions { get; set; } = 3;
    public double GlobalMin { get; set; } = 0;
    public Func<double[], double> Function { get; set; } = x => x.Sum(xi => xi * xi);
}

public class RosenbrockFunction : IFitnessFunction
{
    public string Name { get; set; } = "Rosenbrock";
    public double[] MinDomain { get; set; } = { -5, -5, -5 };
    public double[] MaxDomain { get; set; } = { 10, 10, 10 };
    public int MaxDimensions { get; set; } = 3;
    public double GlobalMin { get; set; } = 0;
    public Func<double[], double> Function { get; set; } = x =>
    {
        double sum = 0;
        for (int i = 0; i < x.Length - 1; i++)
        {
            sum += 100 * Math.Pow(x[i + 1] - x[i] * x[i], 2) + Math.Pow(x[i] - 1, 2);
        }
        return sum;
    };
}

public class RastriginFunction : IFitnessFunction
{
    public string Name { get; set; } = "Rastrigin";
    public double[] MinDomain { get; set; } = { -5.12, -5.12, -5.12 };
    public double[] MaxDomain { get; set; } = { 5.12, 5.12, 5.12 };
    public int MaxDimensions { get; set; } = 3;
    public double GlobalMin { get; set; } = 0;
    public Func<double[], double> Function { get; set; } = x =>
    {
        double sum = 10 * x.Length;
        for (int i = 0; i < x.Length; i++)
        {
            sum += x[i] * x[i] - 10 * Math.Cos(2 * Math.PI * x[i]);
        }
        return sum;
    };
}

public class BealeFunction : IFitnessFunction
{
    public string Name { get; set; } = "Beale";
    public double[] MinDomain { get; set; } = [-4.5, -10];
    public double[] MaxDomain { get; set; } = [10, 4.5];
    public int MaxDimensions { get; set; } = 2;
    public double GlobalMin { get; set; } = 0;
    public Func<double[], double> Function { get; set; } = x =>
    {
        if (x.Length != 2) return 999;
        return Math.Pow(1.5 - x[0] + x[0] * x[1], 2) + Math.Pow(2.25 - x[0] + x[0] * x[1] * x[1], 2) + Math.Pow(2.625 - x[0] + x[0] * x[1] * x[1] * x[1], 2);
    };
}
public class BukinFunction : IFitnessFunction
{
    public string Name { get; set; } = "Bukin";
    public double[] MinDomain { get; set; } = [-5, -10];
    public double[] MaxDomain { get; set; } = [10, 5];
    public int MaxDimensions { get; set; } = 2;
    public double GlobalMin { get; set; } = 0;
    public Func<double[], double> Function { get; set; } = x =>
    {
        if (x.Length != 2) return 999;
        return 100 * Math.Sqrt(Math.Abs(x[1] - 0.01 * x[0] * x[0])) + 0.01 * Math.Abs(x[0] + 10);
    };
}
public class HimmelblauFunction : IFitnessFunction
{
    public string Name { get; set; } = "Himmelblau";
    public double[] MinDomain { get; set; } = [-5, -10];
    public double[] MaxDomain { get; set; } = [10, 5];
    public int MaxDimensions { get; set; } = 2;
    public double GlobalMin { get; set; } = 0;
    public Func<double[], double> Function { get; set; } = x =>
    {
        if (x.Length != 2) return 999;
        return Math.Pow(x[0] * x[0] + x[1] - 11, 2) + Math.Pow(x[0] + x[1] * x[1] - 7, 2);
    };
}