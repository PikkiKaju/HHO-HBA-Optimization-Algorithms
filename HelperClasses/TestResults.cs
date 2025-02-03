

public class TestResults
{
    public IOptimizationAlgorithm Algorithm { get; set; } = null;
    public IFitnessFunction Function { get; set; } = null;
    public int PopulationSize { get; set; } = 0;
    public int Iterations { get; set; } = 0;
    // Result of the fitness function
    public double ResultF { get; set; } = double.MaxValue;
    // The best result of the algorithm
    public double[] ResultX { get; set; } = null;
    public double Mean { get; set; } = double.MaxValue;
    public double StandardDeviation { get; set; } = double.MaxValue;
    public double CoefficientOfVariation { get; set; } = double.MaxValue;

    public string ToString(int roundingDigits = 2)
    {
        if (Algorithm is not null && Function is not null)
        {
            string foundMin = "[";
            foreach (var x in ResultX)
            {
                foundMin += string.Format("{0:F" + roundingDigits + "}", Math.Round(x, roundingDigits).ToString());
                if (x != ResultX[ResultX.Length - 1])
                {
                    foundMin += ", ";
                }
            }
            foundMin.Remove(foundMin.Length - 2, 2);
            foundMin += "]";

            double mean = Math.Round(Mean, roundingDigits);
            double stdDev = Math.Round(StandardDeviation, roundingDigits);
            double cofVar = Math.Round(CoefficientOfVariation, roundingDigits);
            double best = Math.Round(ResultF, roundingDigits);

            string str = "";

            str += ($"Algorytm: {Algorithm.Name}\r\n");
            str += ($"Funkcja: {Function.Name}, wielk. pop.: {PopulationSize}, iteracje: {Iterations} \r\n");
                //$"Domain: [{Function.DomainMin} ; {Function.DomainMax}], GlobalMin: {Function.GlobalMin}\r\n");
            str += ($"Średnia: {mean.ToString()}, odch. sta.: {stdDev.ToString()}, wsp. zmienn.: {cofVar.ToString()}%\r\n");
            str += ($"Najl.: {string.Format("{0:F" + roundingDigits + "}", best.ToString())} Zn. minimum: {foundMin}\r\n");
            str += ("--------------------------------------------------\r\n");

            return str;
        }
        else
        {
            throw new NullReferenceException();
        }
    }
}
