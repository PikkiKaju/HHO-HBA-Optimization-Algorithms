import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const TestPage: React.FC = () => {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    id: 1, // Static ID (can be dynamic if needed)
    algorithmName: '',
    populationSize: '', // Renamed from size_of_iterations
    iterations: '',
    dimension: '',
    fitnessFunctions: [] as string[],
    createdAt: new Date().toISOString(), // Automatically set timestamp
  });

  const options = ["Rastrigin", "Rosenbrock", "Sphere", "Beale", "Bukin", "Himmelblau"];
  const algorithms = ["HBA", "HBO"];

  const handleCheckboxChange = (option: string) => {
    setFormData((prev) => ({
      ...prev,
      fitnessFunctions: prev.fitnessFunctions.includes(option)
        ? prev.fitnessFunctions.filter((item) => item !== option)
        : [...prev.fitnessFunctions, option]
    }));
  };

  const handleAlgorithmChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData((prev) => ({
      ...prev,
      algorithmName: e.target.value
    }));
  };

  const handleParameterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: Number(value) // Convert input values to numbers
    }));
  };

  const handleSubmit = async () => {
    try {
      const response = await axios.post(
        "https://localhost:7178/api/algorithm/run-single",
        formData,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      console.log("✅ Response:", response.data);
    } catch (error) {
      console.error("❌ Error:", error.response ? error.response.data : error.message);
    }
  };

  return (
    <div style={{ display: "flex", flexDirection: "column", justifyContent: "center", alignItems: "center", height: "100vh" }}>
      <h1>Test</h1>
      <p>Pick an Algorithm:</p>
      <div>
        {algorithms.map((algorithm) => (
          <label key={algorithm} style={{ display: "block", margin: "5px 0" }}>
            <input 
              type="radio" 
              name="algorithm" 
              value={algorithm} 
              checked={formData.algorithmName === algorithm} 
              onChange={handleAlgorithmChange} 
            />
            {algorithm}
          </label>
        ))}
      </div>
      <p>Pick a function:</p>
      <div>
        {options.map((option) => (
          <label key={option} style={{ display: "block", margin: "5px 0" }}>
            <input
              type="checkbox"
              value={option}
              checked={formData.fitnessFunctions.includes(option)}
              onChange={() => handleCheckboxChange(option)}
            />
            {option}
          </label>
        ))}
      </div>
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
      <button onClick={() => navigate('/start-test')}>Start Test</button>
      <button onClick={() => navigate('/')}>Go Back</button>
    </div>
  );
};

export default TestPage;
