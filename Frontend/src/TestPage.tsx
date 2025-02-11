import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const TestPage: React.FC = () => {
    const navigate = useNavigate();
    const [stop, setStop] = useState(false);
    const [isRunning, setIsRunning] = useState(false);
    const [isGeneratingReport, setIsGeneratingReport] = useState(false);
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

    const handleGenerateReport = async () => {
        setIsGeneratingReport(true);
        try {
            const response = await axios.get("https://localhost:7178/api/report/multi", {
                responseType: "text",
            });
            setReportData(response.data);
        } catch (error) {
            console.error("Error fetching the report:", error);
        }
        setIsGeneratingReport(false);
    };

    return (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", height: "100vh" }}>
            <h1>Test by function</h1>
            <button onClick={handleGenerateReport} disabled={isGeneratingReport || isRunning} style={{ marginTop: "20px", padding: "10px 20px", backgroundColor: (isGeneratingReport || isRunning) ? "gray" : "blue", color: "white", border: "none", cursor: (isGeneratingReport || isRunning) ? "not-allowed" : "pointer" }}>
                {isGeneratingReport ? "Generating..." : "Generate Report"}
            </button>
            {reportData && (
                <div style={{ marginTop: "20px", padding: "10px", border: "1px solid #ccc", width: "80%", overflow: "auto" }}>
                    <h2>Report Output</h2>
                    <pre style={{ whiteSpace: "pre-wrap", wordWrap: "break-word" }}>{reportData}</pre>
                </div>
            )}
        </div>
    );
};

export default TestPage;
