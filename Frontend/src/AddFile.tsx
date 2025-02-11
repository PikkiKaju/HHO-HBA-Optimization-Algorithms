import React, { useState } from 'react'; 
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useEffect } from 'react';

const CreateReportPage: React.FC = () => {
  const navigate = useNavigate();
  const [message, setMessage] = useState('');
  const [file, setFile] = useState<File | null>(null);

  useEffect(() => {
    axios.get('https://localhost:7178/api/report/generate')
      .then(response => setMessage(response.data.message))
      .catch(error => console.error('Error:', error));
  }, []);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      setFile(event.target.files[0]);
    }
  };

  const handleUpload = async () => {
    if (!file) {
      alert("Please select a file first");
      return;
    }
    
    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await axios.post("https://localhost:7178/api/upload/dll", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      console.log("✅ Upload Success:", response.data);
    } catch (error) {
      console.error("❌ Upload Error:", error);
    }
  };

  return (
    <div>
      {message}
      <input type="file" accept=".dll" onChange={handleFileChange} />
      <button onClick={handleUpload}>Upload DLL</button>
    </div>
  );
};

export default CreateReportPage;
