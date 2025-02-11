import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const TestPage: React.FC = () => {
  const navigate = useNavigate();
  const [stop, setStop] = useState(false);
  const [isRunning, setIsRunning] = useState(false);
  const [options, setOptions] = useState<string[]>([]);
  const [algorithms, setAlgorithms] = useState<string[]>([]);

  const [formData, setFormData] = useState({
    id: 1,
    FitnessFunction: "",
    AlgorithmName: [] as string[],
    createdAt: new Date().toISOString(),
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const optionsResponse = await axios.get("https://localhost:7178/api/options");
        const algorithmsResponse = await axios.get("https://localhost:7178/api/algorithms");
        
        setOptions(optionsResponse.data);
        setAlgorithms(algorithmsResponse.data);
      } catch (error) {
        console.error("Error fetching data from backend:", error);
      }
    };
    fetchData();
  }, []);

  const handleAlgorithmChange = (option: string) => {
    setIsRunning(true);
    setFormData((prev) => ({
      ...prev,
      AlgorithmName: prev.AlgorithmName.includes(option)
        ? prev.AlgorithmName.filter((item) => item !== option)
        : [...prev.AlgorithmName, option],
    }));
  };

  const handleFunctionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData((prev) => ({
      ...prev,
      FitnessFunction: e.target.value,
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
    setIsRunning(false);
  };

  const handleGenerateReport = async () => {
    try {
      const response = await axios.get("https://localhost:7178/api/report/multi", {
        responseType: "blob",
      });

      const fileURL = window.URL.createObjectURL(new Blob([response.data], { type: "application/pdf" }));
      const link = document.createElement("a");
      link.href = fileURL;
      link.setAttribute("download", "report.pdf");
      document.body.appendChild(link);
      link.click();
      link.parentNode?.removeChild(link);
      window.URL.revokeObjectURL(fileURL);
    } catch (error) {
      console.error("Error downloading the report:", error);
    }
  };

  return (
    <div style={{ position: "relative", display: "flex", flexDirection: "column", justifyContent: "center", alignItems: "center", height: "100vh" }}>
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

      <p>Pick Algorithms:</p>
      <div>
        {algorithms.map((algorithm) => (
          <label key={algorithm} style={{ display: "block", margin: "5px 0" }}>
            <input type="checkbox" value={algorithm} checked={formData.AlgorithmName.includes(algorithm)} onChange={() => handleAlgorithmChange(algorithm)} />
            {algorithm}
          </label>
        ))}
      </div>

      <p>Pick a function:</p>
      <div>
        {options.map((option) => (
          <label key={option} style={{ display: "block", margin: "5px 0" }}>
            <input type="radio" name="FitnessFunction" value={option} checked={formData.FitnessFunction === option} onChange={handleFunctionChange} />
            {option}
          </label>
        ))}
      </div>

      <button onClick={handleSubmit}>Start Test</button>
      <button onClick={() => navigate("/")}>Go Back</button>

      <button onClick={handleGenerateReport} disabled={isRunning} style={{ marginTop: "20px", padding: "10px 20px", backgroundColor: isRunning ? "gray" : "blue", color: "white", border: "none", cursor: isRunning ? "not-allowed" : "pointer" }}>
        Generate Report
      </button>
    </div>
  );
};

export default TestPage;
