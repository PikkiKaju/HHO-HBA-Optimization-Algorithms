import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const TestPage: React.FC = () => {
    const navigate = useNavigate();
    const [stop, setStop] = useState(false);
    const [isRunning, setIsRunning] = useState(false);
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
        populationSizes: generateRange(10, 100, 10),
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
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", height: "100vh" }}>
            <h1>Test by function</h1>
            <button onClick={handleGenerateReport} disabled={isRunning} style={{ marginTop: "20px", padding: "10px 20px", backgroundColor: isRunning ? "gray" : "blue", color: "white", border: "none", cursor: isRunning ? "not-allowed" : "pointer" }}>
                Generate Report
            </button>
        </div>
    );
};

export default TestPage;
