namespace MetaheuristicOptimizer.Calculations.HelperClasses
{

    /// Class for storring fitness function information
    /// 
    /// Fields:
    /// - Name: function name
    /// - MinDomain: array of minimal values for the arguments for each dimension
    /// - MaxDomain: array of maximal values for the arguments for each dimension
    /// - MaxDimensions: maximal number of dimension for the function
    /// - GlobalMin: the global minimum of the function
    /// - Function: the fittness function
    public class FitnessFunction
    {
        public required string Name { get; set; }
        public required double[] MinDomain { get; set; }
        public required double[] MaxDomain { get; set; }
        public required int MaxDimensions { get; set; }
        public required int GlobalMin { get; set; }
        public required Func<double[], double> Function { get; set; }
    }
}
