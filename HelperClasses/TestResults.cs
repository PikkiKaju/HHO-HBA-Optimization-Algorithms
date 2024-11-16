
public class TestResults
{
    public required IOptimizationAlgorithm Algorithm { get; set; }
    public required TestFunctions.FunctionInfo Function { get; set; }
    public required int PopulationSize { get; set; }
    public required int Iterations { get; set; }
    public required double Result { get; set; }
    public required double Mean { get; set; }
    public required double StandardDeviation { get; set; }
    public required double CoefficientOfVariation { get; set; }
}
