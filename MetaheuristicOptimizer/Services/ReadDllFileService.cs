using MetaheuristicOptimizer.Calculations.Algorithms;
using MetaheuristicOptimizer.Calculations.HelperClasses;
using System.Diagnostics;
using System.Reflection;

namespace MetaheuristicOptimizer.Services
{
    public class ReadDllFileService
    {
        public static List<IOptimizationAlgorithm> LoadAlgorithmsFromDll()
        {
            var algorithms = new List<IOptimizationAlgorithm>();
            string dllDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Algorithms");

            if (!Directory.Exists(dllDirectory))
            {
                throw new FileNotFoundException($"The Algorithms DLL directory was not found: {dllDirectory}");
            }

            var dllFiles = Directory.GetFiles(dllDirectory, "*.dll");

            if (dllFiles.Count() == 0)
            {
                throw new FileNotFoundException($"The Algorithms DLL directory does not contain any files: {dllDirectory}");
            }

            foreach (var dll in dllFiles)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dll);

                    var algorithmTypes = assembly.GetTypes()
                        .Where(t => typeof(IOptimizationAlgorithm).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in algorithmTypes)
                    {
                        if (Activator.CreateInstance(type) is IOptimizationAlgorithm algorithmInstance)
                        {
                            algorithms.Add(algorithmInstance);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception($"The DLL file(s) from the specified directory could not be read: {dllDirectory}");
                }
            }

            return algorithms;
        }

        public static List<IFitnessFunction> LoadFitnessFunctionsFromDll()
        {
            var functions = new List<IFitnessFunction>();
            string dllDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Functions");

            if (!Directory.Exists(dllDirectory))
            {
                throw new FileNotFoundException($"The Functions DLL directory was not found: {dllDirectory}");
            }

            var dllFiles = Directory.GetFiles(dllDirectory, "*.dll");

            if (dllFiles.Count() == 0)
            {
                throw new FileNotFoundException($"The Functions DLL directory does not contain any files: {dllDirectory}");
            }

            foreach (string dll in dllFiles)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dll);

                    var functionTypes = assembly.GetTypes()
                        .Where(t => typeof(IFitnessFunction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    Debug.Write(functionTypes.Count());

                    foreach (var type in functionTypes)
                    {
                        if (Activator.CreateInstance(type) is IFitnessFunction functionInstance)
                        {
                            if (!FitnessFunctions.List.Contains(functionInstance))
                            {
                                FitnessFunctions.List.Add(functionInstance);
                            }
                            functions.Add(functionInstance);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception($"The DLL file(s) from the specified directory could not be read: {dllDirectory}");
                }
            }

            return functions;
        }
    }
}
