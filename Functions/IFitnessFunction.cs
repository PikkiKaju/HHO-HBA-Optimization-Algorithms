
public interface IFitnessFunction
{
    string Name { get; set; }
    double[] MinDomain { get; set; }
    double[] MaxDomain { get; set; }
    int MaxDimensions { get; set; }
    double GlobalMin { get; set; }
    Func<double[], double> Function { get; set; }
}
