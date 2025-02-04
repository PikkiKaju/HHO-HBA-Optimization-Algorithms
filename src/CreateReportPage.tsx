import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const CreateReportPage: React.FC = () => {
  const navigate = useNavigate();
  const [message, setMessage] = useState('');
  const [filePath, setFilePath] = useState<string | null>(null);

  useEffect(() => {
    axios.get('https://localhost:7178/api/report/generate', { responseType: 'text' }) // Treat response as plain text
      .then(response => {
        setMessage('Report generated successfully!');
        setFilePath(response.data); // Store the file path
      })
      .catch(error => {
        console.error('Error:', error);
        setMessage('Failed to generate report.');
      });
  }, []);

  const handleDownload = () => {
    if (filePath) {
      // Assuming the backend serves files from the "reports" directory
      const downloadUrl = `https://localhost:7178/${filePath}`;
      window.open(downloadUrl, '_blank'); // Open file in a new tab
    }
  };

  return (
    <div style={{ textAlign: 'center', marginTop: '50px' }}>
      <h2>Create Report</h2>
      <p>{message}</p>
      {filePath && (
        <button onClick={handleDownload} style={{ padding: '10px 20px', cursor: 'pointer' }}>
          Download Report
        </button>
      )}
    </div>
  );
};

export default CreateReportPage;
