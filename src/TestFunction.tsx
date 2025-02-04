import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const TestPage: React.FC = () => {
  const navigate = useNavigate();
  const [stop, setStop] = useState(false); // Stop state

  const [formData, setFormData] = useState({
    id: 1,
    algorithmName: "", // Single algorithm
    populationSize: 10,
    iterations: 5,
    dimension: 1,
    fitnessFunctions: [] as string[], // Multiple functions
    createdAt: new Date().toISOString(),
  });

  const options = ["Rastrigin", "Rosenbrock", "Sphere", "Beale", "Bukin", "Himmelblau"];
  const algorithms = ["HBA", "HHO"];

  // Handle single algorithm selection (radio buttons)
  const handleAlgorithmChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData((prev) => ({
      ...prev,
      algorithmName: e.target.value,
    }));
  };

  // Handle multiple fitness function selection (checkboxes)
  const handleFunctionChange = (option: string) => {
    setFormData((prev) => ({
      ...prev,
      fitnessFunctions: prev.fitnessFunctions.includes(option)
        ? prev.fitnessFunctions.filter((item) => item !== option) // Uncheck
        : [...prev.fitnessFunctions, option], // Check
    }));
  };

  const handleParameterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: Number(value),
    }));
  };

  const handleSubmit = async () => {
    try {
      const response = await axios.post("https://localhost:7178/api/algorithm/run-single", formData, {
        headers: { "Content-Type": "application/json" },
      });
      console.log("✅ Response:", response.data);
    } catch (error) {
      console.error("❌ Error:", error.response ? error.response.data : error.message);
    }
  };

  return (
    <div style={{ position: "relative", display: "flex", flexDirection: "column", justifyContent: "center", alignItems: "center", height: "100vh" }}>
      {/* Stop Button */}
      <button
        onClick={() => setStop(true)}
        style={{
          position: "absolute",
          top: "10px",
          right: "10px",
          backgroundColor: stop ? "red" : "#f8d7da",
          color: "black",
          padding: "10px 20px",
          border: "none",
          cursor: "pointer",
          fontWeight: "bold",
        }}
      >
        {stop ? "Stopped" : "Stop"}
      </button>

      <h1>Test by function</h1>

      {/* Single Algorithm Selection */}
      <p>Pick an Algorithm:</p>
      <div>
        {algorithms.map((algorithm) => (
          <label key={algorithm} style={{ display: "block", margin: "5px 0" }}>
            <input type="radio" name="algorithmName" value={algorithm} checked={formData.algorithmName === algorithm} onChange={handleAlgorithmChange} />
            {algorithm}
          </label>
        ))}
      </div>

      {/* Multiple Function Selection */}
      <p>Pick Functions:</p>
      <div>
        {options.map((option) => (
          <label key={option} style={{ display: "block", margin: "5px 0" }}>
            <input type="checkbox" value={option} checked={formData.fitnessFunctions.includes(option)} onChange={() => handleFunctionChange(option)} />
            {option}
          </label>
        ))}
      </div>

      {/* Input Fields */}
      <p>Set parameters:</p>
      <label>
        Number of Iterations:
        <input type="number" name="iterations" value={formData.iterations} onChange={handleParameterChange} />
      </label>
      <label>
        Population Size:
        <input type="number" name="populationSize" value={formData.populationSize} onChange={handleParameterChange} />
      </label>
      <label>
        Dimensions:
        <input type="number" name="dimension" value={formData.dimension} onChange={handleParameterChange} />
      </label>

      {/* Buttons */}
      <button onClick={handleSubmit}>Start Test</button>
      <button onClick={() => navigate("/")}>Go Back</button>
    </div>
  );
};

export default TestPage;
