using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IOptimizationAlgorithm
{
    // Nazwa algorytmu
    string Name { get; set; }

    // Metoda rozpoczynająca rozwiązywanie zagadnienia poszukiwania
    // minimum funkcji celu.
    // Na początku sprawdza, czy w lokalizacji jest plik ze stanem algorytmu
    // w odpowiednim formacie. Jeśli taki plik istnieje, wznawia obliczenia
    // dla tego stanu,
    // W przeciwnym razie zaczyna obliczenia od początku.
    // Zwraca wartość funkcji celu dla znalezionego rozwiązania
    // (najlepszego osobnika)
    double Solve(Func<double[], double> function, int populationSize, int maxIterations, int dimension);

    // Właściwość zwracająca tablicę z najlepszym osobnikiem
    double[] XBest { get; set; }

    // Właściwość zwracająca wartość funkcji dopasowania
    // dla najlepszego osobnika
    double FBest { get; set; }

    // Właściwość zwracająca liczbę wywołań funkcji dopasowania
    int NumberOfEvaluationFitnessFunction { get; set; }
}
