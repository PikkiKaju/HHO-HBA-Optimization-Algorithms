//using MetaheuristicOptimizer.Calculations.Algorithms;
//using System.Reflection;

//namespace MetaheuristicOptimizer.Calculations.HelperClasses
//{
//    public class AlgorithmLoader
//    {
//        // Loads all classes that implement IOptimizationAlgorithm from the current assembly.
//        public static List<IOptimizationAlgorithm> LoadAlgorithmsDefault()
//        {
//            var algorithms = new List<IOptimizationAlgorithm>();
//            var algorithmType = typeof(IOptimizationAlgorithm);
//            var types = Assembly.GetExecutingAssembly().GetTypes()
//                .Where(t => algorithmType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

//            if (types.Count() != 0)
//            {
//                foreach (var type in types)
//                {
//                    var algorithm = (IOptimizationAlgorithm)Activator.CreateInstance(type);
//                    algorithms.Add(algorithm);
//                    if (!RunAlgorithms.algorithms.Contains(algorithm))
//                    {
//                        RunAlgorithms.algorithms.Add(algorithm);
//                    }
//                }
//            }
//            else
//            {
//                throw new Exception($"The default DLL file(s) could not be read.");
//            }
//            return algorithms;
//        }

//        // Loads all classes that implement IOptimizationAlgorithm from the DLL in specified path.
//        public static List<IOptimizationAlgorithm> LoadAlgorithmsFromDll(string dllDirectory = "C:\\.Projects\\Visual_Studio_cs_projects\\Zastosowania-Sztucznej-Inteligencji\\DLLs\\OptimizationAlgorithms")
//        {
//            var algorithms = new List<IOptimizationAlgorithm>();

//            if (!Directory.Exists(dllDirectory))
//            {
//                throw new FileNotFoundException($"The specified DLL directory was not found: {dllDirectory}");
//            }

//            var dllFiles = Directory.GetFiles(dllDirectory, "*.dll");

//            if (dllFiles.Count() == 0)
//            {
//                throw new FileNotFoundException($"The specified DLL directory does not contain any files: {dllDirectory}");
//            }

//            foreach (var dll in dllFiles)
//            {
//                try
//                {
//                    Assembly assembly = Assembly.LoadFrom(dll);

//                    var algorithmTypes = assembly.GetTypes()
//                        .Where(t => typeof(IOptimizationAlgorithm).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

//                    foreach (var type in algorithmTypes)
//                    {
//                        if (Activator.CreateInstance(type) is IOptimizationAlgorithm algorithmInstance)
//                        {
//                            if (!RunAlgorithms.algorithms.Any(a => a.GetType() == algorithmInstance.GetType()))
//                            //if (!RunAlgorithms.algorithms.Contains(algorithmInstance))
//                            {
//                                RunAlgorithms.algorithms.Add(algorithmInstance);
//                                algorithms.Add(algorithmInstance);
//                            }
//                        }
//                    }
//                }
//                catch (Exception)
//                {
//                    throw new Exception($"The DLL file(s) from the specified directory could not be read: {dllDirectory}");
//                }
//            }

//            return algorithms;
//        }
//    }

//    // Add an optimization algorithm to the algorithms list.
//    public static bool AddAlgorithm(IOptimizationAlgorithm algorithm)
//        {
//            if (!RunAlgorithms.algorithms.Contains(algorithm))
//            {
//                RunAlgorithms.algorithms.Add(algorithm);
//                return true;
//            }
//            return false;
//        }
//    }
