import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const CreateReportPage: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get("https://localhost:7178/api/report/generate", {
        responseType: "blob", // ⚠️ Important! Expect a binary file
      })
      .then((response) => {
        // Create a Blob from the PDF Stream
        const fileURL = window.URL.createObjectURL(new Blob([response.data], { type: "application/pdf" }));

        // Create a temporary link element
        const link = document.createElement("a");
        link.href = fileURL;
        link.setAttribute("download", "report.pdf"); // File name
        document.body.appendChild(link);

        // Simulate a click to download the file
        link.click();

        // Clean up
        link.parentNode?.removeChild(link);
        window.URL.revokeObjectURL(fileURL);
      })
      .catch((error) => {
        console.error("Error downloading the report:", error);
      });
  }, []);

  return (
    <div>
      <h2>Generating Report...</h2>
      <p>Your report is being generated and will be downloaded automatically.</p>
      <button onClick={() => navigate("/")}>Go Back</button>
    </div>
  );
};

export default CreateReportPage;
