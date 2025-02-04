import { useEffect, useState } from "react";

interface TestResult {
  fitnessFunctionName: string;
  resultF: number;
  resultX: number[];
  mean: number;
  standardDeviation: number;
  coefficientOfVariation: number;
}

interface AlgorithmResult {
  id: string;
  algorithmName: string;
  populationSize: number;
  iterations: number;
  dimension: number;
  createdAt: string;
  testResults: TestResult[];
}

interface ApiResponse {
  message: string;
  result: AlgorithmResult;
}

const FetchDataComponent = () => {
  const [data, setData] = useState<ApiResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch("https://localhost:7178/api/algorithm/run-single") // Replace with actual API endpoint
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data: ApiResponse) => {
        setData(data);
        setLoading(false);
      })
      .catch((error) => {
        setError(error.message);
        setLoading(false);
      });
  }, []);

  if (loading) return <p className="text-center">Loading...</p>;
  if (error) return <p className="text-center text-red-500">Error: {error}</p>;

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white shadow-md rounded-lg">
      <h1 className="text-xl font-bold mb-4">Algorithm Execution Result</h1>
      <p className="text-gray-700">Message: {data?.message}</p>
      <div className="mt-4 border-t pt-4">
        <h2 className="text-lg font-semibold">Algorithm Details</h2>
        <p><strong>Name:</strong> {data?.result.algorithmName}</p>
        <p><strong>Population Size:</strong> {data?.result.populationSize}</p>
        <p><strong>Iterations:</strong> {data?.result.iterations}</p>
        <p><strong>Dimension:</strong> {data?.result.dimension}</p>
        <p><strong>Created At:</strong> {new Date(data?.result.createdAt).toLocaleString()}</p>
      </div>
      <div className="mt-4 border-t pt-4">
        <h2 className="text-lg font-semibold">Test Results</h2>
        {data?.result.testResults.map((test, index) => (
          <div key={index} className="mt-3 p-3 bg-gray-100 rounded-md">
            <p><strong>Fitness Function:</strong> {test.fitnessFunctionName}</p>
            <p><strong>Result F:</strong> {test.resultF}</p>
            <p><strong>Result X:</strong> {test.resultX.join(", ")}</p>
            <p><strong>Mean:</strong> {test.mean}</p>
            <p><strong>Standard Deviation:</strong> {test.standardDeviation}</p>
            <p><strong>Coefficient of Variation:</strong> {test.coefficientOfVariation}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default FetchDataComponent;