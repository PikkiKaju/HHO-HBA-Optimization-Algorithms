import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const TestPage: React.FC = () => {
    const navigate = useNavigate();
    const [isRunning, setIsRunning] = useState(false);
    const [reportData, setReportData] = useState<string>("");
    const [options, setOptions] = useState<string[]>([]);
    const [algorithms, setAlgorithms] = useState<string[]>([]);

    const generateRange = (start: number, end: number, step: number) => {
        let range = [];
        for (let i = start; i <= end; i += step) {
            range.push(i);
        }
        return range;
    };

    const [formData, setFormData] = useState({
        id: 1,
        algorithmName: "",
        PopulationSizes: generateRange(10, 100, 10),
        iterations: generateRange(5, 50, 5),
        dimension: 1,
        fitnessFunctions: [] as string[],
        createdAt: new Date().toISOString(),
    });

    useEffect(() => {
        const fetchOptionsAndAlgorithms = async () => {
            try {
                const [functionsRes, algorithmsRes] = await Promise.all([
                    axios.get("https://localhost:7178/api/update/functions"),
                    axios.get("https://localhost:7178/api/update/algorithms"),
                ]);
                setOptions(functionsRes.data);
                setAlgorithms(algorithmsRes.data);
            } catch (error) {
                console.error("Error fetching functions and algorithms:", error);
            }
        };
        fetchOptionsAndAlgorithms();
    }, []);

    const handleStartTest = async () => {
        setIsRunning(true);
        try {
            await axios.post("https://localhost:7178/api/test/start", formData);
            setTimeout(async () => {
                await fetchReport();
                setIsRunning(false);
            }, 5000);
        } catch (error) {
            console.error("Error starting the test:", error);
            setIsRunning(false);
        }
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
    const fetchReport = async () => {
        try {
            const response = await axios.get("https://localhost:7178/api/report/single", {
                responseType: "text",
            });
            setReportData(response.data);
        } catch (error) {
            console.error("Error fetching the report:", error);
        }
    };

    return (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", height: "100vh" }}>
            <h1>Test by algorithm</h1>

            <p>Pick an Algorithm:</p>
            <div>
                {algorithms.map((algorithm) => (
                    <label key={algorithm} style={{ display: "block", margin: "5px 0" }}>
                        <input type="radio" name="algorithmName" value={algorithm} checked={formData.algorithmName === algorithm} onChange={(e) => setFormData({ ...formData, algorithmName: e.target.value })} />
                        {algorithm}
                    </label>
                ))}
            </div>

            <p>Pick Functions:</p>
            <div>
                {options.map((option) => (
                    <label key={option} style={{ display: "block", margin: "5px 0" }}>
                        <input type="checkbox" value={option} checked={formData.fitnessFunctions.includes(option)} onChange={() => {
                            setFormData((prev) => ({
                                ...prev,
                                fitnessFunctions: prev.fitnessFunctions.includes(option)
                                    ? prev.fitnessFunctions.filter((item) => item !== option)
                                    : [...prev.fitnessFunctions, option],
                            }));
                        }} />
                        {option}
                    </label>
                ))}
            </div>

            <button onClick={handleStartTest} disabled={isRunning} style={{ marginTop: "20px", padding: "10px 20px", backgroundColor: isRunning ? "gray" : "green", color: "white", border: "none", cursor: isRunning ? "not-allowed" : "pointer" }}>
                {isRunning ? "Running..." : "Start Test"}
            </button>

            {reportData && (
                <div style={{ marginTop: "20px", padding: "10px", border: "1px solid #ccc", width: "80%" }}>
                    <h2>Test Report</h2>
                    <pre>{reportData}</pre>
                </div>
            )}
            <button onClick={() => navigate("/")}>Go Back</button>
            <button onClick={handleGenerateReport} disabled={isRunning} style={{ marginTop: "20px", padding: "10px 20px", backgroundColor: isRunning ? "gray" : "blue", color: "white", border: "none", cursor: isRunning ? "not-allowed" : "pointer" }}>
        Generate Report
      </button>
        </div>
    );
};

export default TestPage;
