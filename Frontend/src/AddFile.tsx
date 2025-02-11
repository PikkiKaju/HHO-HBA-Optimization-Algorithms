import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const CreateReportPage: React.FC = () => {
    const navigate = useNavigate();
    const [message, setMessage] = useState("");
    const [file, setFile] = useState<File | null>(null);
    const [uploadType, setUploadType] = useState<"function" | "algorithm" | "">("");

    useEffect(() => {
        axios
            .get("https://localhost:7178/api/report/generate")
            .then((response) => setMessage(response.data.message))
            .catch((error) => console.error("Error:", error));
    }, []);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            setFile(event.target.files[0]);
        }
    };

    const handleUpload = async () => {
        if (!file || !uploadType) {
            alert("Please select a file and choose what you are uploading (Function or Algorithm).");
            return;
        }

        const formData = new FormData();
        formData.append("file", file);

        const apiUrl =
            uploadType === "function"
                ? "https://localhost:7178/api/upload/function"
                : "https://localhost:7178/api/upload/algorithm";

        try {
            const response = await axios.post(apiUrl, formData, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            });
            console.log("✅ Upload Success:", response.data);
            alert(`Upload successful: ${uploadType}`);
        } catch (error) {
            console.error("❌ Upload Error:", error);
            alert("Upload failed, please try again.");
        }
    };

    return (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", padding: "20px" }}>
            <h2>Upload a DLL File</h2>
            {message && <p>{message}</p>}

            <p>Choose what you are uploading:</p>
            <label>
                <input
                    type="radio"
                    name="uploadType"
                    value="function"
                    checked={uploadType === "function"}
                    onChange={() => setUploadType("function")}
                />
                Function
            </label>
            <label>
                <input
                    type="radio"
                    name="uploadType"
                    value="algorithm"
                    checked={uploadType === "algorithm"}
                    onChange={() => setUploadType("algorithm")}
                />
                Algorithm
            </label>

            <input type="file" accept=".dll" onChange={handleFileChange} style={{ marginTop: "10px" }} />
            <button onClick={handleUpload} style={{ marginTop: "10px", padding: "10px", cursor: "pointer" }}>
                Upload DLL
            </button>

            <button onClick={() => navigate("/")} style={{ marginTop: "20px", padding: "10px", cursor: "pointer" }}>
                Go Back
            </button>
        </div>
    );
};

export default CreateReportPage;
