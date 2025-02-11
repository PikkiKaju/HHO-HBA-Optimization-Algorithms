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
        algorithmName: "",
        populationSize: [10, 100, 10],
        iterations: [5, 50, 5],
        dimension: 1,
        fitnessFunctions: [] as string[],
        createdAt: new Date().toISOString(),
    });

    useEffect(() => {
        const fetchOptionsAndAlgorithms = async () => {
            try {
                const [optionsRes, algorithmsRes] = await Promise.all([
                    axios.get("https://localhost:7178/api/options"),
                    axios.get("https://localhost:7178/api/algorithms"),
                ]);
                setOptions(optionsRes.data);
                setAlgorithms(algorithmsRes.data);
            } catch (error) {
                console.error("Error fetching options and algorithms:", error);
            }
        };
        fetchOptionsAndAlgorithms();
    }, []);

    const handleAlgorithmChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData((prev) => ({
            ...prev,
            algorithmName: e.target.value,
        }));
    };

    const handleFunctionChange = (option: string) => {
        setFormData((prev) => ({
            ...prev,
            fitnessFunctions: prev.fitnessFunctions.includes(option)
                ? prev.fitnessFunctions.filter((item) => item !== option)
                : [...prev.fitnessFunctions, option],
        }));
    };

    const handleRangeChange = (
        e: React.ChangeEvent<HTMLInputElement>,
        name: "populationSize" | "iterations",
        index: number
    ) => {
        const value = Number(e.target.value);
        setFormData((prev) => {
            const newRange = [...prev[name]];
            newRange[index] = value;
            return { ...prev, [name]: newRange };
        });
    };

    const handleSubmit = async () => {
        setIsRunning(true);
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
        } catch (error: any) {
            console.error("❌ Error:", error.response ? error.response.data : error.message);
        }
        setIsRunning(false);
    };

    return (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", height: "100vh" }}>
            <h1>Test by function</h1>
            <p>Pick an Algorithm:</p>
            <div>
                {algorithms.map((algorithm) => (
                    <label key={algorithm} style={{ display: "block", margin: "5px 0" }}>
                        <input type="radio" name="algorithmName" value={algorithm} checked={formData.algorithmName === algorithm} onChange={handleAlgorithmChange} />
                        {algorithm}
                    </label>
                ))}
            </div>

            <p>Pick Functions:</p>
            <div>
                {options.map((option) => (
                    <label key={option} style={{ display: "block", margin: "5px 0" }}>
                        <input type="checkbox" value={option} checked={formData.fitnessFunctions.includes(option)} onChange={() => handleFunctionChange(option)} />
                        {option}
                    </label>
                ))}
            </div>

            <p>Set parameters:</p>
            <label>
                Iterations (Start, End, Step):
                <input type="number" value={formData.iterations[0]} onChange={(e) => handleRangeChange(e, "iterations", 0)} />
                <input type="number" value={formData.iterations[1]} onChange={(e) => handleRangeChange(e, "iterations", 1)} />
                <input type="number" value={formData.iterations[2]} onChange={(e) => handleRangeChange(e, "iterations", 2)} />
            </label>
            <label>
                Population Size (Start, End, Step):
                <input type="number" value={formData.populationSize[0]} onChange={(e) => handleRangeChange(e, "populationSize", 0)} />
                <input type="number" value={formData.populationSize[1]} onChange={(e) => handleRangeChange(e, "populationSize", 1)} />
                <input type="number" value={formData.populationSize[2]} onChange={(e) => handleRangeChange(e, "populationSize", 2)} />
            </label>

            <button onClick={handleSubmit}>Start Test</button>
            <button onClick={() => navigate("/")}>Go Back</button>
        </div>
    );
};

export default TestPage;
