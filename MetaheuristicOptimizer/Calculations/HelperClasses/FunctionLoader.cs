//using System.Diagnostics;
//using System.Reflection;

//namespace MetaheuristicOptimizer.Calculations.HelperClasses
//{
//    public class FunctionLoader
//    {
//        // Loads the Fitness fiunctions from the current assembly.
//        public static List<IFitnessFunction> LoadFitnessFunctionsDefault()
//        {
//            foreach (var function in DefaultFitnessFunctions.List)
//            {
//                if (!RunAlgorithms.functions.Contains(function))
//                {
//                    RunAlgorithms.functions.Add(function);
//                }
//            }

//            return DefaultFitnessFunctions.List;
//        }

//        public static List<IFitnessFunction> LoadFitnessFunctionsFromDll(string dllDirectory = @".\\DLLs\\FitnessFunctions")
//        {
//            var functions = new List<IFitnessFunction>();

//            if (!Directory.Exists(dllDirectory))
//            {
//                throw new FileNotFoundException($"The specified DLL directory was not found: {dllDirectory}");
//            }

//            var dllFiles = Directory.GetFiles(dllDirectory, "*.dll");

//            if (dllFiles.Count() == 0)
//            {
//                throw new FileNotFoundException($"The specified DLL directory does not contain any files: {dllDirectory}");
//            }

//            foreach (string dll in dllFiles)
//            {
//                try
//                {
//                    Assembly assembly = Assembly.LoadFrom(dll);

//                    var functionTypes = assembly.GetTypes()
//                        .Where(t => typeof(IFitnessFunction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

//                    Debug.Write(functionTypes.Count());

//                    foreach (var type in functionTypes)
//                    {
//                        if (Activator.CreateInstance(type) is IFitnessFunction functionInstance)
//                        {
//                            if (!RunAlgorithms.functions.Contains(functionInstance))
//                            {
//                                RunAlgorithms.functions.Add(functionInstance);
//                                functions.Add(functionInstance);
//                            }
//                        }
//                    }
//                }
//                catch (Exception)
//                {
//                    throw new Exception($"The DLL file(s) from the specified directory could not be read: {dllDirectory}");
//                }
//            }

//            return functions;
//        }

//        public static bool AddFunction(IFitnessFunction function)
//        {
//            if (!RunAlgorithms.functions.Contains(function))
//            {
//                RunAlgorithms.functions.Add(function);
//                return true;
//            }
//            return false;
//        }
//    }
//}
