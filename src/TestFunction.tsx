import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const TestPage: React.FC = () => {
  const navigate = useNavigate();
  const [stop, setStop] = useState(false); // Stop state

  const [formData, setFormData] = useState({
    id: 1,
    FitnessFunction: "", // Single function (backend expects string)
    AlgorithmName: [] as string[], // Multiple algorithms (backend expects List<string>)
    createdAt: new Date().toISOString(),
  });

  const options = ["Rastrigin", "Rosenbrock", "Sphere", "Beale", "Bukin", "Himmelblau"];
  const algorithms = ["HBA", "HBO"];

  // Handle multiple algorithm selection (checkboxes)
  const handleAlgorithmChange = (option: string) => {
    setFormData((prev) => ({
      ...prev,
      AlgorithmName: prev.AlgorithmName.includes(option)
        ? prev.AlgorithmName.filter((item) => item !== option) // Uncheck
        : [...prev.AlgorithmName, option], // Check
    }));
  };

  // Handle single fitness function selection (radio buttons)
  const handleFunctionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData((prev) => ({
      ...prev,
      FitnessFunction: e.target.value, // Backend expects a string
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

      {/* Multiple Algorithm Selection */}
      <p>Pick Algorithms:</p>
      <div>
        {algorithms.map((algorithm) => (
          <label key={algorithm} style={{ display: "block", margin: "5px 0" }}>
            <input type="checkbox" value={algorithm} checked={formData.AlgorithmName.includes(algorithm)} onChange={() => handleAlgorithmChange(algorithm)} />
            {algorithm}
          </label>
        ))}
      </div>

      {/* Single Fitness Function Selection */}
      <p>Pick a function:</p>
      <div>
        {options.map((option) => (
          <label key={option} style={{ display: "block", margin: "5px 0" }}>
            <input type="radio" name="FitnessFunction" value={option} checked={formData.FitnessFunction === option} onChange={handleFunctionChange} />
            {option}
          </label>
        ))}
      </div>

      {/* Buttons */}
      <button onClick={handleSubmit}>Start Test</button>
      <button onClick={() => navigate("/")}>Go Back</button>
    </div>
  );
};

export default TestPage;
