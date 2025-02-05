import React, { useState } from "react";
import { UploadCloud } from "lucide-react";
import { useNavigate } from "react-router-dom";

const DllUploader: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const navigate = useNavigate();
  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const selectedFile = event.target.files?.[0];
    if (selectedFile && selectedFile.name.endsWith(".dll")) {
      setFile(selectedFile);
    } else {
      alert("Please upload a valid .dll file");
      setFile(null);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="p-6 shadow-lg w-96 text-center bg-white rounded-lg">
        <h2 className="text-xl font-bold mb-4">Please upload your .dll file</h2>
        <input
          type="file"
          accept=".dll"
          className="hidden"
          id="file-upload"
          onChange={handleFileChange}
        />
        <label htmlFor="file-upload" className="cursor-pointer inline-block p-2 border border-gray-300 rounded-lg bg-gray-200 hover:bg-gray-300">
          <UploadCloud size={20} className="inline mr-2" /> Upload .dll File
        </label>
        {file && <p className="mt-3 text-sm text-gray-600">Selected: {file.name}</p>}
        <button onClick={() => navigate("/")}>Go Back</button>
      </div>
    </div>
  );
};

export default DllUploader;
