
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Linq;
using MathNet.Numerics;

public class TestResults
{
    public IOptimizationAlgorithm Algorithm { get; set; } = null;
    public FitnessFunction Function { get; set; } = null;
    public int PopulationSize { get; set; } = 0;
    public int Iterations { get; set; } = 0;
    public double Result { get; set; } = double.MaxValue;
    public double Mean { get; set; } = double.MaxValue;
    public double StandardDeviation { get; set; } = double.MaxValue;
    public double CoefficientOfVariation { get; set; } = double.MaxValue;

    public string ToString(int roundingDigits = 2)
    {
        if (Algorithm is not null && Function is not null)
        {
            string str = "";

            str += ($"Algorithm: {Algorithm.Name}, Pop Size: {PopulationSize}, Iterations: {Iterations}\r\n");
            str += ($"Function: {Function.Name}, \r\n");
                //$"Domain: [{Function.DomainMin} ; {Function.DomainMax}], GlobalMin: {Function.GlobalMin}\r\n");
            str += ($"Mean: {Math.Round(Mean, roundingDigits)}, Std Dev: {Math.Round(StandardDeviation, roundingDigits)}, Coefficient of Variation: {Math.Round(CoefficientOfVariation, roundingDigits)}%\r\n");
            str += ($"Best: {string.Format("{0:F" + roundingDigits + "}", Math.Round(Result, roundingDigits))}\r\n");
            str += ("--------------------------------------------------\r\n");

            return str;
        }
        else
        {
            throw new NullReferenceException();
        }
    }
}
